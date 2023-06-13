using Microsoft.EntityFrameworkCore;
using EventsAPI.Data;
using EventsDTO;

namespace EventsAPI.Endpoints;

public static class EventEndpoints
{
    public static void MapEventEndpoints(this IEndpointRouteBuilder routes)
    {
        // Get all including many-to-many
        routes.MapGet("/api/Event", async (ApplicationDbContext db) =>
        {
            return await db.Event.AsNoTracking()
                            .Include(e => e.EventGuests)
                            .ThenInclude(eg => eg.Guest)
                            .Include(e => e.EventOrgs)
                            .ThenInclude(eo => eo.Organization)
                            .Select(m => m.MapEventResponse())
                            .ToListAsync()
            is List<EventResponse> model
                ? Results.Ok(model)
                : Results.NotFound();
        })
        .WithTags("Event")
        .WithName("GetAllEvents")
        .Produces<List<EventResponse>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        // Get by id including many-to-many
        routes.MapGet("/api/Event/{id}", async (int id, ApplicationDbContext db) =>
        {
            return await db.Event.AsNoTracking()
                        .Include(e => e.EventGuests)
                        .ThenInclude(eg => eg.Guest)
                        .Include(e => e.EventOrgs)
                        .ThenInclude(eo => eo.Organization)
                        .SingleOrDefaultAsync(e => e.Id == id)
                is Data.Event model
                    ? Results.Ok(model.MapEventResponse())
                    : Results.NotFound();
        })
        .WithTags("Event")
        .WithName("GetEventById")
        .Produces<EventResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        // Create
        routes.MapPost("/api/Event/", async (EventsDTO.Event input, ApplicationDbContext db) =>
        {
            var events = new Data.Event
            {
                Id = input.Id,
                Title = input.Title
            };

            db.Event.Add(events);
            await db.SaveChangesAsync();

            return Results.Created($"/api/Events/{events.Id}", events.MapEventResponse());
        })
        .WithTags("Event")
        .WithName("CreateEvent")
        .Produces<EventResponse>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status409Conflict);

        // Update
        routes.MapPut("/api/Event/{id}", async (int id, EventsDTO.Event input, ApplicationDbContext db) =>
        {
            // Check if exist
            var events = await db.Event.SingleOrDefaultAsync(e => e.Id == id);

            if (events is null)
            {
                return Results.NotFound();
            }

            events.Title = input.Title;

            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithTags("Event")
        .WithName("UpdateEvent")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        // Delete
        routes.MapDelete("/api/Event/{id}", async (int id, ApplicationDbContext db) =>
        {
            // Check if exist
            if (await db.Event.SingleOrDefaultAsync(e => e.Id == id) is Data.Event events)
            {
                db.Event.Remove(events);
                await db.SaveChangesAsync();
                return Results.Ok();
            }

            return Results.NotFound();
        })
        .WithTags("Event")
        .WithName("DeleteEvent")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
