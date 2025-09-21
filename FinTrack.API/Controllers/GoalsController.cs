using FinTrack.API.Models;
using FinTrack.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GoalsController : ControllerBase
{
    private readonly IGoalService _goalService;

    public GoalsController(IGoalService goalService)
    {
        _goalService = goalService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var goals = await _goalService.GetAllAsync();
        return Ok(goals);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var goal = await _goalService.GetByIdAsync(id);
        if (goal == null) return NotFound();
        return Ok(goal);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Goal goal)
    {
        var result = await _goalService.CreateAsync(goal);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] Goal goal)
    {
        var result = await _goalService.UpdateAsync(id, goal);
        if (!result) return NotFound();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _goalService.DeleteAsync(id);
        if (!result) return NotFound();
        return NoContent();
    }
}