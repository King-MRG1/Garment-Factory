using Shared.Dtos.OrderDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Dtos.ModelDtos
{
    public class ViewModelDto
    {
        public int Id { get; set; }
        public string Model_Name { get; set; }
        public decimal Price_Stitcher { get; set; }
        public decimal Price_Iron { get; set; }
        public decimal Price_Cutter { get; set; }
        public decimal Price_Trader_Cash { get; set; }
        public decimal Price_Trader_Rent { get; set; }
        public int Total_Units { get; set; } = 0;
        public DateOnly Last_Update { get; set; }
        public List<ViewOrderDto> Orders { get; set; } = new List<ViewOrderDto>();
    }
}
