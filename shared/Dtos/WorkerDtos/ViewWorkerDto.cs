using Shared.Dtos.AdvanceAndDeductionDtos;
using Shared.Dtos.PhoneDtos;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Shared.Dtos.WorkerDtos
{
    public class ViewWorkerDto
    {
        public int Id { get; set; }
        public string Worker_Name { get; set; }
        public string Worker_Type { get; set; }
        public List<PhoneDto> Phones { get; set; } = new List<PhoneDto>();
        public string Address { get; set; }
        public DateOnly Hire_Date { get; set; }
        public List<ViewAdvanceAndDeductionDto> AdvanceAndDeduction { get; set; } = new List<ViewAdvanceAndDeductionDto>();
        public DateOnly Last_Update { get; set; }

    }
}
