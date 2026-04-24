using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace Shared.Dtos.ModelDtos
{
    public class CreateModelDto
    {
        [Required]
        public string Model_Name { get; set; }
        [Range(0.01, double.MaxValue)]
        public decimal Price_Trader_Cash { get; set; }
        [Range(0.01, double.MaxValue)]
        public decimal Price_Trader_Rent { get; set; }
        [Range(0.01, double.MaxValue)]
        public decimal Price_Stitcher { get; set; }
        [Range(0.01, double.MaxValue)]
        public decimal Price_Iron { get; set; }
        [Range(0.01, double.MaxValue)]
        public decimal Price_Cutter { get; set; }
    }
}
