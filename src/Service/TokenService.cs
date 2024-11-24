using IDWM_TallerAPI.Src.Interfaces.Service;

namespace IDWM_TallerAPI.Src.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config)
        {
            _config = config;
            var signingKey = _config["JWT:SigningKey"];
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey));
        }
        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email ?? throw new ArgumentNullException(nameof(user.Email), "User email cannot be null")),
                new Claim(JwtRegisteredClaimNames.GivenName, user.UserName ?? throw new ArgumentNullException(nameof(user.UserName), "User name cannot be null"))
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds,
                Issuer = _config["JWT:Issuer"],
                Audience = _config["JWT:Audience"]
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}