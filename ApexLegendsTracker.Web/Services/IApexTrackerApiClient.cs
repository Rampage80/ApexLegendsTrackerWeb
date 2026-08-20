using ApexLegendsTracker.Shared;

namespace ApexLegendsTracker.Web.Services;

public interface IApexTrackerApiClient
{
	Task<PlayerLookupResult> GetPlayerAsync(string playerName, string platform, CancellationToken cancellationToken = default);
}
