using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Interfaces
{
    public interface IAdvanceAndDeductionService
    {
        public IEnumerable<ViewEnumDto> GetTypes();
    }
}
