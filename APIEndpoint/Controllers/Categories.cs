using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APIEndpoint.Models;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Authorization;

namespace APIEndpoint.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly RefhubContext _context;

    public CategoriesController(RefhubContext context)
    {
        _context = context;
    }

    // GET: api/categories
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
    {
        var categories = await _context.Categories.Take(10).ToListAsync();
        var url = Url.Action("GetCategoriesProtected", "Categories");
        return Ok(categories);
    }


    [HttpGet("Protected")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategoriesProtected()
    {
        var categories = await _context.Categories.Take(10).ToListAsync();
        return Ok(categories);
    }

    // GET: api/categories/{id}
    /// <summary>
    /// دریافت اطلاعات یک دسته‌بندی بر اساس ID
    /// </summary>
    /// <param name="id">شناسه دسته‌بندی</param>
    /// <returns>دسته‌بندی موردنظر</returns>
    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "دریافت یک دسته‌بندی", Description = "این متد یک دسته‌بندی را بر اساس ID برمی‌گرداند.")]
    public async Task<ActionResult<Category>> GetCategory(int id)
    {
        if (id < 0)
        {
            return BadRequest();
        }
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }
        return category;
    }

    // POST: api/categories
    [HttpPost]
    public async Task<ActionResult<Category>> PostCategory(Category category)
    {
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
    }

    // PUT: api/categories/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCategory(int id, Category category)
    {
        if (id != category.Id)
        {
            return BadRequest();
        }

        _context.Entry(category).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CategoryExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // DELETE: api/categories/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool CategoryExists(int id)
    {
        return _context.Categories.Any(e => e.Id == id);
    }
}
