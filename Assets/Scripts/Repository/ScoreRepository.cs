using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public static class ScoreRepository
    {
        private const string _BASEURI = "/";
        public static HttpClient GetHttpClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");

            return client;

        }
        public static async Task<List<ScoreboardItem>> GetScoresAsync()
        {
            string url = $"{_BASEURI}";
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
       public static async Task AddAsync(ScoreboardItem Score)
        {
            string url = $"{_BASEURI}";
            using (HttpClient client = GetHttpClient())
            {
                try
                {
                    string json = JsonConvert.SerializeObject(reg);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(url, content);
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


    }
}
