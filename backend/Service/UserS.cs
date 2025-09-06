using backend.Data;
using backend.Model;
using backend.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace backend.Service
{
    public class UserS : IUser
    {
        private readonly DataContext _context;
        private readonly IConfiguration _config;
        private readonly IPasswordHasher<User> _passwordHasher;

        public UserS(DataContext context, IPasswordHasher<User> passwordHasher, IConfiguration config)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _config = config;
        }

        public async Task<User> CreateUser(User user)
        {
            try
            {

                  var nowColombia = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "SA Pacific Standard Time");
                var exists = await _context.Users.AnyAsync(u => u.Email == user.Email);
                if (exists)
                    throw new InvalidOperationException("Ya existe un usuario con ese email.");

                user.Password = _passwordHasher.HashPassword(user, user.Password);
                user.CreatedAtUtc = nowColombia;
                user.UpdatedAtUtc = nowColombia;

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return user;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CreateUser] Error: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdatePassword(string email, string newPassword)
        {
            try
            {
                var nowColombia = TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.UtcNow, "SA Pacific Standard Time");
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null) return false;

                user.Password = _passwordHasher.HashPassword(user, newPassword);

                user.UpdatedAtUtc = nowColombia;

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UpdatePassword] Error: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetAllByRol()
        {
            try
            {
                return await _context.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GetAllByRol] Error: {ex.Message}");
                throw;
            }
        }

        public async Task<User?> Authenticate(string email, string password)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null) return null;

                var res = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
                if (res == PasswordVerificationResult.Success)
                    return user;

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Authenticate] Error: {ex.Message}");
                throw;
            }
        }

        public string GenerateToken(int id, string username, string role)
        {
            try
            {
                var secretkey = this._config.GetSection("settings").GetSection("secretkey").ToString();

                if (string.IsNullOrEmpty(secretkey))
                {


                    throw new InvalidOperationException("secretkey not config or not found.");
                }


                if (id == 0 || string.IsNullOrEmpty(username))
                {

                    throw new ArgumentException("username is empty.");
                }


                var tokenHandler = new JwtSecurityTokenHandler();

                var keyBytes = Encoding.ASCII.GetBytes(secretkey);


                var claims = new ClaimsIdentity();

                claims.AddClaim(new Claim(ClaimTypes.Name, id.ToString()));

                claims.AddClaim(new Claim(ClaimTypes.Name, username));
                 claims.AddClaim(new Claim(ClaimTypes.Role, role));


                var tokenDescriptor = new SecurityTokenDescriptor
                {

                    Subject = claims,

                    Issuer = "backend",

                    Expires = DateTime.UtcNow.AddMinutes(20),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
                };


                var token = tokenHandler.CreateToken(tokenDescriptor);
                // return the token
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)

            {

                Console.WriteLine($"[Generate Token] Error: {ex.Message}");
                throw;
            }
        }

        public async Task<User?> ValidateUser(string email, string password)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
                if (user == null)
                    return null;

                var result = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
                return result == PasswordVerificationResult.Success ? user : null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ValidateUser] Error: {ex.Message}");
                throw;
            }
        }

    }
}
