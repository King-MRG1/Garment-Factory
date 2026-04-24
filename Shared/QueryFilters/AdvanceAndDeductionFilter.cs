using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Dtos.QueryFilters
{
    public class AdvanceAndDeductionFilter
    {
        public int? Type { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string? WorkerName { get; set; }
        public bool IsUsed { get; set; }
    }
}
