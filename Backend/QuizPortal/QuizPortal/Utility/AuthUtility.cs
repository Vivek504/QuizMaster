using System;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace QuizPortal.Utility
{
	public class AuthUtility
	{
        public static string GenerateToken(IEnumerable<Claim> claims, string jwtSecret, string jwtTokenExpiryTimeInHour, string jwtValidIssuer, string jwtValidAudience)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
            var _TokenExpiryTimeInHour = Convert.ToInt64(jwtTokenExpiryTimeInHour);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = jwtValidIssuer,
                Audience = jwtValidAudience,
                Expires = DateTime.UtcNow.AddHours(_TokenExpiryTimeInHour),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static string GenerateRandomPassword()
        {
            const string uppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowercaseChars = "abcdefghijklmnopqrstuvwxyz";
            const string numericChars = "0123456789";
            const string specialChars = "!@#$%^&*()-_=+";
            int length = 8;

            var requiredChars = new List<string>
            {
                uppercaseChars,
                lowercaseChars,
                numericChars,
                specialChars
            };

            var passwordChars = new char[length];
            var random = new Random();

            for (int i = 0; i < requiredChars.Count; i++)
            {
                var charSet = requiredChars[i];
                passwordChars[i] = charSet[random.Next(charSet.Length)];
            }

            for (int i = requiredChars.Count; i < length; i++)
            {
                var charSet = requiredChars[random.Next(requiredChars.Count)];
                passwordChars[i] = charSet[random.Next(charSet.Length)];
            }

            for (int i = 0; i < length; i++)
            {
                int randomIndex = random.Next(length);
                char temp = passwordChars[i];
                passwordChars[i] = passwordChars[randomIndex];
                passwordChars[randomIndex] = temp;
            }

            return new string(passwordChars);
        }
    }
}

