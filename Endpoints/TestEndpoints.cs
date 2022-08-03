namespace DogeFriendsAPI.Endpoints
{
    public static class TestEndpoints
    {
    public static WebApplication SetTestEndpoints(this WebApplication app)
        {
            app.MapGet("/test/exception", () => 
            {
                throw new Exception("Test exception thrown");
            })
            .Produces(StatusCodes.Status500InternalServerError)
            .WithName("Exception test endpoint")
            .WithTags("Test endpoints");     

            return app;
        }               
    }
}