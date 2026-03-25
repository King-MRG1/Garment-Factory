using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shared.Dtos.AdvanceAndDeductionDtos
{
    public class UpdateAdvanceAndDeductionDto
    {
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero")]
        public decimal Amount { get; set; }

        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        [Range(0, 1, ErrorMessage = "Type must be 0 (Advance) or 1 (Deduction)")]
        public int Type { get; set; }

        [Required]
        public int Worker_Id { get; set; }
    }
}
