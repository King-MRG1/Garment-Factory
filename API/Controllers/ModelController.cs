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
            var models = await _unitOfServices.Models.GetModelsByFilterAsync(modelFilter);

            return Ok(models);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetModelById(int id)
        {
            var model = await _unitOfServices.Models.GetModelByIdAsync(id);

            if (model == null)
                return NotFound();

            return Ok(model);
        }

        [HttpPost("AddUnits")]
        public async Task<IActionResult> AddUnitsToModel(int modelId, int units)
        {
            var updatedModel = await _unitOfServices.Models.AddQuantityToModelAsync(modelId, units);

            if(updatedModel == null)
                return NotFound();

            return Ok(updatedModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateModel([FromBody] CreateModelDto createModelDto)
        {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdModel = await _unitOfServices.Models.CreateModelAsync(createModelDto);
            if (createdModel == null)
                return BadRequest("Failed to create model.");

            return CreatedAtAction(nameof(GetModelById), new { id = createdModel.Id }, createdModel);
        }

        [HttpPut("/{id}")]
        public async Task<IActionResult> UpdateModel(int id, [FromBody] UpdateModelDto updateModelDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedModel = await _unitOfServices.Models.UpdateModelAsync(id, updateModelDto);
            if (updatedModel == null)
                return NotFound();

            return Ok(updatedModel);
        }

        [HttpDelete("/{id}")]
        public async Task<IActionResult> DeleteModel(int id)
        {
            var deletedModel = await _unitOfServices.Models.DeleteModelAsync(id);

            if (deletedModel == null)
                return NotFound();

            return Ok("Deleted successfully.");
        }    
    }
}
