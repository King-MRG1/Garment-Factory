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
            try
            {
                var result = await _unitOfService.AdvancesAndDeductions
                    .GetAdvancesAndDeductionsByFilterAsync(filter);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to retrieve advances and deductions", error = ex.Message });
            }
        }

        [HttpGet("types")]
        public async Task<IActionResult> GetAdvanceAndDeductionTypes()
        {
            try
            {
                var result = await _unitOfService.AdvancesAndDeductions.GetTypesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to retrieve types", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAdvanceOrDeductionById(int id)
        {
            try
            {
                var result = await _unitOfService.AdvancesAndDeductions.GetAdvanceOrDeductionByIdAsync(id);

                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to retrieve record", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAdvanceOrDeduction([FromBody] CreateAdvanceAndDeductionDto dto)
        {
            try
            {
                var result = await _unitOfService.AdvancesAndDeductions.CreateAdvanceOrDeductionAsync(dto);

                if (result == null)
                    return BadRequest("Failed to create advance or deduction.");

                return CreatedAtAction(nameof(GetAdvanceOrDeductionById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to create record", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAdvanceOrDeduction(int id, [FromBody] UpdateAdvanceAndDeductionDto dto)
        {
            try
            {
                var result = await _unitOfService.AdvancesAndDeductions.UpdateAdvanceOrDeductionAsync(id, dto);

                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to update record", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAdvanceOrDeduction(int id)
        {
            try
            {
                var result = await _unitOfService.AdvancesAndDeductions.DeleteAdvanceOrDeductionAsync(id);

                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to delete record", error = ex.Message });
            }
        }
    }
}
