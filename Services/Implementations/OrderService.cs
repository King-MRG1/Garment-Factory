using Database.Models;
using Repository.Interfaces;
using Services.Interfaces;
using Shared.Dtos.OrderDtos;
using Shared.Dtos.QueryFilters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        public OrderService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ViewOrderDto?> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            try
            {
                var trader = await _unitOfWork.Traders.GetByIdAsync(createOrderDto.Trader_Id);
                if (trader == null)
                    throw new Exception($"Trader with ID {createOrderDto.Trader_Id} not found.");

                var userId = _currentUserService.GetCurrentUserId();
                if (string.IsNullOrWhiteSpace(userId))
                    throw new InvalidOperationException("UserID not found in authentication context.");

                var order = createOrderDto.ToOrder();

                var ordermodels = await ProcessOrderModelsAsync(order.OrderModels, createOrderDto.Is_Rental);

                var totalCost = ordermodels.Sum(om => om.Price);
                var totalQuantity = ordermodels.Sum(om => om.Quantity);

                trader.Amount += totalCost;

                order.UserId = userId;

                await _unitOfWork.Orders.AddAsync(order);

                await _unitOfWork.SaveChangesAsync();

                var orderDto = order.ToOrderDto();
                orderDto.OrderModels = ordermodels;
                orderDto.Total_Cost = totalCost;
                orderDto.Total_Quantity = totalQuantity;

                return orderDto;
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException($"Order creation failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while creating the order: {ex.Message}");
            }
        }

        private async Task<List<ViewOrderModelDto>>
            ProcessOrderModelsAsync(IEnumerable<OrderModel> orderModels, bool isRental)
        {
            var modelIds = orderModels.Select(om => om.Model_Id).ToList();

            var models = (modelIds.Count > 0)
                ? await _unitOfWork.Models.GetModelsByIdsAsync(modelIds)
                : new List<Model>();

            var ordermodels = new List<ViewOrderModelDto>();

            foreach (var orderModel in orderModels)
            {
                var model = models.FirstOrDefault(m => m.Id == orderModel.Model_Id);
                if (model == null)
                    throw new Exception($"Model with ID {orderModel.Model_Id} not found.");

                if (orderModel.Quantity > model.Total_Units)
                    throw new InvalidOperationException(
                        $"Not enough units for model '{model.Model_Name}'. " +
                        $"Available: {model.Total_Units}, Requested: {orderModel.Quantity}.");

                model.Total_Units -= orderModel.Quantity;

                ordermodels.Add(new ViewOrderModelDto
                {
                    Model_Id = orderModel.Model_Id,
                    Order_Id = orderModel.Order_Id,
                    Model_Name = model.Model_Name,
                    Quantity = orderModel.Quantity,
                    Price = isRental ? model.Price_Trader_Rent * orderModel.Quantity
                    : model.Price_Trader_Cash * orderModel.Quantity
                });
            }

            return ordermodels;
        }

        public async Task<ViewOrderDto?> DeleteOrderAsync(int id)
        {
            var order = await _unitOfWork.Orders.GetByIdAsync(id);

            if (order == null)
                return null;

            _unitOfWork.Orders.Delete(order);
            await _unitOfWork.SaveChangesAsync();

            return order.ToOrderDto();
        }

        public async Task<IEnumerable<ViewOrderDto>> GetAllOrdersAsync()
        {
            var orders = await _unitOfWork.Orders.GetOrdersAsync();

            return orders.Select(o => o.ToOrderDto());
        }

        public async Task<ViewOrderDto?> GetOrderByIdAsync(int id)
        {
            var order = await _unitOfWork.Orders.GetOrderByIdAsync(id);

            if (order == null)
                return null;

            return order.ToOrderDto();
        }

        public async Task<IEnumerable<ViewOrderDto>> GetOrdersByModelIdAsync(int modelId)
        {
            var orders = await _unitOfWork.Orders.GetOrdersByModelIdAsync(modelId);

            return orders.Select(o => o.ToOrderDto());
        }

        public async Task<IEnumerable<ViewOrderDto>> GetOrdersByTraderIdAsync(int traderId)
        {
            var order = await _unitOfWork.Orders.GetOrdersByTraderIdAsync(traderId);

            return order.Select(o => o.ToOrderDto());
        }

        public async Task<ViewOrderDto?> UpdateOrderAsync(int id, UpdateOrderDto updateOrderDto)
        {
            var order = await _unitOfWork.Orders.GetOrderByIdAsync(id);

            if (order == null)
                return null;

            order.UpdateOrder(updateOrderDto);
            await _unitOfWork.SaveChangesAsync();

            return order.ToOrderDto();
        }

        public async Task<IEnumerable<ViewOrderDto>> GetOrdersByFilterAsync(OrderFilter orderFilter)
        {
            var orders = await _unitOfWork.Orders.GetOrdersByFilterAsync
                (modelName: orderFilter.ModelName,
            traderName: orderFilter.TraderName);

            return orders.Select(o => o.ToOrderDto());
        }
    }
}
