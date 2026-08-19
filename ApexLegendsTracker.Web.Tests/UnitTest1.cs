using System.Net;
using ApexLegendsTracker.Web.Services;

namespace ApexLegendsTracker.Web.Tests;

public class UnitTest1
{
    [Fact]
    public async Task GetPlayerAsync_UsesExampleApiResponse()
    {
        string exampleJson = await File.ReadAllTextAsync(
            Path.Combine(AppContext.BaseDirectory, "ExampleAPIJsonReturns.json"));
        using var handler = new StubHttpMessageHandler(exampleJson);
        using var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://api.example.test/")
        };
        var client = new ApexTrackerApiClient(httpClient);

        var result = await client.GetPlayerAsync("Rampage80", "ps4");

        Assert.NotNull(result);
        Assert.Equal(HttpMethod.Get, handler.Method);
        Assert.Equal(
            "https://api.example.test/api/v1/players/PS4/Rampage80",
            handler.RequestUri?.ToString());
        Assert.Equal("Rampage80", result.Global.Name);
        Assert.Equal("PS4", result.Global.Platform);
        Assert.Equal(437, result.Global.Level);
        Assert.Equal("Gold", result.Global.Rank.RankName);
        Assert.Equal(7503, result.Global.Rank.RankScore);
        Assert.Equal("Sparrow", result.Legends.Selected.LegendName);
        Assert.Equal("Career Kills", result.Legends.Selected.Data[0].Name);
        Assert.Equal(20091, result.Legends.Selected.Data[0].Value);
    }

    private sealed class StubHttpMessageHandler : HttpMessageHandler
    {
        private readonly string _responseBody;

        public StubHttpMessageHandler(string responseBody)
        {
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

            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(_responseBody, System.Text.Encoding.UTF8, "application/json")
            });
        }

    }
}
