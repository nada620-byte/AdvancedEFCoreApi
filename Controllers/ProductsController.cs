using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdvancedEFCoreApi.Data;

namespace AdvancedEFCoreApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProductsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/products
    [HttpGet]
    public async Task<IActionResult> GetProducts(
        int pageNumber = 1,
        int pageSize = 10)
    {
        if (pageNumber < 1 || pageSize < 1 || pageSize > 100)
        {
            return BadRequest("Invalid pagination values. Page size must be between 1 and 100.");
        }

        var products = await _context.Products
            .AsNoTracking()
            .OrderBy(p => p.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new
            {
                p.Id,
                p.Name,
                p.Price,
                CategoryName = p.Category.Name,
                RowVersion = Convert.ToBase64String(p.RowVersion)
            })
            .ToListAsync();

        return Ok(products);
    }

    // DELETE: api/products/1
    // Soft Delete
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound("Product not found.");
        }

        product.IsDeleted = true;

        try
        {
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict("The product was modified or deleted by another user.");
        }
    }

    // PUT: api/products/1
    // Change Tracking + Optimistic Concurrency
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(
        int id,
        [FromBody] UpdateProductDto request)
    {
        var product = await _context.Products.FindAsync(id);

        if (product == null)
        {
            return NotFound("Product not found.");
        }

        if (string.IsNullOrWhiteSpace(request.Name) || request.Price < 0)
        {
            return BadRequest("Invalid product name or price.");
        }

        if (string.IsNullOrWhiteSpace(request.RowVersion))
        {
            return BadRequest("RowVersion is required.");
        }

        byte[] originalRowVersion;

        try
        {
            originalRowVersion = Convert.FromBase64String(request.RowVersion);
        }
        catch (FormatException)
        {
            return BadRequest("Invalid RowVersion format.");
        }

        if (originalRowVersion.Length != 8)
        {
            return BadRequest("Invalid RowVersion length.");
        }

        // Use the version the client originally received
        _context.Entry(product)
            .Property(p => p.RowVersion)
            .OriginalValue = originalRowVersion;

        // Only update allowed fields
        product.Name = request.Name;
        product.Price = request.Price;

        try
        {
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict("The product was modified or deleted by another user.");
        }
    }
}

public class UpdateProductDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string RowVersion { get; set; } = string.Empty;
}