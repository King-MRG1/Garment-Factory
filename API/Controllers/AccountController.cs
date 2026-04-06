using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Shared.Dtos.UserDtos;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfServices _unitOfServices;

        public AccountController(IUnitOfServices unitOfServices)
        {
            _unitOfServices = unitOfServices;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _unitOfServices.Auth.RegisterAsync(model);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _unitOfServices.Auth.LoginAsync(model);

            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromHeader(Name = "X-Refresh-Token")] string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return BadRequest("Refresh token is required.");
            }
            var result = await _unitOfServices.Auth.RefreshTokenAsync(refreshToken);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [Authorize]
        [HttpGet("Logout")]
        public async Task<IActionResult> Logout([FromHeader(Name = "X-Refresh-Token")] string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                return BadRequest("Refresh token is required.");
            }
            var result = await _unitOfServices.Auth.Logout(refreshToken);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}