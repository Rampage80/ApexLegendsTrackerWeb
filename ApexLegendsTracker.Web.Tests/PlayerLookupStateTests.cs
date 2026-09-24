using ApexLegendsTracker.Shared;
using ApexLegendsTracker.Web.Services;
using Microsoft.JSInterop;

namespace ApexLegendsTracker.Web.Tests;

public sealed class PlayerLookupStateTests
{
    [Fact]
    public async Task SaveAndRestore_PreservesPlayerResultAcrossStateInstances()
    {
        var jsRuntime = new InMemoryJSRuntime();
        var result = System.Text.Json.JsonSerializer.Deserialize<PlayerLookupResult>(
            "{\"playerName\":\"Rampage80\",\"platform\":\"PS4\"}")!;
        result.PlayerName = "Rampage80";
        result.Platform = "PS4";

        var firstState = new PlayerLookupState(jsRuntime);
        await firstState.SaveAsync(result);

        var refreshedState = new PlayerLookupState(jsRuntime);
        PlayerLookupResult? restored = await refreshedState.RestoreAsync();

        Assert.NotNull(restored);
        Assert.Equal("Rampage80", restored.PlayerName);
        Assert.Equal("PS4", restored.Platform);
        Assert.Equal(result.PlayerName, restored.PlayerName);
    }

    private sealed class InMemoryJSRuntime : IJSRuntime
    {
        private readonly Dictionary<string, string> _storage = new();

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, object?[]? args)
        {
            string key = args?[0]?.ToString() ?? string.Empty;
            object? value = identifier switch
            {
                "localStorage.getItem" => _storage.TryGetValue(key, out string? storedValue) ? storedValue : null,
                "localStorage.setItem" => SetItem(key, args),
                "localStorage.removeItem" => _storage.Remove(key) ? null : null,
                _ => null
            };

            return ValueTask.FromResult(value is null ? default! : (TValue)value);
        }

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, CancellationToken cancellationToken, object?[]? args)
        {
            return InvokeAsync<TValue>(identifier, args);
        }

        public ValueTask InvokeVoidAsync(string identifier, object?[]? args)
        {
            return new ValueTask(InvokeAsync<object?>(identifier, args).AsTask());
        }

        public ValueTask InvokeVoidAsync(string identifier, CancellationToken cancellationToken, object?[]? args)
        {
            return InvokeVoidAsync(identifier, args);
        }

        private object? SetItem(string key, object?[]? args)
        {
            if (args?.Length > 1)
            {
                _storage[key] = args[1]?.ToString() ?? string.Empty;
            }

            return null;
        }
    }
}