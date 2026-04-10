using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Shared.Dtos.AdvanceAndDeductionDtos;
using Shared.Dtos.QueryFilters;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AdvanceAndDeductionController : ControllerBase
    {
        private readonly IUnitOfServices _unitOfService;
        public AdvanceAndDeductionController(IUnitOfServices unitOfService)
        {
            _unitOfService = unitOfService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAdvancesAndDeductions([FromQuery] AdvanceAndDeductionFilter filter)
        {
            var result = await _unitOfService.AdvancesAndDeductions
                .GetAdvancesAndDeductionsByFilterAsync(filter);

            return Ok(result);
        }

        [HttpGet("types")]
        public async Task<IActionResult> GetAdvanceAndDeductionTypes()
        {
            var result = await _unitOfService.AdvancesAndDeductions.GetTypesAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAdvanceOrDeductionById(int id)
        {
            var result = await _unitOfService.AdvancesAndDeductions.GetAdvanceOrDeductionByIdAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdvanceOrDeduction([FromBody] CreateAdvanceAndDeductionDto dto)
        {
            var result = await _unitOfService.AdvancesAndDeductions.CreateAdvanceOrDeductionAsync(dto);

            if (result == null)
                return BadRequest("Failed to create advance or deduction.");

            return CreatedAtAction(nameof(GetAdvanceOrDeductionById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAdvanceOrDeduction(int id, [FromBody] UpdateAdvanceAndDeductionDto dto)
        {
            var result = await _unitOfService.AdvancesAndDeductions.UpdateAdvanceOrDeductionAsync(id, dto);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdvanceOrDeduction(int id)
        {
            var result = await _unitOfService.AdvancesAndDeductions.DeleteAdvanceOrDeductionAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
