using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Models
{
    public class ScoreboardItem
    {
        [JsonProperty("username")]
        public string Username { get; set; }
        
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("score")]
        public int Score { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }
    }
}
