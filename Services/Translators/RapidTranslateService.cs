using System.Text.Json.Nodes;

namespace DogeFriendsAPI.Services
{
    public class RapidTranslateService : ITranslateService
    {
        private readonly string _key;
        private readonly string _host;
        private readonly string _uri;

        public RapidTranslateService(IConfiguration config)
        {
            _key = config["Rapid:Key"];
            _host = config["Rapid:Host"];
            _uri = config["Rapid:Uri"];
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
                    { "X-RapidAPI-Host", _host },
                },
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "q", input },
                    { "target", "ru" },
                    { "source", "en" },
                }),
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                var translateNode = JsonNode.Parse(body)!["data"]!["translations"]!.AsArray();
                return translateNode[0]!["translatedText"]!.GetValue<string>();
            }
        }
    }
}