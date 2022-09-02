using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogeFriendsAPI.Interfaces
{
    public interface ITranslateService
    {
        Task<string> Translate(string input);
    }
}