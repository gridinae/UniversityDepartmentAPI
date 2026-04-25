using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UniversityDepartmentAPI.Data;
using UniversityDepartmentAPI.Models;

namespace UniversityDepartmentAPI.Controllers;

[ApiController]
[Route("api/v1/teachers")]
[Authorize]
public class TeachersController : ControllerBase
{
    private readonly AppDbContext _context;
    public TeachersController(AppDbContext context) => _context = context;

    [HttpGet]
    [Authorize(Roles = "Secretary")]
    public async Task<IActionResult> GetAll() => Ok(await _context.Teachers.Include(t => t.Classroom).ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (role == "Teacher" && userId != id.ToString()) return Forbid();

        var teacher = await _context.Teachers.Include(t => t.Classroom).FirstOrDefaultAsync(t => t.Id == id);
        return teacher == null ? NotFound() : Ok(teacher);
    }

    [HttpPost]
    [Authorize(Roles = "Secretary")]
    public async Task<IActionResult> Create([FromBody] Teacher teacher)
    {
        _context.Teachers.Add(teacher);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = teacher.Id }, teacher);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Secretary")]
    public async Task<IActionResult> Update(int id, [FromBody] Teacher teacher)
    {
        if (id != teacher.Id) return BadRequest();
        _context.Entry(teacher).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Secretary")]
    public async Task<IActionResult> Delete(int id)
    {
        var teacher = await _context.Teachers.FindAsync(id);
        if (teacher == null) return NotFound();
        _context.Teachers.Remove(teacher);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}