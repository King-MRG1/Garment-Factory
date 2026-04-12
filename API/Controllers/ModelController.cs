using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Shared.Dtos.ModelDtos;
using Shared.Dtos.QueryFilters;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ModelController : ControllerBase
    {
        private readonly IUnitOfServices _unitOfServices;
        public ModelController(IUnitOfServices unitOfServices)
        {
            _unitOfServices = unitOfServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetModels([FromQuery] ModelFilter modelFilter)
        {
            try
            {
                var models = await _unitOfServices.Models.GetModelsByFilterAsync(modelFilter);
                return Ok(models);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to retrieve models", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetModelById(int id)
        {
            try
            {
                var model = await _unitOfServices.Models.GetModelByIdAsync(id);

                if (model == null)
                    return NotFound();

                return Ok(model);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to retrieve model", error = ex.Message });
            }
        }

        [HttpPost("AddUnits")]
        public async Task<IActionResult> AddUnitsToModel(int modelId, int units)
        {
            try
            {
                var updatedModel = await _unitOfServices.Models.AddQuantityToModelAsync(modelId, units);

                if (updatedModel == null)
                    return NotFound();

                return Ok(updatedModel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to add units to model", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateModel([FromBody] CreateModelDto createModelDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var createdModel = await _unitOfServices.Models.CreateModelAsync(createModelDto);
                if (createdModel == null)
                    return BadRequest("Failed to create model.");

                return CreatedAtAction(nameof(GetModelById), new { id = createdModel.Id }, createdModel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to create model", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateModel(int id, [FromBody] UpdateModelDto updateModelDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var updatedModel = await _unitOfServices.Models.UpdateModelAsync(id, updateModelDto);
                if (updatedModel == null)
                    return NotFound();

                return Ok(updatedModel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to update model", error = ex.Message });
            }
        }

        [HttpDelete("/{id}")]
        public async Task<IActionResult> DeleteModel(int id)
        {
            try
            {
                var deletedModel = await _unitOfServices.Models.DeleteModelAsync(id);

                if (deletedModel == null)
                    return NotFound();

                return Ok("Deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to delete model", error = ex.Message });
            }
        }
    }
}
