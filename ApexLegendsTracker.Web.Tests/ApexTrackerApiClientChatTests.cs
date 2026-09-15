using System.Net;
using System.Net.Http.Json;
using ApexLegendsTracker.Shared;
using ApexLegendsTracker.Web.Services;
using Microsoft.JSInterop;

namespace ApexLegendsTracker.Web.Tests;

public sealed class ApexTrackerApiClientChatTests
{
	[Fact]
	public async Task SendChatMessageAsync_PostsMessageAndReturnsReply()
	{
		using var handler = new StubChatHandler(HttpStatusCode.OK, "{\"reply\":\"Rotate early.\",\"source\":\"Knowledge\"}");
		using var httpClient = new HttpClient(handler) { BaseAddress = new Uri("https://api.example.test/") };
		var client = new ApexTrackerApiClient(httpClient, new NoOpJSRuntime());

		ChatResponse result = await client.SendChatMessageAsync(new ChatRequest("how do I rotate in ranked?"));

		Assert.Equal(HttpMethod.Post, handler.Method);
		Assert.Equal("https://api.example.test/api/v1/chat", handler.RequestUri?.ToString());
		Assert.Equal("Rotate early.", result.Reply);
		Assert.Equal(ChatSource.Knowledge, result.Source);
	}

	[Fact]
	public async Task SendChatMessageAsync_ThrowsOnUpstreamFailure()
	{
		using var handler = new StubChatHandler(HttpStatusCode.TooManyRequests, "{}");
		using var httpClient = new HttpClient(handler) { BaseAddress = new Uri("https://api.example.test/") };
		var client = new ApexTrackerApiClient(httpClient, new NoOpJSRuntime());

		HttpRequestException exception = await Assert.ThrowsAsync<HttpRequestException>(
			() => client.SendChatMessageAsync(new ChatRequest("what's a good loadout?")));

		Assert.Equal(HttpStatusCode.TooManyRequests, exception.StatusCode);
	}

	private sealed class StubChatHandler : HttpMessageHandler
	{
		private readonly HttpStatusCode _statusCode;
		private readonly string _responseBody;

		public StubChatHandler(HttpStatusCode statusCode, string responseBody)
		{
			_statusCode = statusCode;
			_responseBody = responseBody;
		}

		public HttpMethod? Method { get; private set; }

		public Uri? RequestUri { get; private set; }

		protected override Task<HttpResponseMessage> SendAsync(
			HttpRequestMessage request,
			CancellationToken cancellationToken)
		{
			Method = request.Method;
			RequestUri = request.RequestUri;

			return Task.FromResult(new HttpResponseMessage(_statusCode)
			{
				Content = new StringContent(_responseBody, System.Text.Encoding.UTF8, "application/json")
			});
		}
	}

	private sealed class NoOpJSRuntime : IJSRuntime
	{
		public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args) => default;

		public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args) => default;
	}
}
