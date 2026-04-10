using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Shared.Dtos.ExpenseDtos;
using Shared.Dtos.QueryFilters;

namespace API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IUnitOfServices _unitOfServices;
        public ExpenseController(IUnitOfServices unitOfServices)
        {
            _unitOfServices = unitOfServices;
        }

        [HttpGet]
        public async Task<IActionResult> GetExpenses([FromQuery] ExpenseFilter expenseFilter)
        {
            var expenses = await _unitOfServices.Expenses.GetExpensesByFilterAsync(expenseFilter);
            return Ok(expenses);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExpenseById(int id)
        {
            var expense = await _unitOfServices.Expenses.GetExpenseByIdAsync(id);

            if (expense == null)
                return NotFound();

            return Ok(expense);
        }

        [HttpPost]
        public async Task<IActionResult> AddExpense([FromBody] CreateExpenseDto createExpenseDto)
        {
            var expense = await _unitOfServices.Expenses.AddExpenseAsync(createExpenseDto);

            if (expense == null)
                return BadRequest("Failed to create expense.");

            return Ok(expense);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpense(int id, [FromBody] UpdateExpenseDto updateExpenseDto)
        {
            var expense = await _unitOfServices.Expenses.UpdateExpenseAsync(id, updateExpenseDto);

            if (expense == null)
                return NotFound();

            return Ok(expense);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var expense = await _unitOfServices.Expenses.DeleteExpenseAsync(id);

            if (expense == null)
                return NotFound();

            return NoContent();

        }
    }
}
