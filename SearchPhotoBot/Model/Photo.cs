using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace SearchPhotoBot.Model;

public class Photo
{
    [JsonPropertyName("id")] public int Id { get; set; }
    [JsonPropertyName("width")] public int Width { get; set; }
    [JsonPropertyName("height")] public int Height { get; set; }
    [JsonPropertyName("url")] public string Url { get; set; }
    [JsonPropertyName("photographer")] public string Photographer { get; set; }
    [JsonPropertyName("photographer_url")] public string PhotographerUrl { get; set; }
    [JsonPropertyName("photographer_id")] public int PhotographerId { get; set; }
    [JsonPropertyName("avg_color")] public string AvgColor { get; set; }
    [JsonPropertyName("src")] public Src Src { get; set; }
    [JsonPropertyName("liked")] public bool Liked { get; set; }
    [JsonPropertyName("alt")] public string Alt { get; set; }
}