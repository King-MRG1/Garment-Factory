using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Shared.Dtos.QueryFilters;
using Shared.Dtos.ReportsDtos;
using Shared.Dtos.WorkerDtos;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WorkerController : ControllerBase
    {
        private readonly IUnitOfServices _unitOfServices;
        public WorkerController(IUnitOfServices unitOfServices)
        {
            _unitOfServices = unitOfServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetWorkers([FromQuery] WorkerFilter workerFilter)
        {
            try
            {
                var workers = await _unitOfServices.Workers.GetWorkersByFilterAsync(workerFilter);
                return Ok(workers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to retrieve workers", error = ex.Message });
            }
        }

        [HttpGet("types")]
        public async Task<IActionResult> GetWorkerTypes()
        {
            try
            {
                var workerTypes = await _unitOfServices.Workers.GetWorkerTypesAsync();
                return Ok(workerTypes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to retrieve worker types", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkerById(int id)
        {
            try
            {
                var worker = await _unitOfServices.Workers.GetWorkerByIdAsync(id);

                if (worker == null)
                    return NotFound();

                return Ok(worker);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to retrieve worker", error = ex.Message });
            }
        }

        [HttpPost("weekly-payment")]
        public async Task<IActionResult> CreateWeeklyPayment(
            [FromBody] CreateWeeklyPaymentDto createWeeklyPaymentDto, [FromQuery] bool addToExpense)
        {
            try
            {
                var paymentReport = await _unitOfServices.Workers.GetWeeklyPaymentAsync(createWeeklyPaymentDto, addToExpense);

                if (paymentReport == null)
                    return NotFound();

                return Ok(paymentReport);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to create weekly payment", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorker([FromBody] CreateWorkerDto createWorkerDto)
        {
            try
            {
                var worker = await _unitOfServices.Workers.CreateWorkerAsync(createWorkerDto);

                if (worker == null)
                    return BadRequest();

                return Ok(worker);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to create worker", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorker(int id, [FromBody] UpdateWorkerDto updateWorkerDto)
        {
            try
            {
                var worker = await _unitOfServices.Workers.UpdateWorkerAsync(id, updateWorkerDto);

                if (worker == null)
                    return NotFound();

                return Ok(worker);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to update worker", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorker(int id)
        {
            try
            {
                var deletedWorker = await _unitOfServices.Workers.DeleteWorkerAsync(id);

                if (deletedWorker == null)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to delete worker", error = ex.Message });
            }
        }
    }
}
