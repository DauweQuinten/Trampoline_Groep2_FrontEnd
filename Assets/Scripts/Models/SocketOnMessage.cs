using Newtonsoft.Json;

namespace Models
{
    public class SocketOnMessage
    {
        [JsonProperty("jump")] public Jump Jump { get; set; }
        [JsonProperty("button")] public Button Button { get; set; }
    }
}