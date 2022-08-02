namespace DogeFriendsAPI.Endpoints
{
    public static class UserEndpoints
    {
        public static WebApplication SetUserEndpoints(this WebApplication app)
        {
            app.MapGet("/", () => "Welcome to DogeFriends :)")
                .Produces(StatusCodes.Status200OK)
                .WithName("Welcome")
                .WithTags("Welcome group");

            app.MapGet("/user", async (IUserRepository userRepository) =>
                Results.Ok(await userRepository.GetAllUsersAsync()))
                .Produces<User>(StatusCodes.Status200OK)
                .WithName("Get all users")
                .WithTags("Get commands");

            app.MapGet("/user/{id}", async (IUserRepository userRepository, int id) =>
                await userRepository.GetUserAsync(id) is User user ? Results.Ok(user) : Results.NotFound("User not found."))
                .Produces<List<User>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithName("Get user by id")
                .WithTags("Get commands");

            app.MapGet("/user/search/{username}", async (IUserRepository userRepository, string username) =>
                await userRepository.GetUsersAsync(username) is List<User> userList ? Results.Ok(userList) : Results.NotFound("User not found."))
                .Produces<List<User>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithName("Get user by username")
                .WithTags("Get commands")
                .ExcludeFromDescription();                

            app.MapPost("/user", async (IUserRepository userRepository, [FromBody] User user) =>
                {
                    await userRepository.InsertUserAsync(user);
                    Results.Created($"/user/{user.Id}", user);
                })
                .Accepts<User>("application/json")
                .Produces<User>(StatusCodes.Status201Created)
                .WithName("Create user")
                .WithTags("Create commands");

            app.MapPut("/user", async (IUserRepository userRepository, [FromBody] User user) =>
                await userRepository.UpdateUserAsync(user) is User updatedUser ? Results.Ok(updatedUser) : Results.NotFound("User cannot be updated."))
                .Accepts<User>("application/json")
                .Produces<User>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithName("Update user")
                .WithTags("Update commands");

            app.MapDelete("/user/{id}", async (IUserRepository userRepository, int id) =>
                await userRepository.DeleteUserAsync(id) ? Results.Ok("User was deleted.") : Results.NotFound("Nothing to delete"))
                .Produces<Boolean>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .WithName("Delete user")
                .WithTags("Delete commands");

            return app;
        }
    }
}
