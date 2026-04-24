using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shared.Dtos.PhoneDtos
{
    public class PhoneDto
    {
        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^[0-9+]{11}$", ErrorMessage = "Phone number must be 11 digits")]
        public string Number { get; set; }
    }
}
