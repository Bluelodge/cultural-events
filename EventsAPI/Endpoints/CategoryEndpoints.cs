using Microsoft.EntityFrameworkCore;
using EventsAPI.Data;
using EventsAPI.ResponseExamples;
using EventsDTO;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Annotations;

namespace EventsAPI.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints (this IEndpointRouteBuilder routes)
    {
        // Get all
        routes.MapGet("/api/Categories",
            [SwaggerOperation(
                Summary = "Get Categories",
                Description = "Returns all Categories"
            )]
            [SwaggerResponse(200, "Categories successfully returned")]
            [SwaggerResponse(404, "Categories don't exist")]
            [SwaggerResponseExample(200, typeof(CategoryExample.CategoryResponse))]
        async (ApplicationDbContext db) =>
        {
            return await db.Category
                        .AsNoTracking()
                        .Include(c => c.Talks)
                        .Select(m => m.MapCategoryResponse())
                        .ToListAsync()
            is List<CategoryResponse> model
                ? Results.Ok(model)
                : Results.NotFound();
        })
        .WithTags("Category")
        .WithName("GetAllCategories")
        .Produces<List<CategoryResponse>>(StatusCodes.Status200OK);

        // Get by id
        routes.MapGet("/api/Categories/{id}",
            [SwaggerOperation(
                Summary = "Get Category by id",
                Description = "Returns a Category as per id"
            )]
            [SwaggerResponse(200, "Category successfully returned")]
            [SwaggerResponse(404, "Category doesn't exist")]
            [SwaggerResponseExample(200, typeof(CategoryExample.CategoryResponse))]
        async (int id, ApplicationDbContext db) =>
        {
            return await db.Category
                        .AsNoTracking()
                        .Include(c => c.Talks)
                        .SingleOrDefaultAsync(c => c.Id == id)
            is Data.Category model
                ? Results.Ok(model.MapCategoryResponse())
                : Results.NotFound(new { Category = id });
        })
        .WithTags("Category")
        .WithName("GetCategoryById")
        .Produces<CategoryResponse>(StatusCodes.Status200OK);

        // Create
        routes.MapPost("/api/Categories/",
            [SwaggerOperation(
                Summary = "Create single Category",
                Description = "Adds new Attendee with unique name"
            )]
            [SwaggerResponse(201, "Category successfully created")]
            [SwaggerResponse(409, "Can't create Category due to conflicts with unique key")]
            [SwaggerResponseExample(201, typeof(CategoryExample.Category))]
        async (EventsDTO.Category input, ApplicationDbContext db) =>
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
        .Produces<CategoryResponse>(StatusCodes.Status201Created);

        // Update
        routes.MapPut("/api/Categories/{id}",
            [SwaggerOperation(
                Summary = "Update single Category by Id",
                Description = "Updates Category info as per id"
            )]
            [SwaggerResponse(204, "Category successfully updated")]
            [SwaggerResponse(404, "Category doesn't exist")]
            [SwaggerResponse(409, "Can't update Category due to conflicts with unique key")]
        async (int id, EventsDTO.Category input, ApplicationDbContext db) =>
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
        .WithName("UpdateCategory");

        // Delete
        routes.MapDelete("/api/Categories/{id}",
            [SwaggerOperation(
                Summary = "Remove Category by Id",
                Description = "Deletes Category as per id"
            )]
            [SwaggerResponse(200, "Category successfully deleted")]
            [SwaggerResponse(404, "Category doesn't exist")]
        async (int id, ApplicationDbContext db) =>
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
        .WithName("DeleteCategory");
    }
}
