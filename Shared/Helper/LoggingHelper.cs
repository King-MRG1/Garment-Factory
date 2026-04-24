using Shared.Interfaces;

namespace Shared.Helper
{
    public static class LoggingHelper
    {
        public static string GetUserContext(ICurrentUserService currentUserService)
        {
            var userId = currentUserService.GetCurrentUserId();
            var userName = currentUserService.GetCurrentUserName();

            if (string.IsNullOrWhiteSpace(userId))
                return "[Anonymous User]";

            return $"[User: {userName} (ID: {userId})]";
        }
    }
}