namespace DogeFriendsAPI.Services
{
    public class GoogleTranslateService : ITranslateService
    {
        private readonly string _url;
        public GoogleTranslateService(IConfiguration config)
        {
            _url = config["GoogleTranslate:Url"];
        }

        public async Task<string> Translate(string input)
        {
            return await TranslateText(input, "en|ru");
        }

        public async Task<string> TranslateText(string input, string languagePair)
        {
            var url = String.Format(_url, input, languagePair);
            var webClient = new HttpClient();
            var byteResult = await webClient.GetByteArrayAsync(url);
            var result = System.Text.Encoding.Default.GetString(byteResult);

            result = result.Substring(result.IndexOf("id=result_box") + 22, result.IndexOf("id=result_box") + 500);

            result = result.Substring(0, result.IndexOf("</div"));

            return result;
        }
    }
}