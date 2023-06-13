using Microsoft.EntityFrameworkCore;
using EventsAPI.Data;
using EventsDTO;

namespace EventsAPI.Endpoints;

public static class GuestEndpoints
{
    public static void MapGuestEndpoints (this IEndpointRouteBuilder routes)
    {
        // Get all including many-to-many
        routes.MapGet("/api/Guest", async (ApplicationDbContext db) =>
        {
            return await db.Host.AsNoTracking()
                        .Include(g => g.EventGuests)
                        .ThenInclude(eg => eg.Event)
                        .Include(g=> g.TalkGuests)
                        .ThenInclude(tg => tg.Talk)
                        .Select(m => m.MapGuestResponse())
                        .ToListAsync()
            is List<GuestResponse> model
                ? Results.Ok(model)
                : Results.NotFound();
        })
        .WithTags("Guest")
        .WithName("GetAllGuests")
        .Produces<List<GuestResponse>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        // Get by id including many-to-many
        routes.MapGet("/api/Guest/{id}", async (int id, ApplicationDbContext db) =>
        {
            return await db.Host.AsNoTracking()
                        .Include(g => g.EventGuests)
                        .ThenInclude(eg => eg.Event)
                        .Include(g => g.TalkGuests)
                        .ThenInclude(tg => tg.Talk)
                        .SingleOrDefaultAsync(g => g.Id == id)
                is Data.Guest model
                    ? Results.Ok(model.MapGuestResponse())
                    : Results.NotFound();
        })
        .WithTags("Guest")
        .WithName("GetGuestById")
        .Produces<GuestResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        // Create
        routes.MapPost("/api/Guest/", async (EventsDTO.Guest input, ApplicationDbContext db) =>
        {
            var guest = new Data.Guest
            {
                Id = input.Id,
                FullName = input.FullName,
                Bio = input.Bio,
                Social = input.Social,
                WebSite = input.WebSite
            };

            db.Host.Add(guest);
            await db.SaveChangesAsync();

            return Results.Created($"/api/Guests/{guest.Id}", guest.MapGuestResponse());
        })
        .WithTags("Guest")
        .WithName("CreateGuest")
        .Produces<GuestResponse>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status409Conflict);

        // Update
        routes.MapPut("/api/Guest/{id}", async (int id, EventsDTO.Guest input, ApplicationDbContext db) =>
        {
            // Check if exist
            var guest = await db.Host.SingleOrDefaultAsync(g => g.Id == id);

            if (guest is null)
            {
                return Results.NotFound();
            }

            guest.FullName = input.FullName ?? guest.FullName;
            guest.Bio = input.Bio ?? guest.Bio;
            guest.Social = input.Social ?? guest.Social;
            guest.WebSite = input.WebSite ?? guest.WebSite;

            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithTags("Guest")
        .WithName("UpdateGuest")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        // Delete
        routes.MapDelete("/api/Guest/{id}", async (int id, ApplicationDbContext db) =>
        {
            // Check if exist
            if (await db.Host.SingleOrDefaultAsync(g => g.Id == id) is Data.Guest guest)
            {
                db.Host.Remove(guest);
                await db.SaveChangesAsync();
                return Results.Ok();
            }

            return Results.NotFound();
        })
        .WithTags("Guest")
        .WithName("DeleteGuest")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
