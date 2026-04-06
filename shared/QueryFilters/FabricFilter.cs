using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Shared.Dtos.QueryFilters
{
    public class FabricFilter
    {
        public string? FabricName { get; set; }
        public string? TraderName { get; set; }
         public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }

    }
}
