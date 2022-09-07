using System.Text.Json;
using DogeFriendsAPI.Dto;
using Microsoft.AspNetCore.Authorization;

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

            app.MapGet("/test/seedbreed", [Authorize(Roles = "Administrator")] async (DataContext context, IMapper mapper, ITranslateService translateService) =>
            {
                if (await context.Breeds!.AnyAsync()) return Results.BadRequest("We already have breeds, you dont need to seed them.");

                var breedData = await System.IO.File.ReadAllTextAsync("Seed\\Breeds.json");
                var breeds = JsonSerializer.Deserialize<List<BreedSeedDto>>(breedData);

                foreach (var breed in breeds!)
                {
                    using (var httpClient = new HttpClient())
                    {
                        var breedDb = new Breed
                        {
                            BreedName = breed.Name.Trim(),
                            BreedNameRu = translateService.Translate(breed.Name.Trim()).Result,
                            BreedPhoto = await httpClient.GetByteArrayAsync(breed.Url),
                            BreedPhotoFileName = breed.Url.Split('/').LastOrDefault()!
                        };

                        context.Breeds!.Add(breedDb);
                    }
                }

                await context.SaveChangesAsync();

                return Results.Ok($"Breeds successfull seeded. {breeds.Count()} positions added.");
            })
            .WithName("Seed breeds")
            .WithTags("Test endpoints");

            app.MapGet("/test/breed", [Authorize(Roles = "Administrator")] async (DataContext context) =>
            {
                return Results.Ok(await context.Breeds.ToListAsync());
            })            
            .WithName("Get all breeds")
            .WithTags("Test endpoints");

            app.MapGet("/test/translate", [Authorize(Roles = "Administrator")] async (ITranslateService translateService, [FromQuery] string input) =>
            {
                return Results.Ok(await translateService.Translate(input));
            })
            .WithName("Translate text")
            .WithTags("Test endpoints");

            return app;
        }
    }
}