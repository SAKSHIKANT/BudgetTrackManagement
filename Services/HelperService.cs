using System.Security.Cryptography;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
namespace InternalBudgetTracker.Services
{
    

    public class HelperService
    {
        private readonly IConfiguration _configuration;

        public HelperService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // =====================================
        // 1️⃣ Generate Hash Password

        // =====================================
        public string GenerateHashPassword(string password)
        {
            string secretKey = _configuration["Security:SecretKey"];

            using var hmac = new HMACSHA256(
                Encoding.UTF8.GetBytes(secretKey)
            );

            byte[] hashBytes =
                hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

            return Convert.ToHexString(hashBytes).ToLower();
        }

        // =====================================
        // 2️⃣ Generate JWT Token
        // =====================================
        //public string GenerateToken(string email, string role)
        //{
        //    string secretKey = _configuration["Security:SecretKey"];

        //    var claims = new[]
        //    {
        //    new Claim("email", email),
        //    new Claim("role", role)
        //};

        //    var key = new SymmetricSecurityKey(
        //        Encoding.UTF8.GetBytes(secretKey)
        //    );

        //    var creds = new SigningCredentials(
        //        key,
        //        SecurityAlgorithms.HmacSha256
        //    );

        //    var token = new JwtSecurityToken(
        //        claims: claims,
        //        expires: DateTime.UtcNow.AddMinutes(600),
        //        signingCredentials: creds
        //    );

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}


       
 
public string GenerateToken(string email, string role)
    {
        string secretKey = _configuration["Security:SecretKey"];

        var claims = new[]
        {
        new Claim(ClaimTypes.Email, email),
        new Claim(ClaimTypes.Role, role)
    };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secretKey)
        );

        var creds = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(600),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

        // =====================================
        // 3️⃣ Check / Verify JWT Token
        // =====================================
        //public object CheckValidToken(string token)
        //    {
        //        try
        //        {
        //            string secretKey = _configuration["Security:SecretKey"];
        //            var key = Encoding.UTF8.GetBytes(secretKey);

        //            var handler = new JwtSecurityTokenHandler();

        //            var principal = handler.ValidateToken(
        //                token,
        //                new TokenValidationParameters
        //                {
        //                    ValidateIssuer = false,
        //                    ValidateAudience = false,
        //                    ValidateLifetime = true,
        //                    ValidateIssuerSigningKey = true,
        //                    IssuerSigningKey = new SymmetricSecurityKey(key)
        //                },
        //                out SecurityToken validatedToken
        //            );

        //            return new
        //            {
        //                valid = true,
        //                data = new
        //                {
        //                    email = principal.FindFirst("email")?.Value,
        //                    role = principal.FindFirst("role")?.Value
        //                }
        //            };
        //        }
        //        catch (SecurityTokenExpiredException)
        //        {
        //            return new { valid = false, error = "TOKEN_EXPIRED" };
        //        }
        //        catch
        //        {
        //            return new { valid = false, error = "Invalid token" };
        //        }
        //    }
        public object CheckValidToken(string token)
        {
            try
            {
                string secretKey = _configuration["Security:SecretKey"];
                var key = Encoding.UTF8.GetBytes(secretKey);

                var handler = new JwtSecurityTokenHandler();

                var principal = handler.ValidateToken(
                    token,
                    new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    },
                    out SecurityToken validatedToken
                );

                return new
                {
                    valid = true,
                    data = new
                    {
                        email = principal.FindFirst(ClaimTypes.Email)?.Value,
                        role = principal.FindFirst(ClaimTypes.Role)?.Value
                    }
                };
            }
            catch
            {
                return new { valid = false, error = "Invalid token" };
            }
        }


    }

}

