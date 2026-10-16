using System.Text.Json.Serialization;

namespace makeshotgunsgreatagain;

public record ModConfig
{
    [JsonPropertyName("enableBotsUseFrag12")]
    public bool EnableBotsUseFrag12 { get; init; } = true;

    [JsonPropertyName("enableBotsUseDragonBreath")]
    public bool EnableBotsUseDragonBreath { get; init; } = true;

    [JsonPropertyName("enableDebugLogs")]
    public bool EnableDebugLogs { get; init; } = false;

    /// <summary>
    /// The config read by the main mod class (Preload + 2); the quest loaders run after it.
    /// </summary>
    public static ModConfig Current { get; set; } = new();
}
