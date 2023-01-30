using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using UiScripts;

namespace Models
{
    public class SocketLedMessage
    {
        [JsonProperty("btnLed")] public LedMessage LedMessage { get; set; }
    }

    public class LedMessage
    {
        [JsonProperty("id")] public LedType Id { get; set; }

        [JsonProperty("led")] public LedValue Led { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum LedType
    {
        [JsonProperty("left")] Left,
        [JsonProperty("right")] Right
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum LedValue
    {
        [JsonProperty("ON")] On,
        [JsonProperty("OFF")] Off
    }
}