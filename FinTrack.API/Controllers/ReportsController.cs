using FinTrack.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary()
    {
        var summary = await _reportService.GetSummaryAsync();
        return Ok(summary);
    }

    [HttpGet("income-expense-summary")]
    public async Task<IActionResult> GetIncomeExpenseSummary()
    {
        var summary = await _reportService.GetIncomeExpenseSummaryAsync();
        return Ok(summary);
    }

    [HttpGet("transactions-by-category")]
    public async Task<IActionResult> GetTransactionsByCategory()
    {
        var report = await _reportService.GetTransactionsByCategoryAsync();
        return Ok(report);
    }
}