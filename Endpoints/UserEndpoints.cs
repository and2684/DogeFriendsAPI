namespace DogeFriendsAPI.Endpoints
{
    public static class UserEndpoints
    {
        public static WebApplication SetUserEndpoints(this WebApplication app)
        {
            app.MapGet("/", () => "Welcome to DogeFriends :)");

            app.MapGet("/user", async (IUserRepository userRepository) =>
                Results.Ok(await userRepository.GetAllUsers()));

            app.MapGet("/user/{id}", async (IUserRepository userRepository, int id) =>
                await userRepository.GetUser(id) is User user ? Results.Ok(user) : Results.NotFound("User not found."));

            app.MapPost("/user", async (IUserRepository userRepository, [FromBody] User user) =>
            {
                await userRepository.InsertUser(user);
                Results.Created($"/user/{user.Id}", user);
            });

            app.MapPut("/user", async (IUserRepository userRepository, [FromBody] User user) =>
                await userRepository.UpdateUser(user) is User updatedUser ? Results.Ok(updatedUser) : Results.NotFound("User cannot be updated."));

            app.MapDelete("/user/{id}", async (IUserRepository userRepository, int id) =>
                await userRepository.DeleteUser(id) ? Results.Ok("User was deleted.") : Results.BadRequest("User cannot be deleted"));

            return app;
        }
    }
}
