using Database.Models;
using Microsoft.Extensions.Logging;
using Repository.Interfaces;
using Services.Interfaces;
using Shared.Dtos;
using Shared.Dtos.QueryFilters;
using Shared.Dtos.TraderDtos;
using Shared.Helper;
using Shared.Interfaces;
using Shared.Mapping;

namespace Services.Implementations
{
    public class TraderService : ITraderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<TraderService> _logger; 
        public TraderService(
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            ILogger<TraderService> logger)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<ViewTraderDto?> AddTraderAsync(CreateTraderDto createTraderDto)
        {
            var userContext = LoggingHelper.GetUserContext(_currentUserService);

            try
            {
                _logger.LogInformation("{userContext} - Creating new Trader", userContext);

                var trader = createTraderDto.ToTrader();

                if (trader == null)
                    throw new InvalidOperationException("Failed to map trader DTO to entity.");

                var userid = _currentUserService.GetCurrentUserId();
                if (string.IsNullOrWhiteSpace(userid))
                    throw new InvalidOperationException("UserID not found in authentication context.");

                trader.UserId = userid;

                foreach (var phone in trader.Phones)
                {
                    phone.UserId = userid;
                }

                await _unitOfWork.Traders.AddAsync(trader);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("{userContext} - Trader created successfully. Trader ID: {Id}", userContext, trader.Id);

                return await GetTraderByIdAsync(trader.Id);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("{userContext} - Validation error: {Message}", userContext, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("{userContext} - Error creating trader: {Message}", userContext, ex.Message);
                throw;
            }
        }

        public async Task<ViewTraderDto?> DeleteTraderAsync(int id)
        {
            var userContext = LoggingHelper.GetUserContext(_currentUserService);

            try
            {
                _logger.LogInformation("{userContext} - Deleting trader {Id}", userContext, id);

                var trader = await _unitOfWork.Traders.GetByIdAsync(id);

                if (trader == null)
                {
                    _logger.LogWarning("{userContext} - Trader {Id} not found", userContext, id);
                    return null;
                }

                _unitOfWork.Traders.Delete(trader);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("{userContext} - Trader {Id} deleted successfully", userContext, id);

                return await GetTraderByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError("{userContext} - Error deleting trader: {Message}", userContext, ex.Message);
                throw;
            }
        }

        public async Task<ViewTraderDto?> GetTraderByIdAsync(int id)
        {
            try
            {
                var trader = await _unitOfWork.Traders.GetByIdAsync(id);

                if (trader == null)
                    return null;

                return trader.ToTraderDto();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error retrieving trader {Id}: {Message}", id, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<ViewTraderDto>> GetTradersByFilterAsync(TraderFilter traderFilter)
        {
            var userContext = LoggingHelper.GetUserContext(_currentUserService);

            try
            {
                _logger.LogInformation("{userContext} - Retrieving Traders by filter", userContext);

                var traders = await _unitOfWork.Traders.GetTradersByFilterAsync(
                    traderName: traderFilter.TraderName,
                    type: traderFilter.Type);

                _logger.LogInformation("{userContext} - Retrieved {Count} traders", userContext, traders.Count());

                return traders.Select(trader => trader.ToTraderDto());
            }
            catch (Exception ex)
            {
                _logger.LogError("{userContext} - Error retrieving traders: {Message}", userContext, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<ViewEnumDto>> GetTraderTypesAsync()
        {
            try
            {
                _logger.LogInformation("Retrieving trader types");

                var types = EnumHelper.GetEnumList<TraderType>();

                return types;
            }
            catch (Exception ex)
            {
                _logger.LogError("Error retrieving trader types: {Message}", ex.Message);
                throw;
            }
        }

        public async Task<ViewTraderDto?> UpdateTraderAsync(int id, UpdateTraderDto updateTraderDto)
        {
            var userContext = LoggingHelper.GetUserContext(_currentUserService);

            try
            {
                _logger.LogInformation("{userContext} - Updating trader {Id}", userContext, id);

                var trader = await _unitOfWork.Traders.GetByIdAsync(id);

                if (trader == null)
                {
                    _logger.LogWarning("{userContext} - Trader {Id} not found", userContext, id);
                    return null;
                }

                trader.UpdateTrader(updateTraderDto);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("{userContext} - Trader {Id} updated successfully", userContext, id);

                return await GetTraderByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError("{userContext} - Error updating trader: {Message}", userContext, ex.Message);
                throw;
            }
        }
    }
}
