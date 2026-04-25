using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityDepartmentAPI.Data;
using UniversityDepartmentAPI.Models;

namespace UniversityDepartmentAPI.Controllers;

[ApiController]
[Route("api/v1/extraworks")]
[Authorize(Roles = "Secretary,Teacher")]
public class ExtraWorksController : ControllerBase
{
    private readonly AppDbContext _context;
    public ExtraWorksController(AppDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _context.ExtraWorks.ToListAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var extraWork = await _context.ExtraWorks.FindAsync(id);
        return extraWork == null ? NotFound() : Ok(extraWork);
    }

    [HttpPost]
    [Authorize(Roles = "Secretary")]
    public async Task<IActionResult> Create([FromBody] ExtraWork extraWork)
    {
        _context.ExtraWorks.Add(extraWork);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = extraWork.Id }, extraWork);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Secretary")]
    public async Task<IActionResult> Update(int id, [FromBody] ExtraWork extraWork)
    {
        if (id != extraWork.Id) return BadRequest();
        _context.Entry(extraWork).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Secretary")]
    public async Task<IActionResult> Delete(int id)
    {
        var extraWork = await _context.ExtraWorks.FindAsync(id);
        if (extraWork == null) return NotFound();
        _context.ExtraWorks.Remove(extraWork);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}