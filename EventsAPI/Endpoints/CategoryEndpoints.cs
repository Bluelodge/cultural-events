using Microsoft.EntityFrameworkCore;
using EventsAPI.Data;
namespace EventsAPI.Endpoints;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints (this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/Category", async (ApplicationDbContext db) =>
        {
            return await db.Category.ToListAsync();
        })
        .WithTags("Category").WithName("GetAllCategorys")
        .Produces<List<Category>>(StatusCodes.Status200OK);

        routes.MapGet("/api/Category/{id}", async (int Id, ApplicationDbContext db) =>
        {
            return await db.Category.FindAsync(Id)
                is Category model
                    ? Results.Ok(model)
                    : Results.NotFound();
        })
        .WithTags("Category")
        .WithName("GetCategoryById")
        .Produces<Category>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        routes.MapPut("/api/Category/{id}", async (int Id, Category category, ApplicationDbContext db) =>
        {
            var foundModel = await db.Category.FindAsync(Id);

            if (foundModel is null)
            {
                return Results.NotFound();
            }

            db.Update(category);

            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithTags("Category")
        .WithName("UpdateCategory")
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status204NoContent);

        routes.MapPost("/api/Category/", async (Category category, ApplicationDbContext db) =>
        {
            db.Category.Add(category);
            await db.SaveChangesAsync();
            return Results.Created($"/Categorys/{category.Id}", category);
        })
        .WithTags("Category")
        .WithName("CreateCategory")
        .Produces<Category>(StatusCodes.Status201Created);

        routes.MapDelete("/api/Category/{id}", async (int Id, ApplicationDbContext db) =>
        {
            if (await db.Category.FindAsync(Id) is Category category)
            {
                db.Category.Remove(category);
                await db.SaveChangesAsync();
                return Results.Ok(category);
            }

            return Results.NotFound();
        })
        .WithTags("Category")
        .WithName("DeleteCategory")
        .Produces<Category>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
