using DogeFriendsAPI.Dto;
using DogeFriendsAPI.XmlSerialization;
using Extensions;
using Microsoft.AspNetCore.Authorization;

namespace DogeFriendsAPI.Endpoints
{
    public static class DogEndpoints
    {
        public static WebApplication SetDogEndpoints(this WebApplication app)
        {
            app.MapGet("/dog/user/{id}", async (IUserRepository userRepository, IDogRepository dogRepository, int userId) =>
            {
                if (!userRepository.UserExist(userId).Result)
                    return Results.NotFound("User not found.");

                return await dogRepository.GetAllUserDogsAsync(userId) is List<DogDto> dogDto 
                    ? Results.Ok(dogDto) 
                    : Results.NoContent();
            })
            .Produces<List<DogDto>>()
            .Produces(StatusCodes.Status204NoContent)
            .WithName("Get all user dogs")
            .WithTags("Dog endpoints");

            app.MapGet("/dog/{id}", async (IDogRepository dogRepository, int dogId) =>
            {
                return await dogRepository.GetDogAsync(dogId) is Dog dog 
                    ? Results.Ok(dog) 
                    : Results.NoContent();
            })
            .Produces<Dog>()
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

            app.MapDelete("/dog", async (IDogRepository dogRepository, int dogId) =>
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