using System.Data.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinstarTestTask.Entities;
using FinstarTestTask.Helpers;
using FinstarTestTask.Infrastructure;

namespace FinstarTestTask.Api.Controllers;

[Route("api")]
[ApiController]
public class SomeObjectsController : ControllerBase
{
    private readonly FinstarTestTaskDbContext _context;
    private readonly IServiceProvider _serviceProvider;
    public SomeObjectsController(FinstarTestTaskDbContext context, IServiceProvider serviceProvider)
    {
        _context = context;
        _serviceProvider = serviceProvider;
    }

    [Route("some-objects")]
    [HttpPost]
    public async Task<ActionResult> AddSomeObjects([FromBody] List<Dictionary<int, string>> json)
    {
        if (json == null || json.Count == 0)
        {
            return BadRequest("List of objects is empty.");
        }

        List<SomeObject> someObjects = json
            .SelectMany(dict => dict.Select(kv => new SomeObject
            {
                Code = kv.Key,
                Value = kv.Value
            }))
            .ToList();

        // Sort by Code
        someObjects.Sort((x, y) => x.Code - y.Code);

        // Clear database
        using (var scope = _serviceProvider.CreateScope())
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync($"TRUNCATE TABLE dbo.SomeObjects");
                await _context.SomeObjects.AddRangeAsync(someObjects);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine($"An unexpected exception occurred: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected exception occurred: {ex.Message}");
                return StatusCode(500, "An unexpected error occurred.");
            }
        }

        return Ok(new { Message = "Objests has been added successfully", someObjects.Count });
    }

    [Route("some-objects")]
    [HttpGet]
    public ActionResult<Page<SomeObject>> GetSomeObjects(
        [FromQuery] SomeObjectFilter filter,
        [FromQuery] SomeObjectSort sort,
        int pageNumber = 1,
        int pageSize = 10)
    {
        if (pageNumber < 1 || pageSize < 1)
        {
            return BadRequest("Page number and page size must be greater than zero.");
        }

        IQueryable<SomeObject> query = _context.SomeObjects.AsQueryable();

        // Apply filters
        query = FilteringHelper.ApplyFilters(query, filter);

        // Sorting
        query = SortingHelper.ApplySorting(query, sort);

        Page<SomeObject>? pagedObjects;
        try
        {
            // Apply pagination
            pagedObjects = PaginationHelper.ToPagedList(query, pageNumber, pageSize);
            Console.WriteLine($"Query: {query.ToQueryString()}");
        }
        catch (DbException ex)
        {
            Console.WriteLine($"An unexpected exception occurred: {ex.Message}");
            return StatusCode(500, "An unexpected error occurred.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected exception occurred: {ex.Message}");
            return StatusCode(500, "An unexpected error occurred.");
        }

        return Ok(pagedObjects);
    }
}