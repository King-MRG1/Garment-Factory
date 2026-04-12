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
            try
            {
                var expenses = await _unitOfServices.Expenses.GetExpensesByFilterAsync(expenseFilter);
                return Ok(expenses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to retrieve expenses", error = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExpenseById(int id)
        {
            try
            {
                var expense = await _unitOfServices.Expenses.GetExpenseByIdAsync(id);

                if (expense == null)
                    return NotFound();

                return Ok(expense);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to retrieve expense", error = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddExpense([FromBody] CreateExpenseDto createExpenseDto)
        {
            try
            {
                var expense = await _unitOfServices.Expenses.AddExpenseAsync(createExpenseDto);

                if (expense == null)
                    return BadRequest("Failed to create expense.");

                return Ok(expense);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to add expense", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateExpense(int id, [FromBody] UpdateExpenseDto updateExpenseDto)
        {
            try
            {
                var expense = await _unitOfServices.Expenses.UpdateExpenseAsync(id, updateExpenseDto);

                if (expense == null)
                    return NotFound();

                return Ok(expense);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to update expense", error = ex.Message });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            try
            {
                var expense = await _unitOfServices.Expenses.DeleteExpenseAsync(id);

                if (expense == null)
                    return NotFound();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to delete expense", error = ex.Message });
            }
        }
    }
}
