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
            var fabrics = await _unitOfServices.Fabrics.GetFabricReportAsync(startDate, endDate);

            return Ok(fabrics);
        }

        [HttpGet]
        public async Task<IActionResult> GetFabrics([FromQuery] FabricFilter fabricFilter)
        {
            var fabrics = await _unitOfServices.Fabrics.GetFabricsByFilterAsync(fabricFilter);

            return Ok(fabrics);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFabricById(int id)
        {
            var fabric = await _unitOfServices.Fabrics.GetFabricByIdAsync(id);

            if (fabric == null)
                return NotFound();

            return Ok(fabric);
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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var fabric = await _unitOfServices.Fabrics.UpdateFabricAsync(id, updateFabricDto);

            if (fabric == null)
                return NotFound();

            return Ok(fabric);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFabric(int id)
        {
            var deletedFabric = await _unitOfServices.Fabrics.DeleteFabricAsync(id);

            if (deletedFabric == null)
                return NotFound();

            return Ok("Fabric deleted successfully");

        }
    }
}
