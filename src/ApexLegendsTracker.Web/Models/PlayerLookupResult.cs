namespace ApexLegendsTracker.Web.Models;

public sealed class PlayerLookupResult
{
	public string PlayerName { get; init; } = string.Empty;

	public string Platform { get; init; } = string.Empty;

	public string RawJson { get; init; } = string.Empty;
}
