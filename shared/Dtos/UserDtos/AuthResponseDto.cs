using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Dtos.UserDtos
{
    public class AuthResponseDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public UserDataDto? User { get; set; }
    }
}
