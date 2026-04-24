using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shared.Dtos.RevenueDtos
{
    public class UpdateRevenueDto
    {
        [Required(ErrorMessage = "Revenue name is required")]
        [MaxLength(100, ErrorMessage = "Revenue name cannot exceed 100 characters")]
        public string Revenue_Name { get; set; }

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Revenue_Description { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public decimal Amount { get; set; }
        public int? Trader_Id { get; set; }
    }
}
