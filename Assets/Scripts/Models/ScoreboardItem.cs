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
        // IEquatable<ScoreboardItem>
    {
        [JsonProperty("username")] public string Username { get; set; }

        [JsonProperty("id")] public int Id { get; set; }

        [JsonProperty("score")] public int Score { get; set; }

        [JsonProperty("date")] public DateTime Date { get; set; }

        // public string ImgUrl
        // {
        //     get { return $"http://127.0.0.1:3000/username/avatar/{this.Id}"; }
        // }
        //
        public Texture2D Img { get; set; }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            if (obj is ScoreboardItem otherItem)
                return otherItem.Score.CompareTo(Score);

            throw new ArgumentException("Object is not an Item");
        }

        public bool Equals(ScoreboardItem other)
        {
            if (other == null) return false;
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            ScoreboardItem other = obj as ScoreboardItem;
            if (other == null)
                return false;
            else
                return Equals(other);
        }
    }
}