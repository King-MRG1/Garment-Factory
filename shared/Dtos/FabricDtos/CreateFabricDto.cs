using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shared.Dtos.FabricDtos
{
    public class CreateFabricDto
    {
        [Required]
        public string Fabric_Name { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Metres must be greater than zero")]
        public decimal Metres { get; set; }

        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public decimal Price { get; set; }

        [Required]
        public int Trader_Id { get; set; }
    }
}
