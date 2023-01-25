using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Models
{
    public class ScoreboardItem : IComparable
    {
        [JsonProperty("username")]
        public string Username { get; set; }
        
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("score")]
        public int Score { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        public string ImgUrl
        {
            get { return $"http://127.0.0.1:3000/username/avatar/{this.Id}"; }
        }
        
        public Texture2D Img { get; set; }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            ScoreboardItem otherItem = obj as ScoreboardItem;
            if (otherItem != null)
                return otherItem.Score.CompareTo(this.Score);
            
            throw new ArgumentException("Object is not an Item");
        }
    }
}
