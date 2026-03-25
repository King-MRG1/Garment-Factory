using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Shared.Dtos.FabricDtos
{
    public class ViewFabricDto
    {
        public int Id { get; set; }
        public string Fabric_Name { get; set; }
        public decimal Metres { get; set; }
        public decimal Price { get; set; }
        public string Trader_Name { get; set; }
        public DateOnly Date_Added { get; set; }
        public DateOnly Last_Update { get; set; }
    }
}
