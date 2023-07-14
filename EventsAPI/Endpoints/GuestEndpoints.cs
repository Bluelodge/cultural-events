using Microsoft.EntityFrameworkCore;
using EventsAPI.Data;
using EventsDTO;

namespace EventsAPI.Endpoints;

public static class GuestEndpoints
{
    public static void MapGuestEndpoints (this IEndpointRouteBuilder routes)
    {
        // Get all including many-to-many
        routes.MapGet("/api/Guests", async (ApplicationDbContext db) =>
        {
            return await db.Guest.AsNoTracking()
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
        routes.MapGet("/api/Guests/{id}", async (int id, ApplicationDbContext db) =>
        {
            return await db.Guest.AsNoTracking()
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
        routes.MapPost("/api/Guests/", async (EventsDTO.Guest input, ApplicationDbContext db) =>
        {
            // Check if exist (composite key)
            var existingGuest = await db.Guest
                        .Where(g => g.FullName == input.FullName &&
                                    g.Position == input.Position)
                        .FirstOrDefaultAsync();

            if (existingGuest == null)
            {
                var guest = new Data.Guest
                {
                    Id = input.Id,
                    FullName = input.FullName,
                    Position = input.Position,
                    Bio = input.Bio,
                    Social = input.Social,
                    WebSite = input.WebSite
                };

                db.Guest.Add(guest);
                await db.SaveChangesAsync();

                return Results.Created($"/api/Guests/{guest.Id}", guest.MapGuestResponse());
            }
            else
            {
                return Results.Conflict();
            }

        })
        .WithTags("Guest")
        .WithName("CreateGuest")
        .Produces<GuestResponse>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status409Conflict);

        // Update
        routes.MapPut("/api/Guests/{id}", async (int id, EventsDTO.Guest input, ApplicationDbContext db) =>
        {
            // Check if exist 
            var guest = await db.Guest.SingleOrDefaultAsync(g => g.Id == id);

            if (guest is null)
            {
                return Results.NotFound();
            }

            // Check if Name and Position are duplicates (composite key)
            var fullname = input.FullName ?? guest.FullName;
            var position = input.Position ?? guest.Position;

            // Verify values ignoring own id
            var duplicatedGuest = await db.Guest
                        .Where(g => g.FullName == fullname &&
                                    g.Position == position &&
                                    g.Id != id)
                        .FirstOrDefaultAsync();

            if (duplicatedGuest == null)
            {
                guest.FullName = fullname;
                guest.Position = position;
                guest.Bio = input.Bio ?? guest.Bio;
                guest.Social = input.Social ?? guest.Social;
                guest.WebSite = input.WebSite ?? guest.WebSite;

                await db.SaveChangesAsync();

                return Results.NoContent();
            }
            else
            {
                return Results.Conflict();
            }
                
        })
        .WithTags("Guest")
        .WithName("UpdateGuest")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status409Conflict);

        // Delete
        routes.MapDelete("/api/Guests/{id}", async (int id, ApplicationDbContext db) =>
        {
            // Check if exist
            if (await db.Guest.SingleOrDefaultAsync(g => g.Id == id) is Data.Guest guest)
            {
                db.Guest.Remove(guest);
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
