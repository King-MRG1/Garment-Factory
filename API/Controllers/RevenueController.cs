using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Shared.Dtos.QueryFilters;
using Shared.Dtos.RevenueDtos;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RevenueController : ControllerBase
    {
        private readonly IUnitOfServices _unitOfServices;
        public RevenueController(IUnitOfServices unitOfServices)
        {
            _unitOfServices = unitOfServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetRevenue([FromQuery] RevenueFilter revenueFilter)
        {
            var revenues = await _unitOfServices.Revenues.GetRevenuesByFilterAsync(revenueFilter);

            return Ok(revenues);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRevenueById(int id)
        {
            var revenue = await _unitOfServices.Revenues.GetRevenueByIdAsync(id);

            if (revenue == null)
                return NotFound();

            return Ok(revenue);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRevenue([FromBody] CreateRevenueDto createRevenue)
        {
            var revenue = await _unitOfServices.Revenues.AddRevenueAsync(createRevenue);

            if (revenue == null)
                return BadRequest("Failed to create revenue.");

            return CreatedAtAction(nameof(GetRevenueById), new { id = revenue.Id }, revenue);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRevenue(int id, [FromBody] UpdateRevenueDto updateRevenue)
        {
            var revenue = await _unitOfServices.Revenues.UpdateRevenueAsync(id, updateRevenue);

            if (revenue == null)
                return NotFound();

            return Ok(revenue);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRevenue(int id)
        {
            var revenue = await _unitOfServices.Revenues.DeleteRevenue(id);

            if (revenue == null)
                return NotFound();

            return NoContent();
        }
    }
}
