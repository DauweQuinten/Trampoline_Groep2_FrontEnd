using Newtonsoft.Json;

namespace Models
{
    public class Button
    {
        [JsonProperty("btn")] public bool[] BtnStates { get; set; }
    }
}