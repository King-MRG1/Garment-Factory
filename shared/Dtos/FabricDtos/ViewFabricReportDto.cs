using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Dtos.FabricDtos
{
    public class ViewFabricReportDto
    {
        public string FabricName { get; set; }
        public decimal TotalMeters { get; set; }
        public decimal TotalPrice { get; set; }
        public List<string> Trader_Names { get; set; } = new List<string>();
    }
}
