using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Shared.Dtos.FabricDtos;
using Shared.Dtos.QueryFilters;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FabricController : ControllerBase
    {
        private readonly IUnitOfServices _unitOfServices;
        public FabricController(IUnitOfServices unitOfServices)
        {
            _unitOfServices = unitOfServices;
        }

        [HttpGet("FabricReport")]
        public async Task<IActionResult> GetFabricReport([FromQuery] DateOnly startDate, [FromQuery] DateOnly endDate)
        {
            try
            {
                var fabrics = await _unitOfServices.Fabrics.GetFabricReportAsync(startDate, endDate);
                return Ok(fabrics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to retrieve fabric report", error = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetFabrics([FromQuery] FabricFilter fabricFilter)
        {
            try
            {
                var fabrics = await _unitOfServices.Fabrics.GetFabricsByFilterAsync(fabricFilter);
                return Ok(fabrics);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to retrieve fabrics", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFabricById(int id)
        {
            try
            {
                var fabric = await _unitOfServices.Fabrics.GetFabricByIdAsync(id);

                if (fabric == null)
                    return NotFound();

                return Ok(fabric);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to retrieve fabric", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateFabric(CreateFabricDto createFabricDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var fabric = await _unitOfServices.Fabrics.CreateFabricAsync(createFabricDto);

            if (fabric == null)
                return BadRequest();

            return CreatedAtAction(nameof(GetFabricById), new { id = fabric.Id }, fabric);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFabric(int id, UpdateFabricDto updateFabricDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var fabric = await _unitOfServices.Fabrics.UpdateFabricAsync(id, updateFabricDto);

                if (fabric == null)
                    return NotFound();

                return Ok(fabric);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to update fabric", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFabric(int id)
        {
            try
            {
                var deletedFabric = await _unitOfServices.Fabrics.DeleteFabricAsync(id);

                if (deletedFabric == null)
                    return NotFound();

                return Ok("Fabric deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to delete fabric", error = ex.Message });
            }
        }
    }
}
