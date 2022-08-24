using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API.Services
{
    public class TokenService : ITokenService
    {
        private readonly SymmetricSecurityKey _key;
        private readonly string _issuer;
        private readonly string _audience;

        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
            _issuer = config["Jwt:Issuer"];
            _audience = config["Jwt:Audience"];
        }

        public string CreateToken(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Name, user.Username!), // Claims - что хранит токен (здесь субъект токена - это имя пользователя)
                new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                new Claim(ClaimTypes.Role, "Administrator") // Добавить role к юзеру позже
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature); // Ключ для токена            

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7), // Время жизни токена (неделя по умолчанию, потом вынести в appsettings)
                SigningCredentials = creds,
                Audience = _audience,
                Issuer = _issuer
            };

            var tokenHandler = new JwtSecurityTokenHandler();      // tokenHandler нужен для создания токена и его преобразования в string
            var token = tokenHandler.CreateToken(tokenDescriptor); // Создали токен
            return tokenHandler.WriteToken(token);                 // Вернули токен в виде string
        }
    }
}