using Domain.Exceptions;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Services_Abstractions;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AuthService(UserManager<AppUser> userManager) : IAuthService
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
                Token = "token"
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
                Token = "token"
            };
        }
    }
}
