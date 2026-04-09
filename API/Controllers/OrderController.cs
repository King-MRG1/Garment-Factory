using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Shared.Dtos.OrderDtos;
using Shared.Dtos.QueryFilters;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IUnitOfServices _unitOfServices;
        public OrderController(IUnitOfServices unitOfServices)
        {
            _unitOfServices = unitOfServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders([FromQuery] OrderFilter orderFilter)
        {
            var orders = await _unitOfServices.Orders.GetOrdersByFilterAsync(orderFilter);

            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var order = await _unitOfServices.Orders.GetOrderByIdAsync(id);

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdOrder = await _unitOfServices.Orders.CreateOrderAsync(createOrderDto);

            if (createdOrder == null)
                return BadRequest("Failed to create order.");

            return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.Id }, createdOrder);
        }

        [HttpPut("/{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderDto updateOrderDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedOrder = await _unitOfServices.Orders.UpdateOrderAsync(id, updateOrderDto);

            if (updatedOrder == null)
                return BadRequest("Failed to update order.");

            return Ok(updatedOrder);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var deletedOrder = await _unitOfServices.Orders.DeleteOrderAsync(id);

            if (deletedOrder == null)
                return BadRequest("Failed to delete order.");

            return NoContent();
        }
    }
}
