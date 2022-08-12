using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;

        public TokenService(IConfiguration config)        
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
        }

        public string CreateToken(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Username!) // Claims - что хранит токен (здесь субъект токена - это имя пользователя)
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature); // Ключ для токена

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7), // Время жизни токена
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();      // tokenHandler нужен для создания токена и его преобразования в string
            var token = tokenHandler.CreateToken(tokenDescriptor); // Создали токен
            return tokenHandler.WriteToken(token);                 // Вернули токен в виде string
        }
    }
}