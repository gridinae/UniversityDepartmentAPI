using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UniversityDepartmentAPI.Data;
using UniversityDepartmentAPI.Models;

namespace UniversityDepartmentAPI.Controllers;

[ApiController]
[Route("api/v1/teachings")]
public class TeachingsController : ControllerBase
{
    private readonly AppDbContext _context;
    public TeachingsController(AppDbContext context) => _context = context;

    [HttpGet]
    [Authorize(Roles = "Secretary")]
    public async Task<IActionResult> GetAll() => Ok(await _context.Teachings.Include(t => t.Teacher).Include(t => t.Discipline).ToListAsync());

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var teaching = await _context.Teachings.Include(t => t.Teacher).Include(t => t.Discipline).FirstOrDefaultAsync(t => t.Id == id);
        if (teaching == null) return NotFound();

        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (role == "Teacher" && currentUserId != teaching.TeacherId.ToString()) return Forbid();

        return Ok(teaching);
    }

    [HttpGet("teacher/{teacherId}")]
    [Authorize]
    public async Task<IActionResult> GetByTeacher(int teacherId)
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (role == "Teacher" && currentUserId != teacherId.ToString()) return Forbid();

        var teachings = await _context.Teachings
            .Include(t => t.Discipline)
            .Where(t => t.TeacherId == teacherId)
            .ToListAsync();
        return Ok(teachings);
    }

    [HttpPost]
    [Authorize(Roles = "Secretary")]
    public async Task<IActionResult> Create([FromBody] Teaching teaching)
    {
        var teacherExists = await _context.Teachers.AnyAsync(t => t.Id == teaching.TeacherId);
        var disciplineExists = await _context.Disciplines.AnyAsync(d => d.Id == teaching.DisciplineId);
        if (!teacherExists || !disciplineExists) return BadRequest("Teacher or Discipline not found");

        _context.Teachings.Add(teaching);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = teaching.Id }, teaching);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Secretary")]
    public async Task<IActionResult> Update(int id, [FromBody] Teaching teaching)
    {
        if (id != teaching.Id) return BadRequest();
        _context.Entry(teaching).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Secretary")]
    public async Task<IActionResult> Delete(int id)
    {
        var teaching = await _context.Teachings.FindAsync(id);
        if (teaching == null) return NotFound();
        _context.Teachings.Remove(teaching);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}