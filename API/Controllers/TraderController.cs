using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Shared.Dtos.QueryFilters;
using Shared.Dtos.TraderDtos;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TraderController : ControllerBase
    {
        private readonly IUnitOfServices _unitOfServices;
        public TraderController(IUnitOfServices unitOfServices)
        {
            _unitOfServices = unitOfServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetTraders([FromQuery] TraderFilter traderFilter)
        {
            try
            {
                var traders = await _unitOfServices.Traders.GetTradersByFilterAsync(traderFilter);
                return Ok(traders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to retrieve traders", error = ex.Message });
            }
        }

        [HttpGet("types")]
        public async Task<IActionResult> GetTraderTypes()
        {
            try
            {
                var traderTypes = await _unitOfServices.Traders.GetTraderTypesAsync();
                return Ok(traderTypes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to retrieve trader types", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTraderById(int id)
        {
            try
            {
                var trader = await _unitOfServices.Traders.GetTraderByIdAsync(id);

                if (trader == null)
                    return NotFound();

                return Ok(trader);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to retrieve trader", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddTrader([FromBody] CreateTraderDto createTraderDto)
        {
            try
            {
                var trader = await _unitOfServices.Traders.AddTraderAsync(createTraderDto);

                if (trader == null)
                    return BadRequest("Failed to create trader.");

                return CreatedAtAction(nameof(GetTraderById), new { id = trader.Id }, trader);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to add trader", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrader(int id, [FromBody] UpdateTraderDto updateTraderDto)
        {
            try
            {
                var updatedTrader = await _unitOfServices.Traders.UpdateTraderAsync(id, updateTraderDto);

                if (updatedTrader == null)
                    return NotFound();

                return Ok(updatedTrader);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to update trader", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrader(int id)
        {
            try
            {
                var deletedTrader = await _unitOfServices.Traders.DeleteTraderAsync(id);

                if (deletedTrader == null)
                    return NotFound();

                return Ok(deletedTrader);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to delete trader", error = ex.Message });
            }
        }
    }
}
