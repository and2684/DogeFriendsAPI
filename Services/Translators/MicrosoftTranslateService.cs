using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace DogeFriendsAPI.Services
{
    public class MicrosoftTranslateService : ITranslateService
    {
        private readonly string _key;
        private readonly string _host;
        private readonly string _uri;

        public MicrosoftTranslateService(IConfiguration config)
        {
            _key = config["MicrosoftTranslated:Key"];
            _uri = config["MicrosoftTranslated:Uri"];
            _host = config["MicrosoftTranslated:Host"];
        }

        public async Task<string> Translate(string input)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_uri),
                Headers =
                {
                    { "X-RapidAPI-Key", _key },
                    { "X-RapidAPI-Host", _host }
                },
                Content = new StringContent("[{\"Text\": \"" + input + "\"}]")
                {
                    Headers = { ContentType = new MediaTypeHeaderValue("application/json") }
                }
            };

            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();

                return (GetBetween(body, "\"text\":\"", "\",\"to\"")); // Тут возвращался какой-то кривой json, пришлось его парсить как строку
            }
        }

        public static string GetBetween(string strSource, string strStart, string strEnd)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int Start, End;
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }

            return "";
        }
    }
}