using Microsoft.EntityFrameworkCore;
using EventsAPI.Data;
namespace EventsAPI.Endpoints;

public static class EventEndpoints
{
    public static void MapEventEndpoints (this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/Event", async (ApplicationDbContext db) =>
        {
            return await db.Event.ToListAsync();
        })
        .WithTags("Event")
        .WithName("GetAllEvents")
        .Produces<List<Event>>(StatusCodes.Status200OK);

        routes.MapGet("/api/Event/{id}", async (int Id, ApplicationDbContext db) =>
        {
            return await db.Event.FindAsync(Id)
                is Event model
                    ? Results.Ok(model)
                    : Results.NotFound();
        })
        .WithTags("Event")
        .WithName("GetEventById")
        .Produces<Event>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        routes.MapPut("/api/Event/{id}", async (int Id, Event @event, ApplicationDbContext db) =>
        {
            var foundModel = await db.Event.FindAsync(Id);

            if (foundModel is null)
            {
                return Results.NotFound();
            }

            db.Update(@event);

            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithTags("Event")
        .WithName("UpdateEvent")
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status204NoContent);

        routes.MapPost("/api/Event/", async (Event @event, ApplicationDbContext db) =>
        {
            db.Event.Add(@event);
            await db.SaveChangesAsync();
            return Results.Created($"/Events/{@event.Id}", @event);
        })
        .WithTags("Event")
        .WithName("CreateEvent")
        .Produces<Event>(StatusCodes.Status201Created);

        routes.MapDelete("/api/Event/{id}", async (int Id, ApplicationDbContext db) =>
        {
            if (await db.Event.FindAsync(Id) is Event @event)
            {
                db.Event.Remove(@event);
                await db.SaveChangesAsync();
                return Results.Ok(@event);
            }

            return Results.NotFound();
        })
        .WithTags("Event")
        .WithName("DeleteEvent")
        .Produces<Event>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
