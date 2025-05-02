using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Services_Abstractions;
using Shared;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Domain.Models.Identity;

namespace Services
{
    public class AuthService(UserManager<AppUser> userManager , IOptions<JwtOptions> options) : IAuthService
    {
        public async Task<UserResultDto> LoginAsync(LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user is null) throw new UnAuthorizedException();

            var flage = await userManager.CheckPasswordAsync(user, loginDto.Password);
            if(!flage) throw new UnAuthorizedException();

            return new UserResultDto()
            {
                Email = user.Email,
                Display = user.DisplayName,
                Token = await GenerateJwtTokenAsync(user),
            };

        }

        public async Task<UserResultDto> RegisterAsync(RegisterDto registerDto)
        {
            var user = new AppUser()
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                PhoneNumber = registerDto.PhoneNumber,

            };
            var result = await userManager.CreateAsync(user,registerDto.Password);

            if(!result.Succeeded)
            {
               var errors =  result.Errors.Select(error => error.Description);
                throw new ValidationException(errors);
            }

            return new UserResultDto()
            {
                Email = user.Email,
                Display = user.DisplayName,
                Token = await GenerateJwtTokenAsync(user)
            };
        }

        public async Task<string> GenerateJwtTokenAsync(AppUser user)
        {
            // Header
            // Paylaod
            // Signature
            var jwtoptions = options.Value;

            var authClaim = new List<Claim>()
            { 
                 new Claim(ClaimTypes.Name , user.UserName)  ,    
                 new Claim(ClaimTypes.Email , user.Email)
            };

            var roles = await userManager.GetRolesAsync(user);
            foreach(var role in roles)
            {
                authClaim.Add(new Claim(ClaimTypes.Role, role));
            }

            var secreteKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtoptions.SecretKey));
            var teken = new JwtSecurityToken
                (issuer: jwtoptions.Issuer,
                    audience: jwtoptions.Audience,
                    claims: authClaim,
                    expires: DateTime.UtcNow.AddDays(jwtoptions.DurationInDays),
                    signingCredentials : new SigningCredentials( secreteKey ,SecurityAlgorithms.HmacSha256Signature)

                );  
            
            return new JwtSecurityTokenHandler().WriteToken( teken );
        }
    }
}
