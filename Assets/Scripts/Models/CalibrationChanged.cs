using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class CalibrationChanged
    {
        [JsonProperty("kinectIndex")]
        public int KinectIndex { get; set; }

        [JsonProperty("playerIndex")]
        public int PlayerIndex { get; set; }
    }
}
