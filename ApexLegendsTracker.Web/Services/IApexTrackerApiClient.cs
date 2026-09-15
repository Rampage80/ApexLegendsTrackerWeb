using ApexLegendsTracker.Shared;

namespace ApexLegendsTracker.Web.Services;

public interface IApexTrackerApiClient
{
	Task<PlayerLookupResult> GetPlayerAsync(string playerName, string platform, CancellationToken cancellationToken = default);

	Task<MapRotationResult> GetMapRotationAsync(int version = 1, CancellationToken cancellationToken = default);

	Task<PredatorResult> GetPredatorThresholdsAsync(CancellationToken cancellationToken = default);

	Task<ChatResponse> SendChatMessageAsync(ChatRequest request, CancellationToken cancellationToken = default);
}
