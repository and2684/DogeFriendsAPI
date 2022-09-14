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
                    : Results.NotFound("User have no dogs.");
            })
            .Produces<List<DogDto>>()
            .Produces(StatusCodes.Status404NotFound)
            .WithName("Get all user dogs")
            .WithTags("Dog endpoints");

            app.MapGet("/dog/{id}", async (IDogRepository dogRepository, int dogId) =>
            {
                return await dogRepository.GetDogAsync(dogId) is Dog dog 
                    ? Results.Ok(dog) 
                    : Results.NotFound("Dog not found.");
            })
            .Produces<Dog>()
            .Produces(StatusCodes.Status404NotFound)
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

            return app;
        }
    }
}