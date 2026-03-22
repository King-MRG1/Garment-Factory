using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Database.Models
{
    public class RefreshTokenStore
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime CreatedAt { get; set; }
        public ApplicationUser User { get; set; }
    }
}
