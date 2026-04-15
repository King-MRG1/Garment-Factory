namespace Shared.Interfaces
{
    public interface ICurrentUserService
    {
        string? GetCurrentUserId();
        string GetCurrentUserName();
    }
}
