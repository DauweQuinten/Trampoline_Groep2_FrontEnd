using Newtonsoft.Json;

namespace Models
{
    public class Button
    {
        [JsonProperty("btnState")] public BtnState BtnState { get; set; }
    }

    public enum BtnState
    {
        [JsonProperty("both")] BOTH,
        [JsonProperty("left")] LEFT,
        [JsonProperty("right")] RIGHT,     
    }
}