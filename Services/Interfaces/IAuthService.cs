using Shared.Dtos.UserDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface IAuthService
    {
        public Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        public Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        public Task<AuthResponseDto> Logout(string refreshToken);
        public Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);
    }
}
