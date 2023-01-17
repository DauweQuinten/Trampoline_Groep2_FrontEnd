using Newtonsoft.Json;

namespace Models
{
    public class Jump
    {
        [JsonProperty("force")] public float Force { get; set; }
        [JsonProperty("player")] public int Player { get; set; }
    }
}