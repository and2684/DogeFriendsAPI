namespace DogeFriendsAPI.Interfaces
{
    public interface ITranslateService
    {
        Task<string> Translate(string input);
    }
}