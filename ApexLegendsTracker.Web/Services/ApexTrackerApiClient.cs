using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using ApexLegendsTracker.Shared;
using ApexLegendsTracker.Shared.Telemetry;
using Microsoft.JSInterop;

namespace ApexLegendsTracker.Web.Services;

public sealed class ApexTrackerApiClient : IApexTrackerApiClient
{
	// ChatSource is serialized as a readable string (e.g. "Knowledge") by the backend.
	private static readonly JsonSerializerOptions ChatJsonOptions = new()
	{
		PropertyNameCaseInsensitive = true,
		Converters = { new JsonStringEnumConverter() }
	};

	private readonly HttpClient _httpClient;
	private readonly IJSRuntime _jsRuntime;

	public ApexTrackerApiClient(HttpClient httpClient, IJSRuntime jsRuntime)
	{
		_httpClient = httpClient;
		_jsRuntime = jsRuntime;
	}

	public async Task<PlayerLookupResult> GetPlayerAsync(
		string playerName,
		string platform,
		CancellationToken cancellationToken = default)
	{
		string encodedPlatform = Uri.EscapeDataString(platform.Trim().ToUpperInvariant());
		string encodedPlayerName = Uri.EscapeDataString(playerName.Trim());
		string path = $"api/v1/players/{encodedPlatform}/{encodedPlayerName}";

		await TrackEventAsync(TelemetryEvents.PlayerLookupRequested, platform);

		HttpResponseMessage response;
		try
		{
			 response = await _httpClient.GetAsync(path, cancellationToken);
		}
		catch(Exception ex)
		{
			//Specifically communication exception, log for initial debugging purposes
			if(ex.Message == "TypeError: Failed to fetch")
			{
				await TrackEventAsync(TelemetryEvents.PlayerLookupFailed, platform, ex.Message);
				throw new Exception("Couldn't reach API. ApiClient location attempted:" + _httpClient.BaseAddress + path);
			}
			else
			{
				await TrackEventAsync(TelemetryEvents.PlayerLookupFailed, platform, ex.Message);
				throw;
			}
		}
		
		if (!response.IsSuccessStatusCode)
		{
			string errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
			await TrackEventAsync(TelemetryEvents.PlayerLookupFailed, platform, $"HTTP {(int)response.StatusCode}");
			throw new HttpRequestException(
				$"Backend request failed with status {(int)response.StatusCode}. Body: {errorBody}",
				null,
				response.StatusCode);
		}

		PlayerLookupResult? payload = await response.Content.ReadFromJsonAsync<PlayerLookupResult>(cancellationToken: cancellationToken);

		if (payload is null)
		{
			await TrackEventAsync(TelemetryEvents.PlayerLookupFailed, platform, "Empty response body");
			throw new HttpRequestException("Backend returned an empty response body.", null, HttpStatusCode.InternalServerError);
		}

		await TrackEventAsync(TelemetryEvents.PlayerLookupSucceeded, platform);

		return payload;
	}

	private async Task TrackEventAsync(string eventName, string platform, string? errorMessage = null)
	{
		var properties = new Dictionary<string, string> { [TelemetryProperties.Platform] = platform };
		if (errorMessage is not null)
		{
			properties[TelemetryProperties.ErrorMessage] = errorMessage;
		}

		await TrackEventAsync(eventName, properties);
	}

	// Chat events have no platform; never include the raw message text.
	private async Task TrackChatEventAsync(string eventName, string? errorMessage = null)
	{
		var properties = new Dictionary<string, string>();
		if (errorMessage is not null)
		{
			properties[TelemetryProperties.ErrorMessage] = errorMessage;
		}

		await TrackEventAsync(eventName, properties);
	}

	private async Task TrackEventAsync(string eventName, Dictionary<string, string> properties)
	{
		try
		{
			await _jsRuntime.InvokeVoidAsync("apexTelemetry.trackEvent", eventName, properties);
		}
		catch (JSException)
		{
			// Telemetry is best-effort; ignore interop failures (e.g. SDK not loaded).
		}
	}

	public Task<MapRotationResponse> GetMapRotationAsync(
		int version = 1,
		CancellationToken cancellationToken = default)
	{
		return GetAsync<MapRotationResponse>($"api/v1/map-rotation?version={version}", cancellationToken);
	}

	public Task<PredatorResponse> GetPredatorThresholdsAsync(CancellationToken cancellationToken = default)
	{
		return GetAsync<PredatorResponse>("api/v1/predator-thresholds", cancellationToken);
	}

	public async Task<ChatResponse> SendChatMessageAsync(ChatRequest request, CancellationToken cancellationToken = default)
	{
		await TrackChatEventAsync(TelemetryEvents.ChatRequested);

		HttpResponseMessage response;
		try
		{
			response = await _httpClient.PostAsJsonAsync("api/v1/chat", request, cancellationToken);
		}
	catch (Exception ex)
		{
			await TrackChatEventAsync(TelemetryEvents.ChatFailed, ex.Message);
			throw;
		}

		using (response)
		{
			if (!response.IsSuccessStatusCode)
			{
				string errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
				await TrackChatEventAsync(TelemetryEvents.ChatFailed, $"HTTP {(int)response.StatusCode}");
				throw new HttpRequestException(
					$"Backend request failed with status {(int)response.StatusCode}. Body: {errorBody}",
					null,
					response.StatusCode);
			}

			ChatResponse? payload = await response.Content.ReadFromJsonAsync<ChatResponse>(ChatJsonOptions, cancellationToken);
			if (payload is null)
			{
				await TrackChatEventAsync(TelemetryEvents.ChatFailed, "Empty response body");
				throw new HttpRequestException("Backend returned an empty response body.", null, HttpStatusCode.InternalServerError);
			}

			await TrackChatEventAsync(TelemetryEvents.ChatSucceeded);
			return payload;
		}
	}

	private async Task<TResponse> GetAsync<TResponse>(string path, CancellationToken cancellationToken)
		where TResponse : class
	{
		using HttpResponseMessage response = await _httpClient.GetAsync(path, cancellationToken);

		if (!response.IsSuccessStatusCode)
		{
			string errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
			throw new HttpRequestException(
				$"Backend request failed with status {(int)response.StatusCode}. Body: {errorBody}",
				null,
				response.StatusCode);
		}

		TResponse? payload = await response.Content.ReadFromJsonAsync<TResponse>(cancellationToken: cancellationToken);
		return payload ?? throw new HttpRequestException("Backend returned an empty response body.");
	}
}
