using Microsoft.EntityFrameworkCore;
using EventsAPI.Data;
namespace EventsAPI.Endpoints;

public static class GuestEndpoints
{
    public static void MapGuestEndpoints (this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/Guest", async (ApplicationDbContext db) =>
        {
            return await db.Host.ToListAsync();
        })
        .WithTags("Guest")
        .WithName("GetAllGuests")
        .Produces<List<Guest>>(StatusCodes.Status200OK);

        routes.MapGet("/api/Guest/{id}", async (int Id, ApplicationDbContext db) =>
        {
            return await db.Host.FindAsync(Id)
                is Guest model
                    ? Results.Ok(model)
                    : Results.NotFound();
        })
        .WithTags("Guest")
        .WithName("GetGuestById")
        .Produces<Guest>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        routes.MapPut("/api/Guest/{id}", async (int Id, Guest guest, ApplicationDbContext db) =>
        {
            var foundModel = await db.Host.FindAsync(Id);

            if (foundModel is null)
            {
                return Results.NotFound();
            }

            db.Update(guest);

            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithTags("Guest")
        .WithName("UpdateGuest")
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status204NoContent);

        routes.MapPost("/api/Guest/", async (Guest guest, ApplicationDbContext db) =>
        {
            db.Host.Add(guest);
            await db.SaveChangesAsync();
            return Results.Created($"/Guests/{guest.Id}", guest);
        })
        .WithTags("Guest")
        .WithName("CreateGuest")
        .Produces<Guest>(StatusCodes.Status201Created);

        routes.MapDelete("/api/Guest/{id}", async (int Id, ApplicationDbContext db) =>
        {
            if (await db.Host.FindAsync(Id) is Guest guest)
            {
                db.Host.Remove(guest);
                await db.SaveChangesAsync();
                return Results.Ok(guest);
            }

            return Results.NotFound();
        })
        .WithTags("Guest")
        .WithName("DeleteGuest")
        .Produces<Guest>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
