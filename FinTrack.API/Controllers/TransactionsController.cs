using FinTrack.API.DTOs;
using FinTrack.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinTrack.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;

    public TransactionsController(ITransactionService transactionService)
    {
        _transactionService = transactionService;
    }

    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            throw new UnauthorizedAccessException("User ID not found in token");
        }
        return userId;
    }

    // GET /api/transactions
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? type,
        [FromQuery] string? category,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
    {
        try
        {
            var userId = GetUserId();
            IEnumerable<TransactionDto> transactions;

            if (!string.IsNullOrEmpty(type) || !string.IsNullOrEmpty(category) || startDate.HasValue || endDate.HasValue)
            {
                transactions = await _transactionService.GetByUserIdWithFiltersAsync(userId, type, category, startDate, endDate);
            }
            else
            {
                transactions = await _transactionService.GetAllByUserIdAsync(userId);
            }

            return Ok(transactions);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while fetching transactions", error = ex.Message });
        }
    }

    // GET /api/transactions/summary
    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        try
        {
            var userId = GetUserId();
            var summary = await _transactionService.GetSummaryAsync(userId);
            return Ok(summary);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while fetching summary", error = ex.Message });
        }
    }

    // GET /api/transactions/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        try
        {
            var userId = GetUserId();
            var transaction = await _transactionService.GetByIdAsync(id, userId);
            
            if (transaction == null) 
                return NotFound(new { message = "Transaction not found" });
            
            return Ok(transaction);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while fetching transaction", error = ex.Message });
        }
    }

    // POST /api/transactions
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TransactionDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validate Type
            if (dto.Type != "Income" && dto.Type != "Expense")
            {
                return BadRequest(new { message = "Type must be either 'Income' or 'Expense'" });
            }

            var userId = GetUserId();
            var result = await _transactionService.CreateAsync(dto, userId);
            
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating transaction", error = ex.Message });
        }
    }

    // PUT /api/transactions/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] TransactionDto dto)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Validate Type
            if (dto.Type != "Income" && dto.Type != "Expense")
            {
                return BadRequest(new { message = "Type must be either 'Income' or 'Expense'" });
            }

            var userId = GetUserId();
            var result = await _transactionService.UpdateAsync(id, dto, userId);
            
            if (!result) 
                return NotFound(new { message = "Transaction not found" });
            
            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while updating transaction", error = ex.Message });
        }
    }

    // DELETE /api/transactions/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var userId = GetUserId();
            var result = await _transactionService.DeleteAsync(id, userId);
            
            if (!result) 
                return NotFound(new { message = "Transaction not found" });
            
            return NoContent();
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while deleting transaction", error = ex.Message });
        }
    }
}