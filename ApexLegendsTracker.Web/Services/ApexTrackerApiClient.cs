using System.Net;
using System.Net.Http.Json;
using ApexLegendsTracker.Shared;

namespace ApexLegendsTracker.Web.Services;

public sealed class ApexTrackerApiClient : IApexTrackerApiClient
{
	private readonly HttpClient _httpClient;

	public ApexTrackerApiClient(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<PlayerLookupResult> GetPlayerAsync(
		string playerName,
		string platform,
		CancellationToken cancellationToken = default)
	{
		string encodedPlatform = Uri.EscapeDataString(platform.Trim().ToUpperInvariant());
		string encodedPlayerName = Uri.EscapeDataString(playerName.Trim());
		string path = $"api/v1/players/{encodedPlatform}/{encodedPlayerName}";

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
				throw new Exception("Couldn't reach API. ApiClient location attempted:" + _httpClient.BaseAddress + path);
			}
			else
			{
				throw;
			}
		}
		
		if (!response.IsSuccessStatusCode)
		{
			string errorBody = await response.Content.ReadAsStringAsync(cancellationToken);
			throw new HttpRequestException(
				$"Backend request failed with status {(int)response.StatusCode}. Body: {errorBody}",
				null,
				response.StatusCode);
		}

		PlayerLookupResult? payload = await response.Content.ReadFromJsonAsync<PlayerLookupResult>(cancellationToken: cancellationToken);

		if (payload is null)
		{
			throw new HttpRequestException("Backend returned an empty response body.", null, HttpStatusCode.InternalServerError);
		}

		return payload;
	}
}
