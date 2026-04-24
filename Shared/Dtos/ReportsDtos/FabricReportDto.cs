using Shared.Dtos.FabricDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Dtos.ReportsDtos
{
    public class FabricReportDto
    {
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public decimal TotalMeters { get; set; }
        public decimal TotalPrice { get; set; }
        public List<ViewFabricReportDto> Fabrics { get; set; }  = new List<ViewFabricReportDto>();
    }
}
