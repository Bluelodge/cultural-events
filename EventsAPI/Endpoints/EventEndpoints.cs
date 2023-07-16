using Microsoft.EntityFrameworkCore;
using EventsAPI.Data;
using EventsDTO;
using Swashbuckle.AspNetCore.Annotations;

namespace EventsAPI.Endpoints;

public static class EventEndpoints
{
    public static void MapEventEndpoints(this IEndpointRouteBuilder routes)
    {
        // Get all including many-to-many
        routes.MapGet("/api/Events",
            [SwaggerOperation(
                Summary = "Get Events",
                Description = "Returns all Events"
            )]
            [SwaggerResponse(200, "Events successfully returned")]
            [SwaggerResponse(404, "Events don't exist")]
        async (ApplicationDbContext db) =>
        {
            return await db.Event
                        .AsNoTracking()
                        .Include(e => e.Talks)
                        .Select(m => m.MapEventResponse())
                        .ToListAsync()
            is List<EventResponse> model
                ? Results.Ok(model)
                : Results.NotFound();
        })
        .WithTags("Event")
        .WithName("GetAllEvents")
        .Produces<List<EventResponse>>(StatusCodes.Status200OK);

        // Get by id including many-to-many
        routes.MapGet("/api/Events/{id}",
            [SwaggerOperation(
                Summary = "Get Event by id",
                Description = "Returns an Event as per id"
            )]
            [SwaggerResponse(200, "Event successfully returned")]
            [SwaggerResponse(404, "Event doesn't exist")]
        async (int id, ApplicationDbContext db) =>
            {
                return await db.Event
                            .AsNoTracking()
                            .Include(e => e.Talks)
                            .SingleOrDefaultAsync(e => e.Id == id)
                    is Data.Event model
                        ? Results.Ok(model.MapEventResponse())
                        : Results.NotFound(new { Event = id });
            })
        .WithTags("Event")
        .WithName("GetEventById")
        .Produces<EventResponse>(StatusCodes.Status200OK);

        // Create
        routes.MapPost("/api/Events/",
            [SwaggerOperation(
                Summary = "Create single Event",
                Description = "Adds new Event with unique title"
            )]
            [SwaggerResponse(201, "Event successfully created")]
            [SwaggerResponse(409, "Can't create Event due to conflicts with unique key")]
        async (EventsDTO.Event input, ApplicationDbContext db) =>
        {
            // Check if exist
            var existingEvent = await db.Event
                        .Where(e => e.Title == input.Title)
                        .FirstOrDefaultAsync();

            if (existingEvent == null)
            {
                var events = new Data.Event
                {
                    Id = input.Id,
                    Title = input.Title
                };

                db.Event.Add(events);
                await db.SaveChangesAsync();

                return Results.Created($"/api/Events/{events.Id}", events.MapEventResponse());
            }
            else
            {
                return Results.Conflict(new { Error = $"Event with title '{input.Title}' already exists"});
            }
            
        })
        .WithTags("Event")
        .WithName("CreateEvent")
        .Produces<EventResponse>(StatusCodes.Status201Created);

        // Update
        routes.MapPut("/api/Events/{id}",
            [SwaggerOperation(
                Summary = "Update single Event by Id",
                Description = "Updates Event info as per id"
            )]
            [SwaggerResponse(204, "Event successfully updated")]
            [SwaggerResponse(404, "Event doesn't exist")]
            [SwaggerResponse(409, "Can't update Event due to conflicts with unique key")]
        async (int id, EventsDTO.Event input, ApplicationDbContext db) =>
        {
            // Check if exist
            var events = await db.Event.SingleOrDefaultAsync(e => e.Id == id);

            if (events is null)
            {
                return Results.NotFound(new { Event = id });
            }

            // Check if Title is duplicated ignoring own id
            var duplicatedEvent = await db.Event
                        .Where(e => e.Title == input.Title &&
                                    e.Id != id)
                        .FirstOrDefaultAsync();

            if (duplicatedEvent == null)
            {
                events.Title = input.Title;

                await db.SaveChangesAsync();

                return Results.NoContent();
            }
            else
            {
                return Results.Conflict(new { Error = $"Another Event already has the title '{input.Title}'" });
            }
        })
        .WithTags("Event")
        .WithName("UpdateEvent");

        // Delete
        routes.MapDelete("/api/Events/{id}",
            [SwaggerOperation(
                Summary = "Remove Event by Id",
                Description = "Deletes Event as per id"
            )]
            [SwaggerResponse(200, "Event successfully deleted")]
            [SwaggerResponse(404, "Event doesn't exist")]
        async (int id, ApplicationDbContext db) =>
        {
            // Check if exist
            if (await db.Event.SingleOrDefaultAsync(e => e.Id == id) is Data.Event events)
            {
                db.Event.Remove(events);
                await db.SaveChangesAsync();
                return Results.Ok();
            }

            return Results.NotFound(new { Event = id });
        })
        .WithTags("Event")
        .WithName("DeleteEvent");
    }
}
