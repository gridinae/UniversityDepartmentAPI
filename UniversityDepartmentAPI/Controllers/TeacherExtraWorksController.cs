using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using UniversityDepartmentAPI.Data;
using UniversityDepartmentAPI.Models;

namespace UniversityDepartmentAPI.Controllers;

[ApiController]
[Route("api/v1/teacher-extraworks")]
public class TeacherExtraWorksController : ControllerBase
{
    private readonly AppDbContext _context;
    public TeacherExtraWorksController(AppDbContext context) => _context = context;

    [HttpGet]
    [Authorize(Roles = "Secretary")]
    public async Task<IActionResult> GetAll() => Ok(await _context.TeacherExtraWorks.Include(te => te.Teacher).Include(te => te.ExtraWork).ToListAsync());

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var record = await _context.TeacherExtraWorks.Include(te => te.Teacher).Include(te => te.ExtraWork).FirstOrDefaultAsync(te => te.Id == id);
        if (record == null) return NotFound();

        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (role == "Teacher" && currentUserId != record.TeacherId.ToString()) return Forbid();

        return Ok(record);
    }

    [HttpGet("teacher/{teacherId}")]
    [Authorize]
    public async Task<IActionResult> GetByTeacher(int teacherId)
    {
        var role = User.FindFirst(ClaimTypes.Role)?.Value;
        var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (role == "Teacher" && currentUserId != teacherId.ToString()) return Forbid();

        var records = await _context.TeacherExtraWorks
            .Include(te => te.ExtraWork)
            .Where(te => te.TeacherId == teacherId)
            .ToListAsync();
        return Ok(records);
    }

    [HttpPost]
    [Authorize(Roles = "Secretary")]
    public async Task<IActionResult> Create([FromBody] TeacherExtraWork record)
    {
        var teacherExists = await _context.Teachers.AnyAsync(t => t.Id == record.TeacherId);
        var extraExists = await _context.ExtraWorks.AnyAsync(e => e.Id == record.ExtraWorkId);
        if (!teacherExists || !extraExists) return BadRequest("Teacher or ExtraWork not found");

        _context.TeacherExtraWorks.Add(record);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = record.Id }, record);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Secretary")]
    public async Task<IActionResult> Update(int id, [FromBody] TeacherExtraWork record)
    {
        if (id != record.Id) return BadRequest();
        _context.Entry(record).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Secretary")]
    public async Task<IActionResult> Delete(int id)
    {
        var record = await _context.TeacherExtraWorks.FindAsync(id);
        if (record == null) return NotFound();
        _context.TeacherExtraWorks.Remove(record);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}