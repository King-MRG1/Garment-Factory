using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Dtos.AdvanceAndDeductionDtos
{
    public class ViewAdvanceAndDeductionDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public DateOnly Date { get; set; }
        public string Worker_Name { get; set; }
    }
}
