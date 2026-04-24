using Database.Models;
using Microsoft.Extensions.Logging;
using Repository.Interfaces;
using Services.Interfaces;
using Shared.Dtos.OrderDtos;
using Shared.Dtos.QueryFilters;
using Shared.Helper;
using Shared.Interfaces;
using System.ComponentModel;

namespace Services.Implementations
{

    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger<OrderService> _logger;
        public OrderService(IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService,
            ILogger<OrderService> logger)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<ViewOrderDto?> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            var userContext = LoggingHelper.GetUserContext(_currentUserService);

            try
            {
                _logger.LogInformation("{userContext} - Creating order", userContext);

                var userId = ValidateUserAndGetId();

                var trader = await ValidateTraderExistsAsync(createOrderDto.Trader_Id);

                var order = CreateOrderEntity(createOrderDto);

                var (orderModels, totalCost, totalQuantity) =
                    await ProcessOrderModelsAsync(order.OrderModels, createOrderDto.Is_Rental);

                UpdateTraderAmount(trader, totalCost);

                SetOrderOwnership(order, userId);

                SetOrderDetails(order, totalQuantity, totalCost);

                await PersistOrderAsync(order);

                var orderDto = BuildOrderResponse(order, orderModels);

                _logger.LogInformation("{userContext} - Order created successfully. Order ID: {Id}",
                    userContext, order.Id);

                return orderDto;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError("{userContext} - Validation error: {Message}", userContext, ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError("{userContext} - Error creating order: {Message}", userContext, ex.Message);
                throw;
            }
        }

        public async Task<ViewOrderDto?> DeleteOrderAsync(int id)
        {
            var userContext = LoggingHelper.GetUserContext(_currentUserService);

            try
            {
                _logger.LogInformation("{userContext} - Deleting order {Id}", userContext, id);

                var order = await _unitOfWork.Orders.GetByIdAsync(id);

                if (order == null)
                {
                    _logger.LogWarning("{userContext} - Order {Id} not found", userContext, id);
                    return null;
                }

                _unitOfWork.Orders.Delete(order);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("{userContext} - Order {Id} deleted successfully", userContext, id);

                return order.ToOrderDto();
            }
            catch (Exception ex)
            {
                _logger.LogError("{userContext} - Error deleting order: {Message}", userContext, ex.Message);
                throw;
            }
        }

        public async Task<ViewOrderDto?> GetOrderByIdAsync(int id)
        {
            try
            {
                var order = await _unitOfWork.Orders.GetOrderByIdAsync(id);

                if (order == null)
                    return null;

                return order.ToOrderDto();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error retrieving order {Id}: {Message}", id, ex.Message);
                throw;
            }
        }

        public async Task<ViewOrderDto?> UpdateOrderAsync(int id, UpdateOrderDto updateOrderDto)
        {
            var userContext = LoggingHelper.GetUserContext(_currentUserService);

            try
            {
                _logger.LogInformation("{userContext} - Updating order {Id}", userContext, id);

                var order = await _unitOfWork.Orders.GetOrderByIdAsync(id);

                if (order == null)
                {
                    _logger.LogWarning("{userContext} - Order {Id} not found", userContext, id);
                    return null;
                }

                order.UpdateOrder(updateOrderDto);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("{userContext} - Order {Id} updated successfully", userContext, id);

                return await GetOrderByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError("{userContext} - Error updating order: {Message}", userContext, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<ViewOrderDto>> GetOrdersByFilterAsync(OrderFilter orderFilter)
        {
            var userContext = LoggingHelper.GetUserContext(_currentUserService);

            try
            {
                _logger.LogInformation("{userContext} - Retrieving orders by filter", userContext);

                var orders = await _unitOfWork.Orders.GetOrdersByFilterAsync
                    (modelName: orderFilter.ModelName,
                traderName: orderFilter.TraderName);

                var ordermodels = orders.SelectMany(o => o.OrderModels).ToList();

                var ordersdto = orders.Select(o => o.ToOrderDto());

                ordersdto = ordersdto.Select(o =>
                {
                    o.Total_Quantity = ordermodels.Where(om => om.Order_Id == o.Id).Sum(om => om.Quantity);
                    o.OrderModels = ordermodels.Where(om => om.Order_Id == o.Id)
                    .Select(om => om.ToOrderModelDto()).ToList();
                    return o;
                });

                _logger.LogInformation("{userContext} - Retrieved {Count} orders", userContext, ordersdto.Count());

                return ordersdto;
            }
            catch (Exception ex)
            {
                _logger.LogError("{userContext} - Error retrieving orders: {Message}", userContext, ex.Message);
                throw;
            }
        }

        private void SetOrderDetails(Order order, int totalQuantity, decimal totalCost)
        {
            order.Total_Cost = totalCost;
            order.Total_Quantity = totalQuantity;
        }

        private string ValidateUserAndGetId()
        {
            var userId = _currentUserService.GetCurrentUserId();

            if (string.IsNullOrWhiteSpace(userId))
                throw new InvalidOperationException("UserID not found in authentication context.");

            return userId;
        }

        private async Task<Trader> ValidateTraderExistsAsync(int traderId)
        {
            var trader = await _unitOfWork.Traders.GetByIdAsync(traderId);

            if (trader == null)
                throw new Exception($"Trader with ID {traderId} not found.");

            return trader;
        }

        private Order CreateOrderEntity(CreateOrderDto createOrderDto)
        {
            var order = createOrderDto.ToOrder();

            if (order == null)
                throw new InvalidOperationException("Failed to map order DTO to entity.");

            return order;
        }

        private async Task<(List<ViewOrderModelDto>, decimal, int)>
        ProcessOrderModelsAsync(IEnumerable<OrderModel> orderModels, bool isRental)
        {
            var modelIds = orderModels.Select(om => om.Model_Id).ToList();
            var models = (modelIds.Count > 0)
                ? await _unitOfWork.Models.GetModelsByIdsAsync(modelIds)
                : new List<Model>();

            var processedModels = new List<ViewOrderModelDto>();
            decimal totalCost = 0;
            int totalQuantity = 0;

            foreach (var orderModel in orderModels)
            {
                var model = ValidateModelExists(models.ToList(), orderModel.Model_Id);

                ValidateModelStock(model, orderModel.Quantity);

                model.Total_Units -= orderModel.Quantity;

                var price = CalculateOrderModelPrice(model, orderModel.Quantity, isRental);

                orderModel.Price = price;

                var viewModel = CreateOrderModelDto(orderModel, model);

                processedModels.Add(viewModel);
                totalCost += price;
                totalQuantity += orderModel.Quantity;
            }

            return (processedModels, totalCost, totalQuantity);
        }

        private Model ValidateModelExists(List<Model> models, int modelId)
        {
            var model = models.FirstOrDefault(m => m.Id == modelId);

            if (model == null)
                throw new Exception($"Model with ID {modelId} not found.");

            return model;
        }

        private void ValidateModelStock(Model model, int requestedQuantity)
        {
            if (requestedQuantity > model.Total_Units)
                throw new InvalidOperationException(
                    $"Insufficient stock for '{model.Model_Name}'. " +
                    $"Available: {model.Total_Units}, Requested: {requestedQuantity}");
        }

        private decimal CalculateOrderModelPrice(Model model, int quantity, bool isRental)
        {
            var pricePerUnit = isRental
                ? model.Price_Trader_Rent
                : model.Price_Trader_Cash;

            return pricePerUnit * quantity;
        }

        private ViewOrderModelDto CreateOrderModelDto(
                OrderModel orderModel,
                Model model)
        {

            var orderModelDto = orderModel.ToOrderModelDto(model);

            return orderModelDto;
        }

        private void UpdateTraderAmount(Trader trader, decimal orderCost)
        {
            trader.Amount += orderCost;
        }

        private void SetOrderOwnership(Order order, string userId)
        {
            order.UserId = userId;

            foreach (var orderModel in order.OrderModels)
            {
                orderModel.UserId = userId;
            }
        }

        private async Task PersistOrderAsync(Order order)
        {
            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();
        }

        private ViewOrderDto BuildOrderResponse(
                Order order,
                List<ViewOrderModelDto> orderModels)
        {
            var orderDto = order.ToOrderDto();
            foreach (var orderModel in order.OrderModels)
            {
                var model = orderModels.FirstOrDefault(m => m.Model_Id == orderModel.Model_Id);
                if (model != null)
                {
                    model.Order_Id = order.Id;
                }
            }
            orderDto.OrderModels = orderModels;

            return orderDto;
        }

    }
}
