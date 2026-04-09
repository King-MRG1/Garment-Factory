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
            var traders = await _unitOfServices.Traders.GetTradersByFilterAsync(traderFilter);

            return Ok(traders);
        }

        [HttpGet("types")]
        public async Task<IActionResult> GetTraderTypes()
        {
            var traderTypes = await _unitOfServices.Traders.GetTraderTypesAsync();

            return Ok(traderTypes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTraderById(int id)
        {
            var trader = await _unitOfServices.Traders.GetTraderByIdAsync(id);

            if (trader == null)
                return NotFound();

            return Ok(trader);
        }

        [HttpPost]
        public async Task<IActionResult> AddTrader([FromBody] CreateTraderDto createTraderDto)
        {
            var trader = await _unitOfServices.Traders.AddTraderAsync(createTraderDto);

            if (trader == null)
                return BadRequest("Failed to create trader.");

            return CreatedAtAction(nameof(GetTraderById), new { id = trader.Id }, trader);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrader(int id, [FromBody] UpdateTraderDto updateTraderDto)
        {
            var updatedTrader = await _unitOfServices.Traders.UpdateTraderAsync(id, updateTraderDto);

            if (updatedTrader == null)
                return NotFound();

            return Ok(updatedTrader);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrader(int id)
        {
            var deletedTrader = await _unitOfServices.Traders.DeleteTraderAsync(id);

            if (deletedTrader == null)
                return NotFound();

            return Ok(deletedTrader);
        }
    }
}
