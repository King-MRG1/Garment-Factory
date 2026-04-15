using Microsoft.AspNetCore.Http;
using Shared.Interfaces;
using System.Security.Claims;

namespace Services.Implementations
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string? GetCurrentUserId()
        {
            return _httpContextAccessor.HttpContext?.User?
                .FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public string GetCurrentUserName()
        {
            return _httpContextAccessor.HttpContext?.User?
                .FindFirstValue(ClaimTypes.Name) ?? "Unknown User";
        }
    }
}
