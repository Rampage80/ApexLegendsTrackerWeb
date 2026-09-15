using ApexLegendsTracker.Shared;

namespace ApexLegendsTracker.Web.Services;

public interface IApexTrackerApiClient
{
	Task<PlayerLookupResult> GetPlayerAsync(string playerName, string platform, CancellationToken cancellationToken = default);

	Task<MapRotationResponse> GetMapRotationAsync(int version = 1, CancellationToken cancellationToken = default);

	Task<PredatorResponse> GetPredatorThresholdsAsync(CancellationToken cancellationToken = default);

	Task<ChatResponse> SendChatMessageAsync(ChatRequest request, CancellationToken cancellationToken = default);
}
