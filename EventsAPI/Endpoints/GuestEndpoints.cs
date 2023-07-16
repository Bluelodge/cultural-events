using Microsoft.EntityFrameworkCore;
using EventsAPI.Data;
using EventsDTO;
using Swashbuckle.AspNetCore.Annotations;

namespace EventsAPI.Endpoints;

public static class GuestEndpoints
{
    public static void MapGuestEndpoints (this IEndpointRouteBuilder routes)
    {
        // Get all including many-to-many
        routes.MapGet("/api/Guests",
            [SwaggerOperation(
                Summary = "Get Guests",
                Description = "Returns all Guests"
            )]
            [SwaggerResponse(200, "Guests successfully returned")]
            [SwaggerResponse(404, "Guests don't exist")]
        async (ApplicationDbContext db) =>
        {
            return await db.Guest
                        .AsNoTracking()
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
        .Produces<List<GuestResponse>>(StatusCodes.Status200OK);

        // Get by id including many-to-many
        routes.MapGet("/api/Guests/{id}",
            [SwaggerOperation(
                Summary = "Get Guest by id",
                Description = "Returns a Guest as per id"
            )]
            [SwaggerResponse(200, "Guest successfully returned")]
            [SwaggerResponse(404, "Guest doesn't exist")]
        async (int id, ApplicationDbContext db) =>
        {
            return await db.Guest
                        .AsNoTracking()
                        .Include(g => g.TalkGuests)
                        .ThenInclude(tg => tg.Talk)
                        .SingleOrDefaultAsync(g => g.Id == id)
                is Data.Guest model
                    ? Results.Ok(model.MapGuestResponse())
                    : Results.NotFound(new { Guest = id });
        })
        .WithTags("Guest")
        .WithName("GetGuestById")
        .Produces<GuestResponse>(StatusCodes.Status200OK);

        // Create
        routes.MapPost("/api/Guests/",
            [SwaggerOperation(
                Summary = "Create single Guest",
                Description = "Adds new Guest with unique fullname"
            )]
            [SwaggerResponse(201, "Guest successfully created")]
            [SwaggerResponse(409, "Can't create Guest due to conflicts with unique key")]
        async (EventsDTO.Guest input, ApplicationDbContext db) =>
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
                return Results.Conflict(new { Error = $"Guest with name '{input.FullName}' and position '{input.Position}' already exists" });
            }
        })
        .WithTags("Guest")
        .WithName("CreateGuest")
        .Produces<GuestResponse>(StatusCodes.Status201Created);

        // Update
        routes.MapPut("/api/Guests/{id}",
            [SwaggerOperation(
                Summary = "Update single Guest by Id",
                Description = "Updates Guest info as per id"
            )]
            [SwaggerResponse(204, "Guest successfully updated")]
            [SwaggerResponse(404, "Guest doesn't exist")]
            [SwaggerResponse(409, "Can't update Guest due to conflicts with unique key")]
        async (int id, EventsDTO.Guest input, ApplicationDbContext db) =>
        {
            // Check if exist 
            var guest = await db.Guest.SingleOrDefaultAsync(g => g.Id == id);

            if (guest is null)
            {
                return Results.NotFound(new { Guest = id });
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
                return Results.Conflict(new { Error = $"Another Guest already has the name '{input.FullName}' and position '{input.Position}'" });
            }
        })
        .WithTags("Guest")
        .WithName("UpdateGuest");

        // Delete
        routes.MapDelete("/api/Guests/{id}",
            [SwaggerOperation(
                Summary = "Remove Guest by Id",
                Description = "Deletes Guest as per id"
            )]
            [SwaggerResponse(200, "Guest successfully deleted")]
            [SwaggerResponse(404, "Guest doesn't exist")]
        async (int id, ApplicationDbContext db) =>
        {
            // Check if exist
            if (await db.Guest.SingleOrDefaultAsync(g => g.Id == id) is Data.Guest guest)
            {
                db.Guest.Remove(guest);
                await db.SaveChangesAsync();
                return Results.Ok();
            }

            return Results.NotFound(new { Guest = id });
        })
        .WithTags("Guest")
        .WithName("DeleteGuest");
    }
}
