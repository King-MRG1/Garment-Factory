using Database.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository.Interfaces;
using Services.Interfaces;
using Shared.Dtos.UserDtos;
using Shared.Mapping;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITokenService _tokenService;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ITokenService tokenService,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _userManager.FindByNameAsync(loginDto.UserName);

            if (string.IsNullOrWhiteSpace(loginDto.UserName))
                return Fail("Username is required.");

            if (string.IsNullOrWhiteSpace(loginDto.Password))
                return Fail("Password is required.");

            if (user == null)
                return Fail("User not found.");

            var result = await _signInManager.CheckPasswordSignInAsync(
                user, loginDto.Password, false);

            if (!result.Succeeded)
                return Fail("Invalid username or password.");

            await _unitOfWork.Auth.RevokeAllUserTokensAsync(user.Id);

            return await BuildAuthResponeAsync(user, "Login successful.");
        }

        public async Task<AuthResponseDto> Logout(string refreshToken)
        {
            var storedToken = await _unitOfWork.Auth.GetRefreshTokenAsync(refreshToken);
            if (storedToken == null)
                return Fail("Refresh token not found.");

            await _unitOfWork.Auth.RevokeAllUserTokensAsync(storedToken.UserId);

            await _unitOfWork.Auth.DeleteExpiredTokensAsync();

            return Success("Logout successful.");
        }

        public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
        {
            var isVaild = await _unitOfWork.Auth.IsTokenValidAsync(refreshToken);
            if (!isVaild)
                return Fail("Invalid refresh token.");

            var storedToken = await _unitOfWork.Auth.GetRefreshTokenAsync(refreshToken);
            if (storedToken == null)
                return Fail("Refresh token not found.");

            var user = await _userManager.FindByIdAsync(storedToken.UserId);
            if (user == null)
                return Fail("User not found.");

            await _unitOfWork.Auth.RevokeRefreshTokenAsync(storedToken.Id);

            await _unitOfWork.Auth.DeleteExpiredTokensAsync();

            return await BuildAuthResponeAsync(user, "Token refreshed successfully.");
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            var userExists = await _userManager.FindByNameAsync(registerDto.UserName);

            if (userExists != null)
                return Fail("Username already exists.");

            var user = registerDto.ToApplicationUser();

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
                return Fail("User creation failed: " + string.Join(", ", result.Errors.Select(e => e.Description)));

            return await BuildAuthResponeAsync(user, "Registration successful.");
        }

        private async Task<AuthResponseDto> BuildAuthResponeAsync
            (ApplicationUser user, string massage)
        {
            var refreshToken = _tokenService.CreateRefreshTokenStore(user.Id);
            var token = _tokenService.GenerateJwtToken(user);

            await _unitOfWork.Auth.AddAsync(refreshToken);
            await _unitOfWork.SaveChangesAsync();

            return new AuthResponseDto
            {
                IsSuccess = true,
                Message = massage,
                Token = token,
                RefreshToken = refreshToken.Token,
                User = user.ToUserDataDto()
            };
        }

        private AuthResponseDto Fail(string message) =>
            new AuthResponseDto { IsSuccess = false, Message = message };
        private AuthResponseDto Success(string message) =>
            new AuthResponseDto { IsSuccess = true, Message = message };
    }
}
