using Database.Models;
using Shared.Dtos.UserDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Mapping
{
    public static class UserMapping
    {
        public static UserDataDto ToUserDataDto(this ApplicationUser user) =>
            new UserDataDto
            {
                Id = user.Id,
                FullName = user.Full_name,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber!
            };

        public static ApplicationUser ToApplicationUser(this RegisterDto dto) =>
            new ApplicationUser
            {
                UserName = dto.UserName,
                Full_name = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                Email = $"{dto.UserName}@garmentfactory.com"
            };
    }
}
