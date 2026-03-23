using Database.Data;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Implementations
{
    public class AuthRepository : GenericRepository<RefreshTokenStore>, IAuthRepository
    {
        public AuthRepository(AppDbContext context) : base(context) { }

        public async Task DeleteExpiredTokensAsync()
        {
            var now = DateTime.UtcNow;

            var expiredTokens = await _context.RefreshTokens
                .Where(rt => rt.ExpiryDate <= now)
                .ExecuteDeleteAsync();
        }

        public async Task<RefreshTokenStore?> GetActiveRefreshTokenByUserIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return null;

            var now = DateTime.UtcNow;

            var activeToken = await _context.RefreshTokens
                .Where(tr => tr.UserId == userId &&
                             !tr.IsRevoked &&
                             tr.ExpiryDate > now)
                .OrderByDescending(rt => rt.CreatedAt)
                .FirstOrDefaultAsync();

            return activeToken;
        }

        public async Task<IEnumerable<RefreshTokenStore>> GetAllRefreshTokensStoresAsync()
        {
            var allTokens = await _context.RefreshTokens
                .OrderByDescending(rt => rt.CreatedAt)
                .ToListAsync();

            return allTokens;
        }

        public async Task<RefreshTokenStore?> GetRefreshTokenAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
                return null;

            var trimmedToken = token.Trim();

            var refreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == trimmedToken);

            return refreshToken;
        }

        public async Task<bool> IsTokenValidAsync(string token)
        {
            if (string.IsNullOrEmpty(token))
                return false;

            var trimmedToken = token.Trim();
            var now = DateTime.UtcNow;

            var isValid = await _context.RefreshTokens
                .AnyAsync(rt => rt.Token == trimmedToken &&
                                !rt.IsRevoked &&
                                rt.ExpiryDate > now);

            return isValid;
        }

        public async Task RevokeAllUserTokensAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return;

            var activeTokens = await _context.RefreshTokens
                .Where(rt => rt.UserId == userId && !rt.IsRevoked)
                .ExecuteUpdateAsync(s => s.SetProperty(rt => rt.IsRevoked, true));
        }

        public async Task RevokeRefreshTokenAsync(int tokenId)
        {
            var token = await _context.RefreshTokens.FindAsync(tokenId);

            if (token != null && !token.IsRevoked)
            {
                token.IsRevoked = true;
                _context.RefreshTokens.Update(token);
            }
        }
    }
}
