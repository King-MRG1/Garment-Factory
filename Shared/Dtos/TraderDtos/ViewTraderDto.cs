using Shared.Dtos.ExpenseDtos;
using Shared.Dtos.FabricDtos;
using Shared.Dtos.OrderDtos;
using Shared.Dtos.PhoneDtos;
using Shared.Dtos.RevenueDtos;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Shared.Dtos.TraderDtos
{
    public class ViewTraderDto
    {
        public int Id { get; set; }
        public string Trader_Name { get; set; }
        public List<PhoneDto> Phones { get; set; } = new List<PhoneDto>();
        public string Address { get; set; }
        public string Trader_Type { get; set; }
        public decimal Amount { get; set; }
        public DateOnly Register_date { get; set; }
        public List<ViewFabricDto> Fabrics { get; set; } = new List<ViewFabricDto>();
        public List<ViewRevenueDto> Payments { get; set; } = new List<ViewRevenueDto>();
        public List<ViewExpenseDto> Purchases { get; set; } = new List<ViewExpenseDto>();
        public List<ViewOrderDto> Orders { get; set; } = new List<ViewOrderDto>();
        public DateOnly Last_Update { get; set; }
    }
}
