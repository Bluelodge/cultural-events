using Microsoft.EntityFrameworkCore;
using EventsAPI.Data;
using EventsDTO;

namespace EventsAPI.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints (this IEndpointRouteBuilder routes)
    {
        // Get all
        routes.MapGet("/api/Category", async (ApplicationDbContext db) =>
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
        .WithName("GetAllCategorys")
        .Produces<List<CategoryResponse>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        // Get by id
        routes.MapGet("/api/Category/{id}", async (int id, ApplicationDbContext db) =>
        {
            return await db.Category.AsNoTracking()
                        .Include(c => c.Talks)
                        .SingleOrDefaultAsync(c => c.Id == id)
            is Data.Category model
                ? Results.Ok(model.MapCategoryResponse())
                : Results.NotFound();
        })
        .WithTags("Category")
        .WithName("GetCategoryById")
        .Produces<CategoryResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        // Create
        routes.MapPost("/api/Category/", async (EventsDTO.Category input, ApplicationDbContext db) =>
        {
            var category = new Data.Category
            {
                Id = input.Id,
                Name = input.Name
            };

            db.Category.Add(category);
            await db.SaveChangesAsync();

            return Results.Created($"/api/Categorys/{category.Id}", category.MapCategoryResponse());
        })
        .WithTags("Category")
        .WithName("CreateCategory")
        .Produces<CategoryResponse>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status409Conflict);

        // Update
        routes.MapPut("/api/Category/{id}", async (int id, EventsDTO.Category input, ApplicationDbContext db) =>
        {
            // Check if exist
            var category = await db.Category.SingleOrDefaultAsync(c => c.Id == id);

            if (category is null)
            {
                return Results.NotFound();
            }

            category.Name = input.Name;

            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithTags("Category")
        .WithName("UpdateCategory")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        // Delete
        routes.MapDelete("/api/Category/{id}", async (int id, ApplicationDbContext db) =>
        {
            // Check if exist
            if (await db.Category.SingleOrDefaultAsync(c => c.Id == id) is Data.Category category)
            {
                db.Category.Remove(category);
                await db.SaveChangesAsync();
                return Results.Ok();
            }

            return Results.NotFound();
        })
        .WithTags("Category")
        .WithName("DeleteCategory")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
