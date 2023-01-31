using Newtonsoft.Json;

namespace Models
{
    public class SocketResetMessage
    {
        [JsonProperty("resetKinect")] public bool ResetKinect { get; set; }
    }
}