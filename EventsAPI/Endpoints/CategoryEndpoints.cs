using Microsoft.EntityFrameworkCore;
using EventsAPI.Data;
using EventsDTO;

namespace EventsAPI.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints (this IEndpointRouteBuilder routes)
    {
        // Get all
        routes.MapGet("/api/Categories", async (ApplicationDbContext db) =>
        {
            return await db.Category.AsNoTracking()
                        .Include(c => c.Talks)
                        .Select(m => m.MapCategoryResponse())
                        .ToListAsync()
            is List<CategoryResponse> model
                ? Results.Ok(model)
                : Results.NotFound();
        })
        .WithTags("Category")
        .WithName("GetAllCategories")
        .Produces<List<CategoryResponse>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        // Get by id
        routes.MapGet("/api/Categories/{id}", async (int id, ApplicationDbContext db) =>
        {
            return await db.Category.AsNoTracking()
                        .Include(c => c.Talks)
                        .SingleOrDefaultAsync(c => c.Id == id)
            is Data.Category model
                ? Results.Ok(model.MapCategoryResponse())
                : Results.NotFound(new { Category = id });
        })
        .WithTags("Category")
        .WithName("GetCategoryById")
        .Produces<CategoryResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        // Create
        routes.MapPost("/api/Categories/", async (EventsDTO.Category input, ApplicationDbContext db) =>
        {
            // Check if exist
            var existingCategory = await db.Category
                        .Where(c => c.Name == input.Name)
                        .FirstOrDefaultAsync();

            if (existingCategory == null)
            {
                var category = new Data.Category
                {
                    Id = input.Id,
                    Name = input.Name
                };

                db.Category.Add(category);
                await db.SaveChangesAsync();

                return Results.Created($"/api/Categorys/{category.Id}", category.MapCategoryResponse());
            }
            else
            {
                return Results.Conflict(new { Error = $"Category with name '{input.Name}' already exists" });
            }
        })
        .WithTags("Category")
        .WithName("CreateCategory")
        .Produces<CategoryResponse>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status409Conflict);

        // Update
        routes.MapPut("/api/Categories/{id}", async (int id, EventsDTO.Category input, ApplicationDbContext db) =>
        {
            // Check if exist
            var category = await db.Category.SingleOrDefaultAsync(c => c.Id == id);

            if (category is null)
            {
                return Results.NotFound(new { Category = id });
            }

            // Check if Name is duplicated ignoring own id
            var duplicatedCategory = await db.Category
                        .Where(c => c.Name == input.Name &&
                                    c.Id != id)
                        .FirstOrDefaultAsync();

            if (duplicatedCategory == null)
            {
                category.Name = input.Name;

                await db.SaveChangesAsync();

                return Results.NoContent();
            }
            else
            {
                return Results.Conflict(new { Error = $"Another Category already has the name '{input.Name}'" });
            }
        })
        .WithTags("Category")
        .WithName("UpdateCategory")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status409Conflict);

        // Delete
        routes.MapDelete("/api/Categories/{id}", async (int id, ApplicationDbContext db) =>
        {
            // Check if exist
            if (await db.Category.SingleOrDefaultAsync(c => c.Id == id) is Data.Category category)
            {
                db.Category.Remove(category);
                await db.SaveChangesAsync();
                return Results.Ok();
            }

            return Results.NotFound(new { Category = id });
        })
        .WithTags("Category")
        .WithName("DeleteCategory")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
