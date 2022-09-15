using DogeFriendsAPI.Dto;

namespace DogeFriendsAPI.Endpoints
{
    public static class DogEndpoints
    {
        public static WebApplication SetDogEndpoints(this WebApplication app)
        {
            app.MapGet("user/{userId}/dogs", async (IUserRepository userRepository, IDogRepository dogRepository, int userId) =>
            {
                if (!userRepository.UserExist(userId).Result)
                    return Results.NotFound("User not found.");

                return await dogRepository.GetAllUserDogsAsync(userId) is List<DogDto> dogDto && dogDto.Count() > 0
                    ? Results.Ok(dogDto) 
                    : Results.NoContent();
            })
            .Produces<List<DogDto>>()
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithName("Get all user dogs")
            .WithTags("Dog endpoints");

            app.MapGet("/dog/{dogId}", async (IDogRepository dogRepository, int dogId) =>
            {
                return await dogRepository.GetDogAsync(dogId) is DogDto dogDto 
                    ? Results.Ok(dogDto) 
                    : Results.NoContent();
            })
            .Produces<DogDto>()
            .Produces(StatusCodes.Status204NoContent)
            .WithName("Get dog by id")
            .WithTags("Dog endpoints");     

            app.MapPost("/dog", async (IDogRepository dogRepository, [FromBody] DogDto dog) =>
            {
                return await dogRepository.InsertDogAsync(dog) is DogDto dogDto 
                    ? Results.Created($"/dog/{dogDto.Id}", dogDto) 
                    : Results.BadRequest("Dog cannot be added.");
            })
            .Accepts<DogDto>("application/json")
            .Produces<DogDto>(StatusCodes.Status201Created)            
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("Add dog")
            .WithTags("Dog endpoints");        

            app.MapPut("/dog", async (IDogRepository dogRepository, [FromBody] DogDto dog) =>
            {
                return await dogRepository.UpdateDogAsync(dog) is DogDto dogDto 
                    ? Results.Ok(dogDto) 
                    : Results.BadRequest("Dog cannot be updated.");
            })
            .Accepts<DogDto>("application/json")
            .Produces<DogDto>(StatusCodes.Status200OK)            
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("Update dog")
            .WithTags("Dog endpoints");                        

            app.MapDelete("/dog/{dogId}", async (IDogRepository dogRepository, int dogId) =>
            {
                return await dogRepository.DeleteDogAsync(dogId) 
                    ? Results.Ok("Dog deleted sucessfully.")
                    : Results.BadRequest("Dog cannot be deleted.");
            })
            .Produces(StatusCodes.Status200OK)            
            .Produces(StatusCodes.Status400BadRequest)
            .WithName("Delete dog")
            .WithTags("Dog endpoints");              

            return app;
        }
    }
}