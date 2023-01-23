using Newtonsoft.Json;

namespace Models
{
    public class Button
    {
        [JsonProperty("btn")] public Btn BtnStates { get; set; }
    }
    public class Btn
    {
        [JsonProperty("0")] public BtnValue BtnLeft { get; set; }
        [JsonProperty("1")] public BtnValue BtnRight { get; set; }
        [JsonProperty("both")] public BtnValue Both { get; set; }
    }

    public enum BtnValue
    {
        [JsonProperty("0")] Off,
        [JsonProperty("pressed")] Pressed,
        [JsonProperty("released")] Released
    }
    
}