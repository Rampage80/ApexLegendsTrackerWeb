using ApexLegendsTracker.Shared;
using Microsoft.JSInterop;

namespace ApexLegendsTracker.Web.Services;

public sealed class PlayerLookupState
{
    private const string StorageKey = "apexTracker.playerLookupResult";
    private readonly IJSRuntime _jsRuntime;

    public PlayerLookupState(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public PlayerLookupResult? Result { get; set; }

    public async Task SaveAsync(PlayerLookupResult result)
    {
        Result = result;
        string serializedResult = System.Text.Json.JsonSerializer.Serialize(result);
        await _jsRuntime.InvokeVoidAsync("localStorage.setItem", StorageKey, serializedResult);
    }

    public async Task<PlayerLookupResult?> RestoreAsync()
    {
        if (Result is not null)
        {
            return Result;
        }

        string? serializedResult = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", StorageKey);
        if (string.IsNullOrWhiteSpace(serializedResult))
        {
            return null;
        }

        try
        {
            Result = System.Text.Json.JsonSerializer.Deserialize<PlayerLookupResult>(serializedResult);
        }
        catch (System.Text.Json.JsonException)
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", StorageKey);
            Result = null;
        }

        return Result;
    }
}
