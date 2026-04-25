using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityDepartmentAPI.Data;
using UniversityDepartmentAPI.Models;

namespace UniversityDepartmentAPI.Controllers;

[ApiController]
[Route("api/v1/disciplines")]
[Authorize(Roles = "Secretary,Teacher")]
public class DisciplinesController : ControllerBase
{
    private readonly AppDbContext _context;
    public DisciplinesController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _context.Disciplines.ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var discipline = await _context.Disciplines.FindAsync(id);
        return discipline == null ? NotFound() : Ok(discipline);
    }

    [HttpPost]
    [Authorize(Roles = "Secretary")]
    public async Task<IActionResult> Create([FromBody] Discipline discipline)
    {
        _context.Disciplines.Add(discipline);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = discipline.Id }, discipline);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Secretary")]
    public async Task<IActionResult> Update(int id, [FromBody] Discipline discipline)
    {
        if (id != discipline.Id) return BadRequest();
        _context.Entry(discipline).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Secretary")]
    public async Task<IActionResult> Delete(int id)
    {
        var discipline = await _context.Disciplines.FindAsync(id);
        if (discipline == null) return NotFound();
        _context.Disciplines.Remove(discipline);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}