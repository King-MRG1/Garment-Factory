using Database.Models;
using Services.Interfaces;
using Shared.Helper;

namespace Services.Implementations
{
    public class AdvanceAndDeductionService : IAdvanceAndDeductionService
    {
        public IEnumerable<object> GetTypes()
        {
            var types = EnumHelper.GetEnumList<AdvanceOrDeduction>();

            return types;
        }
    }
}
