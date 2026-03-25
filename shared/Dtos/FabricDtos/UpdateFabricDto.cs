using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shared.Dtos.FabricDtos
{
    public class UpdateFabricDto
    {
        [Required(ErrorMessage = "Fabric name is required")]
        [MaxLength(100, ErrorMessage = "Fabric name cannot exceed 100 characters")]
        public string Fabric_Name { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Metres must be greater than zero")]
        public decimal Metres { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public decimal Price { get; set; }

        [Required]
        public int Trader_Id { get; set; }
    }
}
