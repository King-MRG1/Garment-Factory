using Database.Models;
using Shared.Dtos.PhoneDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Mapping
{
    public static class PhoneMapping
    {
        public static PhoneDto ToPhoneDto(this Phone phone)
        {
            return new PhoneDto
            {
                Number = phone.Number,
            };
        }
        public static Phone ToPhone(this PhoneDto createPhoneDto)
        {
            return new Phone
            {
                Number = createPhoneDto.Number,
            };
        }
    }
}
