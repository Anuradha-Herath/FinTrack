using FinTrack.API.Models;
using FinTrack.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class BudgetsController : ControllerBase
{
    private readonly IBudgetService _budgetService;

    public BudgetsController(IBudgetService budgetService)
    {
        _budgetService = budgetService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var budgets = await _budgetService.GetAllAsync();
        return Ok(budgets);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var budget = await _budgetService.GetByIdAsync(id);
        if (budget == null) return NotFound();
        return Ok(budget);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Budget budget)
    {
        var result = await _budgetService.CreateAsync(budget);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Budget budget)
    {
        var result = await _budgetService.UpdateAsync(id, budget);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _budgetService.DeleteAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}