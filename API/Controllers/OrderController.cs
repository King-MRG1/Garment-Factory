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
            try
            {
                var orders = await _unitOfServices.Orders.GetOrdersByFilterAsync(orderFilter);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to retrieve orders", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            try
            {
                var order = await _unitOfServices.Orders.GetOrderByIdAsync(id);

                if (order == null)
                    return NotFound();

                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to retrieve order", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto createOrderDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdOrder = await _unitOfServices.Orders.CreateOrderAsync(createOrderDto);

                if (createdOrder == null)
                    return BadRequest("Failed to create order.");

                return CreatedAtAction(nameof(GetOrderById), new { id = createdOrder.Id }, createdOrder);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = "Invalid operation", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to create order", error = ex.Message });
            }
        }

        [HttpPut("/{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderDto updateOrderDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updatedOrder = await _unitOfServices.Orders.UpdateOrderAsync(id, updateOrderDto);

                if (updatedOrder == null)
                    return BadRequest("Failed to update order.");

                return Ok(updatedOrder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to update order", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            try
            {
                var deletedOrder = await _unitOfServices.Orders.DeleteOrderAsync(id);

                if (deletedOrder == null)
                    return BadRequest("Failed to delete order.");

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to delete order", error = ex.Message });
            }
        }
    }
}
