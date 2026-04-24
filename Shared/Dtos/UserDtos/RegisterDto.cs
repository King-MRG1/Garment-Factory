using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shared.Dtos.UserDtos
{
    public class RegisterDto
    {
        [Required]
        [MinLength(5),MaxLength(50)]
        public string FullName { get; set; }

        [Required]
        [StringLength(20)]
        public string UserName { get; set; }

        [Required,RegularExpression(@"^[0-9+]{11}$", ErrorMessage = "Phone number must be 11 digits")]
        public string PhoneNumber { get; set; }

        [Required,DataType(DataType.Password)]
        public string Password { get; set; }

        [Required,DataType(DataType.Password),Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }
    }
}
