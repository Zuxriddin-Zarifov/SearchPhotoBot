using System.Text.Json.Serialization;

namespace SearchPhotoBot.Model;

public class Src
{
    [JsonPropertyName("original")] public string Original { get; set; }
    [JsonPropertyName("large2x")] public string Large2x { get; set; }
    [JsonPropertyName("large")] public string Large { get; set; }
    [JsonPropertyName("medium")] public string Medium { get; set; }
    [JsonPropertyName("small")] public string Small { get; set; }
    [JsonPropertyName("portrait")] public string Portrait { get; set; }
    [JsonPropertyName("landscape")] public string Landscape { get; set; }
    [JsonPropertyName("tiny")] public string Tiny { get; set; }
}