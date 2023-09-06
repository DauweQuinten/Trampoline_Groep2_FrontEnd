using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace Repository
{
    public static class ScoreRepository
    {
        private const string _BASEURI = "http://127.0.0.1:3000";

        private static HttpClient GetHttpClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            return client;
        }

        public static async Task<List<ScoreboardItem>> GetScoresAsync()
        {
            string url = $"{_BASEURI}/scoreboard";
            using (HttpClient client = GetHttpClient())
            {
                try
                {
                    string json = await client.GetStringAsync(url);
                    Debug.Log("/scoreboard - " + json);
                    if (json == null) return null;

                    List<ScoreboardItem> Scorelists = JsonConvert.DeserializeObject<List<ScoreboardItem>>(json);
                    return Scorelists;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static async Task<ScoreboardItem> GetScoreAsync(int id)
        {
            string url = $"{_BASEURI}/scoreboard/{id}";
            using (HttpClient client = GetHttpClient())
            {
                try
                {
                    string json = await client.GetStringAsync(url);

                    if (json == null) return null;

                    var scorelists = JsonConvert.DeserializeObject<ScoreboardItem>(json);
                    return scorelists;
                }
                catch (Exception ex)
                {
                    Debug.LogWarning("error gettings core");
                    throw;
                }
            }
        }

        public static async Task<int> AddScoreAsync(ScoreboardItem score)
        {
            var url = $"{_BASEURI}/scoreboard";
            using var client = GetHttpClient();
            try
            {
                var json = JsonConvert.SerializeObject(score);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"unsuccesful POST to url:{url}, object:{json}");
                }

                var body = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<IDResponse>(body);
                return result.Id;
            }
            catch (Exception ex)
            {
                Debug.Log($"Adding score failed {ex.Message}");
                throw new Exception("Adding score failed");
            }
        }

        public class IDResponse
        {
            [JsonProperty("id")] public int Id { get; set; }
        }

        public static async Task UpdateScoreAsync(ScoreboardItem score)
        {
            string url = $"{_BASEURI}/scoreboard";
            using (HttpClient client = GetHttpClient())
            {
                try
                {
                    string json = JsonConvert.SerializeObject(score);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PutAsync(url, content);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"unsuccesful PUT to url:{url}, object:{json}");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static async Task<string> UserNameGeneration()
        {
            string url = $"{_BASEURI}/username";
            using (HttpClient client = GetHttpClient())
            {
                try
                {
                    string json = await client.GetStringAsync(url);

                    if (json == null) return null;

                    return json;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static async Task<byte[]> GenerateImageAsync(ScoreboardItem score)
        {
            string url = $"{_BASEURI}/username/avatar";
            using (HttpClient client = GetHttpClient())
            {
                try
                {
                    string json = JsonConvert.SerializeObject(score);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(url, content);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"unsuccesful Image Generation to url:{url}, object:{json}");
                    }

                    // get the png from body
                    var body = await response.Content.ReadAsByteArrayAsync();
                    return body;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        public static async Task<byte[]> GetAvatar(int id)
        {
            try
            {
                var url = $"{_BASEURI}/username/avatar/{id}";
                using var client = GetHttpClient();
                var response = await client.GetAsync(url);
                return await response.Content.ReadAsByteArrayAsync();
            }
            catch (Exception e)
            {
                Debug.LogWarning(e);
                throw;
            }
        }
    }
}