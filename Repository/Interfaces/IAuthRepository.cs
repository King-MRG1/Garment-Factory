using Database.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Interfaces
{
    public interface IAuthRepository : IGenericRepository<RefreshTokenStore>
    {
        public Task<RefreshTokenStore?> GetRefreshTokenAsync(string token);
        public Task<RefreshTokenStore?>GetActiveRefreshTokenByUserIdAsync(string userId);
        public Task<IEnumerable<RefreshTokenStore>> GetAllRefreshTokensStoresAsync();
        public Task RevokeRefreshTokenAsync(int tokenId);
        public Task RevokeAllUserTokensAsync(string userId);
        public Task<bool> IsTokenValidAsync(string token);
        Task DeleteExpiredTokensAsync();
    }
}
