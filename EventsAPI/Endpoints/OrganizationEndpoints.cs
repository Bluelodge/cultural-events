using Microsoft.EntityFrameworkCore;
using EventsAPI.Data;
namespace EventsAPI.Endpoints;

public static class OrganizationEndpoints
{
    public static void MapOrganizationEndpoints (this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/Organization", async (ApplicationDbContext db) =>
        {
            return await db.Organization.ToListAsync();
        })
        .WithTags("Organization")
        .WithName("GetAllOrganizations")
        .Produces<List<Organization>>(StatusCodes.Status200OK);

        routes.MapGet("/api/Organization/{id}", async (int Id, ApplicationDbContext db) =>
        {
            return await db.Organization.FindAsync(Id)
                is Organization model
                    ? Results.Ok(model)
                    : Results.NotFound();
        })
        .WithTags("Organization")
        .WithName("GetOrganizationById")
        .Produces<Organization>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        routes.MapPut("/api/Organization/{id}", async (int Id, Organization organization, ApplicationDbContext db) =>
        {
            var foundModel = await db.Organization.FindAsync(Id);

            if (foundModel is null)
            {
                return Results.NotFound();
            }

            db.Update(organization);

            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithTags("Organization")
        .WithName("UpdateOrganization")
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status204NoContent);

        routes.MapPost("/api/Organization/", async (Organization organization, ApplicationDbContext db) =>
        {
            db.Organization.Add(organization);
            await db.SaveChangesAsync();
            return Results.Created($"/Organizations/{organization.Id}", organization);
        })
        .WithTags("Organization")
        .WithName("CreateOrganization")
        .Produces<Organization>(StatusCodes.Status201Created);

        routes.MapDelete("/api/Organization/{id}", async (int Id, ApplicationDbContext db) =>
        {
            if (await db.Organization.FindAsync(Id) is Organization organization)
            {
                db.Organization.Remove(organization);
                await db.SaveChangesAsync();
                return Results.Ok(organization);
            }

            return Results.NotFound();
        })
        .WithTags("Organization")
        .WithName("DeleteOrganization")
        .Produces<Organization>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
