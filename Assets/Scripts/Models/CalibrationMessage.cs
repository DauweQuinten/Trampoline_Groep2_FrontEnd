using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CalibrationStatus
{
    [JsonProperty("started")] STARTED,
    
    [JsonProperty("finished")] FINISHED,

    [JsonProperty("switchPlayer")] SWITCH_PLAYER,
}


public class CalibrationMessage
{
    public CalibrationMessage(CalibrationStatus status)
    {
        Status = status.ToString();
    }

    [JsonProperty("status")] public string Status { get; set; }

}