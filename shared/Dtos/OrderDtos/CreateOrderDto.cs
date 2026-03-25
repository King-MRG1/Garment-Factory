using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shared.Dtos.OrderDtos
{
    public class CreateOrderDto
    {
        [Required]
        public int Trader_Id { get; set; }

        [Required]
        [MinLength(1, ErrorMessage = "Order must have at least one model")]
        public List<OrderModelDto> OrderModels { get; set; } = new List<OrderModelDto>();
    }
}
