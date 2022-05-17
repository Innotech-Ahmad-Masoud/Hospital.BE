using Hospital.Application.Interfaces;
using Hospital.Application.Models.Authenticate;
using Hospital.Application.Results;
using Hospital.Application.Results.Authenticate;
using Hospital.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Hospital.Application.Services
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticateService(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        public async Task<Response<LoginResult>> Login(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new(ClaimTypes.Name, user.UserName),
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = GetToken(authClaims);

                return new Response<LoginResult>()
                {
                    Status = nameof(HttpStatusCode.OK),
                    Message = "User login successfully",
                    Data = new LoginResult()
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(token),
                        Expiration = token.ValidTo
                    }
                };
            }

            return new Response<LoginResult>()
            {
                Status = nameof(HttpStatusCode.BadRequest)
            };
        }


        public async Task<Response<RegisterResult>> Register(RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return new Response<RegisterResult>()
                {
                    Status = nameof(HttpStatusCode.BadRequest),
                    Message = "User already exists!"
                };

            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return new Response<RegisterResult>()
                {
                    Status = nameof(HttpStatusCode.BadRequest),
                    Message = result.Errors?.FirstOrDefault()?.Description
                };

            return new Response<RegisterResult>()
            {
                Status = nameof(HttpStatusCode.OK),
                Message = "User created successfully!",
                Data = new RegisterResult()
                {
                    Username = model.Username,
                    Email = model.Email
                }
            };
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }
    }
}


