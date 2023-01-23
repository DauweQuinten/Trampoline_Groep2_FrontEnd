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

        public static async Task AddScoreAsync(ScoreboardItem score)
        {
            string url = $"{_BASEURI}/scoreboard";
            using (HttpClient client = GetHttpClient())
            {
                try
                {
                    string json = JsonConvert.SerializeObject(score);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(url, content);
                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception($"unsuccesful POST to url:{url}, object:{json}");
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
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

        public static async Task GenerateImageAsync(ScoreboardItem score)
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
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
    }
}