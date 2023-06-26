using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Models
{
    public class SocketOnMessage
    {
        [JsonProperty("jump")] public Jump Jump { get; set; }
        [JsonProperty("button")] public Button Button { get; set; }
        [JsonProperty("calibrationJumpDetected")] public CalibrationChanged CalibrationChanged { get; set; }
        [JsonProperty("type")] public string Type { get; set; }
        [JsonProperty("data")] public Data MessageData { get; set; }
        
        public class Data
        {
            [JsonProperty("type")] public string Type { get; set; }
            [JsonProperty("data")] [CanBeNull] public CalibrationData MessageData { get; set; }
            [JsonProperty("value")] public int? Value { get; set; }
            
            public class CalibrationData
            {
                [JsonProperty("middleIndex")] public int MiddleIndex { get; set; }
                [JsonProperty("leftIndex")] public int LeftIndex { get; set; }
                [JsonProperty("rightIndex")] public int RightIndex { get; set; }
                [JsonProperty("type")] public string Type { get; set; }
                [JsonProperty("value")] public int Value { get; set; }
            }
        }
    }
}