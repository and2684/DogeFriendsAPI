namespace DogeFriendsAPI.XmlSerialization
{
    // Пример того, как писать расширение для Results. Тут я добавил возможность возвращать результат в виде xml.
    public class XmlResult<T> : IResult
    {
        private static readonly XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
        private readonly T result;
        public XmlResult(T Result) => result = Result;

        public Task ExecuteAsync(HttpContext httpContext)
        {
            using var ms = new MemoryStream();
            xmlSerializer.Serialize(ms, result);
            httpContext.Response.ContentType = "application/xml";
            ms.Position = 0; // Это важно!
            return ms.CopyToAsync(httpContext.Response.Body);
        }
    }

    // Статик класс для расширения.
    static class XmlResultExtensions
    {
        public static IResult Xml<T>(this IResultExtensions _, T result)
        {
            return new XmlResult<T>(result);
        }
    }
}