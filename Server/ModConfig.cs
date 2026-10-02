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
}
