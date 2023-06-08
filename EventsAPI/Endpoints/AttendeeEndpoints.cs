using Microsoft.EntityFrameworkCore;
using EventsAPI.Data;
namespace EventsAPI.Endpoints;

public static class AttendeeEndpoints
{
    public static void MapAttendeeEndpoints (this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/Attendee", async (ApplicationDbContext db) =>
        {
            return await db.Attendee.ToListAsync();
        })
        .WithTags("Attendee")
        .WithName("GetAllAttendees")
        .Produces<List<Attendee>>(StatusCodes.Status200OK);

        routes.MapGet("/api/Attendee/{id}", async (int Id, ApplicationDbContext db) =>
        {
            return await db.Attendee.FindAsync(Id)
                is Attendee model
                    ? Results.Ok(model)
                    : Results.NotFound();
        })
        .WithTags("Attendee")
        .WithName("GetAttendeeById")
        .Produces<Attendee>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        routes.MapPut("/api/Attendee/{id}", async (int Id, Attendee attendee, ApplicationDbContext db) =>
        {
            var foundModel = await db.Attendee.FindAsync(Id);

            if (foundModel is null)
            {
                return Results.NotFound();
            }

            db.Update(attendee);

            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithTags("Attendee")
        .WithName("UpdateAttendee")
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status204NoContent);

        routes.MapPost("/api/Attendee/", async (Attendee attendee, ApplicationDbContext db) =>
        {
            db.Attendee.Add(attendee);
            await db.SaveChangesAsync();
            return Results.Created($"/Attendees/{attendee.Id}", attendee);
        })
        .WithTags("Attendee")
        .WithName("CreateAttendee")
        .Produces<Attendee>(StatusCodes.Status201Created);

        routes.MapDelete("/api/Attendee/{id}", async (int Id, ApplicationDbContext db) =>
        {
            if (await db.Attendee.FindAsync(Id) is Attendee attendee)
            {
                db.Attendee.Remove(attendee);
                await db.SaveChangesAsync();
                return Results.Ok(attendee);
            }

            return Results.NotFound();
        })
        .WithTags("Attendee")
        .WithName("DeleteAttendee")
        .Produces<Attendee>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
