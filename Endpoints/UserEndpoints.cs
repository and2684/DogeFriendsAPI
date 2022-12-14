using DogeFriendsAPI.Dto;
using DogeFriendsAPI.XmlSerialization;
using Extensions;
using Microsoft.AspNetCore.Authorization;

namespace DogeFriendsAPI.Endpoints
{
    public static class UserEndpoints
    {
        public static WebApplication SetUserEndpoints(this WebApplication app)
        {
            // Это тестовый эндпоинт, поэтому он с логикой, это норма
            app.MapGet("/", [Authorize] (HttpContext ctx, DataContext context) =>
            {
                var role = ctx.User.GetLoggedUserRoles(context).Result?.OrderBy(x => x.Id).FirstOrDefault()?.Role;
                return $"Welcome to DogeFriends, my dearest {role} {ctx.User.GetLoggedUsername()} :)";
            })
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status401Unauthorized)
                .WithName("Welcome")
                .WithTags("Welcome group");

            app.MapGet("/user", async (IUserRepository userRepository) =>
                Results.Ok(await userRepository.GetAllUsersAsync()))
                .Produces<List<UserShowDto>>()
                .WithName("Get all users")
                .WithTags("Get commands");

            app.MapGet("/user/xml", async (IUserRepository userRepository) =>
                Results.Extensions.ConvertToXml(await userRepository.GetAllUsersAsync()))
                .Produces<List<UserShowDto>>()
                .WithName("Get all users as xml")
                .WithTags("Get commands");

            app.MapGet("/user/{id}", async (IUserRepository userRepository, int id) =>
                await userRepository.GetUserAsync(id) is UserShowDto userShowDto ? Results.Ok(userShowDto) : Results.NotFound("User not found."))
                .Produces<UserShowDto>()
                .Produces(StatusCodes.Status404NotFound)
                .WithName("Get user by id")
                .WithTags("Get commands");

            app.MapGet("/user/search/username/{username}", async (IUserRepository userRepository, string username) =>
                await userRepository.GetUsersAsync(username) is List<UserShowDto> userList ? Results.Ok(userList) : Results.NotFound("User not found."))
                .Produces<List<UserShowDto>>()
                .Produces(StatusCodes.Status404NotFound)
                .WithName("Get user by username")
                .WithTags("Get commands");
            //                .ExcludeFromDescription(); // This method makes endpoint invisible for swagger but still usable!

            app.MapGet("/person/search/{fullname}", async (IUserRepository userRepository, string fullname) =>
                await userRepository.GetPersonsAsync(fullname) is List<PersonDto> userList ? Results.Ok(userList) : Results.NotFound("Person not found."))
                .Produces<List<PersonDto>>()
                .Produces(StatusCodes.Status404NotFound)
                .WithName("Get person")
                .WithTags("Get commands");

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
                .Produces<User>()
                .Produces(StatusCodes.Status404NotFound)
                .WithName("Update user")
                .WithTags("Update commands");

            app.MapDelete("/user/{id}", [Authorize(Roles = "Administrator")] async (IUserRepository userRepository, int id) =>
                await userRepository.DeleteUserAsync(id) ? Results.Ok("User was deleted.") : Results.NotFound("Nothing to delete"))
                .Produces<Boolean>()
                .Produces(StatusCodes.Status404NotFound)
                .WithName("Delete user")
                .WithTags("Delete commands");

            app.MapPost("user/register", async (IUserRepository userRepository, [FromBody] RegisterDto registerDto) =>
                await userRepository.UserExist(registerDto.Username) ? Results.BadRequest("Username taken") :
                (await userRepository.RegisterUser(registerDto) is UserDto registeredUser ? Results.Ok(registeredUser) : Results.BadRequest("Registration failed")))
                .Produces<UserDto>()
                .Produces(StatusCodes.Status400BadRequest)
                .WithName("Register user")
                .WithTags("Accounting commands");

            // Тут какая-то хрень получилась. Нужно разобраться, как из репо возвращать правильно ошибки. 
            // Временное решение - вытащить каждую проверку в отдельный метод. Но это раздувает наш эндпоинт. Думаю...
            app.MapPost("user/login", async (IUserRepository userRepository, [FromBody] LoginDto loginDto) =>
                {
                    if (!(await userRepository.UserExist(loginDto.Username)))
                        return Results.NotFound("User not found");

                    if (!(await userRepository.PasswordCorrect(loginDto)))
                        return Results.BadRequest("Incorrect password");

                    if (await userRepository.LoginUser(loginDto) is UserDto loggedUser)
                        return Results.Ok(loggedUser);

                    return Results.BadRequest("Login failed");
                })
                .Produces<UserDto>()
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .WithName("Login as user")
                .WithTags("Accounting commands");

            app.MapPut("user/setuserroles", [Authorize(Roles = "Administrator")] async 
                (IUserRepository userRepository, [FromBody] UserRoleSetDto userRoleSetDto) =>
                {   
                    if (userRepository.GetPersonsAsync(userRoleSetDto.Username).Result?.FirstOrDefault() == null)
                        return Results.NotFound("User not found");

                    if (await userRepository.SetUserRoles(userRoleSetDto))
                        return Results.Ok("Roles have been set");

                    return Results.BadRequest("No roles set");     
                })
                .Produces(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status404NotFound)
                .Produces(StatusCodes.Status400BadRequest)
                .WithName("Set user roles")
                .WithTags("Update commands");

            return app;
        }
    }
}