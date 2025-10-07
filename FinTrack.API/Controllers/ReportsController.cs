using FinTrack.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FinTrack.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportsController(IReportService reportService)
    {
        _reportService = reportService;
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

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary([FromQuery] int? month, [FromQuery] int? year)
    {
        try
        {
            var userId = GetUserId();
            var summary = await _reportService.GetSummaryAsync(userId, month, year);
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

    [HttpGet("expenses-by-category")]
    public async Task<IActionResult> GetExpensesByCategory([FromQuery] int? month, [FromQuery] int? year)
    {
        try
        {
            var userId = GetUserId();
            var expensesByCategory = await _reportService.GetExpensesByCategoryAsync(userId, month, year);
            return Ok(expensesByCategory);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while fetching expenses by category", error = ex.Message });
        }
    }

    [HttpGet("income-expense-summary")]
    public async Task<IActionResult> GetIncomeExpenseSummary()
    {
        try
        {
            var userId = GetUserId();
            var summary = await _reportService.GetIncomeExpenseSummaryAsync(userId);
            return Ok(summary);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while fetching income expense summary", error = ex.Message });
        }
    }

    [HttpGet("transactions-by-category")]
    public async Task<IActionResult> GetTransactionsByCategory()
    {
        try
        {
            var userId = GetUserId();
            var categories = await _reportService.GetTransactionsByCategoryAsync(userId);
            return Ok(categories);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while fetching transactions by category", error = ex.Message });
        }
    }

    [HttpGet("income-vs-expense-trend")]
    public async Task<IActionResult> GetIncomeVsExpenseTrend([FromQuery] int year)
    {
        try
        {
            var userId = GetUserId();
            var trend = await _reportService.GetIncomeVsExpenseTrendAsync(userId, year);
            return Ok(trend);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while fetching income vs expense trend", error = ex.Message });
        }
    }
}