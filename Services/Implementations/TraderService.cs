using Database.Models;
using Microsoft.Identity.Client;
using Repository.Interfaces;
using Services.Interfaces;
using Shared.Dtos.OrderDtos;
using Shared.Dtos.QueryFilters;
using Shared.Dtos.TraderDtos;
using Shared.Helper;
using Shared.Mapping;

namespace Services.Implementations
{
    public class TraderService : ITraderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        public TraderService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ViewTraderDto?> AddTrader(CreateTraderDto createTraderDto)
        {
            var trader = createTraderDto.ToTrader();

            if (trader == null)
                return null;

            var userid = _currentUserService.GetCurrentUserId();
            if (string.IsNullOrWhiteSpace(userid))
                throw new InvalidOperationException("UserID not found in authentication context.");

            trader.UserId = userid;

            await _unitOfWork.Traders.AddAsync(trader);
            await _unitOfWork.SaveChangesAsync();

            return trader.ToTraderDto();
        }

        public async Task<ViewTraderDto?> DeleteTrader(int id)
        {
            var trader = await _unitOfWork.Traders.GetByIdAsync(id);

            if (trader == null)
                return null;

            _unitOfWork.Traders.Delete(trader);
            await _unitOfWork.SaveChangesAsync();

            return trader.ToTraderDto();
        }

        public async Task<IEnumerable<ViewTraderDto>> GetAllTraders()
        {
            var traders = await _unitOfWork.Traders.GetAllAsync();

            return traders.Select(trader => trader.ToTraderDto());
        }

        public async Task<ViewTraderDto?> GetTrader(int id)
        {
            var trader = await _unitOfWork.Traders.GetByIdAsync(id);

            if (trader == null)
                return null;

            return trader.ToTraderDto();
        }

        public async Task<IEnumerable<ViewTraderDto>> GetTradersByFilter(TraderFilter traderFilter)
        {
            var traders = await _unitOfWork.Traders.GetTradersByFilterAsync(
                traderName: traderFilter.TraderName,
                type: traderFilter.Type);

            return traders.Select(trader => trader.ToTraderDto());
        }

        public async Task<IEnumerable<ViewTraderDto>> GetTradersByName(string name)
        {
            var traders = await _unitOfWork.Traders.GetTraderByNameAsync(name);

            return traders.Select(trader => trader.ToTraderDto());
        }

        public async Task<IEnumerable<ViewTraderDto>> GetTradersByType(TraderType traderType)
        {
            var traders = await _unitOfWork.Traders.GetTraderByTypeAsync(traderType);

            return traders.Select(trader => trader.ToTraderDto());
        }

        public IEnumerable<object> GetTraderTypes()
        {
           var types = EnumHelper.GetEnumList<TraderType>();

            return types;
        }

        public async Task<ViewTraderDto?> UpdateTrader(int id, UpdateTraderDto updateTraderDto)
        {
            var trader = await _unitOfWork.Traders.GetByIdAsync(id);

            if (trader == null)
                return null;

            trader.UpdateTrader(updateTraderDto);
            await _unitOfWork.SaveChangesAsync();

            return trader.ToTraderDto();
        }
    }
}
