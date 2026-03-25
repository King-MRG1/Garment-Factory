using Shared.Dtos.PhoneDtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shared.Dtos.TraderDtos
{
    public class CreateTraderDto
    {
        [Required(ErrorMessage = "Trader name is required")]
        [MaxLength(100, ErrorMessage = "Trader name cannot exceed 100 characters")]
        public string Trader_Name { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [MaxLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        public string Address { get; set; }

        [Range(0, 1, ErrorMessage = "Trader type must be 0 (Fabric) or 1 (Order)")]
        public int Trader_Type { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Amount cannot be negative")]
        public decimal Amount { get; set; }

        public List<PhoneDto> Phones { get; set; } = new List<PhoneDto>();
    }
}
