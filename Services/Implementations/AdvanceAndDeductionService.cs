using Database.Models;
using Services.Interfaces;
using Shared.Dtos;
using Shared.Helper;

namespace Services.Implementations
{
    public class AdvanceAndDeductionService : IAdvanceAndDeductionService
    {
        public IEnumerable<ViewEnumDto> GetTypes()
        {
            var types = EnumHelper.GetEnumList<AdvanceOrDeduction>();

            return types;
        }
    }
}
