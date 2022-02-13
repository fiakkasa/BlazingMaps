using Microsoft.AspNetCore.Components;
using System.Text.Json.Serialization;

namespace BlazingMaps.Models;

public record AddressLookup
{
    private string? _id;
    private MarkupString? _displayText;

    [JsonIgnore]
    public string Id => _id ??= $"{Lat}-{Lon}";

    [JsonIgnore]
    public MarkupString DisplayText => _displayText ??= new MarkupString(Text);

    [JsonPropertyName("lat")]
    public double Lat { get; init; }

    [JsonPropertyName("lon")]
    public double Lon { get; init; }

    [JsonPropertyName("display_name")]
    public string Text { get; init; } = string.Empty;
}