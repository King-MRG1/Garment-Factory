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
            var workers = await _unitOfServices.Workers.GetWorkersByFilterAsync(workerFilter);

            return Ok(workers);
        }

        [HttpGet("types")]
        public async Task<IActionResult> GetWorkerTypes()
        {
            var workerTypes = await _unitOfServices.Workers.GetWorkerTypesAsync();

            return Ok(workerTypes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkerById(int id)
        {
            var worker = await _unitOfServices.Workers.GetWorkerByIdAsync(id);

            if (worker == null)
                return NotFound();

            return Ok(worker);
        }

        [HttpPost("weekly-payment")]
        public async Task<IActionResult> CreateWeeklyPayment(
            [FromBody] CreateWeeklyPaymentDto createWeeklyPaymentDto,[FromQuery] bool addToExpense)
        {
            var paymentReport = await _unitOfServices.Workers.GetWeeklyPaymentAsync(createWeeklyPaymentDto,addToExpense);

            if (paymentReport == null)
                return NotFound();

            return Ok(paymentReport);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorker([FromBody] CreateWorkerDto createWorkerDto)
        {
            var worker = await _unitOfServices.Workers.CreateWorkerAsync(createWorkerDto);

            if (worker == null)
                return BadRequest();

            return Ok(worker);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorker(int id, [FromBody] UpdateWorkerDto updateWorkerDto)
        {
            var worker = await _unitOfServices.Workers.UpdateWorkerAsync(id, updateWorkerDto);

            if (worker == null)
                return NotFound();

            return Ok(worker);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorker(int id)
        {
            var deletedWorker = await _unitOfServices.Workers.DeleteWorkerAsync(id);

            if (deletedWorker == null)
                return NotFound();

            return NoContent();
        }
    }
}
