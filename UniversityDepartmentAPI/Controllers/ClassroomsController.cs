using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityDepartmentAPI.Data;
using UniversityDepartmentAPI.Models;

namespace UniversityDepartmentAPI.Controllers;

[ApiController]
[Route("api/v1/classrooms")]
[Authorize(Roles = "Secretary,Teacher")]
public class ClassroomsController : ControllerBase
{
    private readonly AppDbContext _context;
    public ClassroomsController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _context.Classrooms.ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var classroom = await _context.Classrooms.FindAsync(id);
        return classroom == null ? NotFound() : Ok(classroom);
    }

    [HttpPost]
    [Authorize(Roles = "Secretary")]
    public async Task<IActionResult> Create([FromBody] Classroom classroom)
    {
        _context.Classrooms.Add(classroom);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = classroom.Id }, classroom);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Secretary")]
    public async Task<IActionResult> Update(int id, [FromBody] Classroom classroom)
    {
        if (id != classroom.Id) return BadRequest();
        _context.Entry(classroom).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Secretary")]
    public async Task<IActionResult> Delete(int id)
    {
        var classroom = await _context.Classrooms.FindAsync(id);
        if (classroom == null) return NotFound();
        _context.Classrooms.Remove(classroom);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}