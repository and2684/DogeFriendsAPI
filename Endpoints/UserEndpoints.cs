namespace DogeFriendsAPI.Endpoints
{
    public static class UserEndpoints
    {
        public static WebApplication SetUserEndpoints(this WebApplication app)
        {
            app.MapGet("/", () => "Welcome to DogeFriends :)").RequireAuthorization();

            app.MapGet("/user", async (DataContext context) =>
                await context.Users.ToListAsync());

            app.MapGet("/user/{id}", async (DataContext context, int id) =>
                await context.Users.FirstOrDefaultAsync(x => x.Id == id) is User user ? Results.Ok(user) : Results.NotFound("User not found."));

            app.MapPost("/user", async (DataContext context, User user) =>
            {
                context.Users.Add(user);
                await context.SaveChangesAsync();
                return Results.Ok(user);
            });

            app.MapPut("/user", async (DataContext context, User user) =>
            {
                var dbUser = await context.Users.FindAsync(user.Id);

                if (dbUser == null) return Results.NotFound("User not found.");

                dbUser.FirstName = user.FirstName;
                dbUser.Password = user.Password;
                dbUser.LastName = user.LastName;
                dbUser.IsCompany = user.IsCompany;
                dbUser.ProfilePhoto = user.ProfilePhoto;

                context.Users.Update(dbUser);
                await context.SaveChangesAsync();
                return Results.Ok(dbUser);
            });

            app.MapDelete("/user/{id}", async (DataContext context, int id) =>
            {
                var dbUser = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
                if (dbUser == null) return Results.NotFound("User not found.");

                context.Users.Remove(dbUser);
                await context.SaveChangesAsync();
                return Results.Ok("User was deleted successfuly.");
            });

            return app;
        }
    }
}
