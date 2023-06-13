using Microsoft.EntityFrameworkCore;
using EventsAPI.Data;
using EventsDTO;

namespace EventsAPI.Endpoints;

public static class AttendeeEndpoints
{
    public static void MapAttendeeEndpoints (this IEndpointRouteBuilder routes)
    {
        // Get for each attendee including many-to-many
        routes.MapGet("/api/Attendee/{username}", async (string username, ApplicationDbContext db) =>
        {
            return await db.Attendee
                        .Include(a => a.EventAttendees)
                        .ThenInclude(ea=> ea.Event)
                        .Include(a => a.TalkAttendees)
                        .ThenInclude(ta => ta.Talk)
                        .SingleOrDefaultAsync(a => a.UserName == username)
            is Data.Attendee model
                ? Results.Ok(model.MapAttendeeResponse())
                : Results.NotFound();
        })
        .WithTags("Attendee")
        .WithName("GetAttendee")
        .Produces<AttendeeResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        // Create
        routes.MapPost("/api/Attendee/", async (EventsDTO.Attendee input, ApplicationDbContext db) =>
        {
            // Check if Attendee (username or email) already exist
            var existingAttendee = await db.Attendee
                        .Where(a => a.UserName == input.UserName ||
                                    a.EmailAddress == input.EmailAddress)
                        .FirstOrDefaultAsync();

            if (existingAttendee == null)
            {
                var attendee = new Data.Attendee
                {
                    Id = input.Id,
                    FirstName = input.FirstName,
                    LastName = input.LastName,
                    UserName = input.UserName,
                    EmailAddress = input.EmailAddress,
                    PhoneNumber = input.PhoneNumber
                };

                db.Attendee.Add(attendee);
                await db.SaveChangesAsync();

                return Results.Created($"/api/Attendee/{attendee.Id}", attendee);
            }
            else
            {
                return Results.Conflict();
            }

        })
        .WithTags("Attendee")
        .WithName("CreateAttendee")
        .Produces<AttendeeResponse>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status409Conflict);

        // Update
        routes.MapPut("/api/Attendee/{id}", async (int id, EventsDTO.Attendee input, ApplicationDbContext db) =>
        {
            // Check if exists
            var attendee = await db.Attendee.SingleOrDefaultAsync(a => a.Id == id);

            if (attendee is null)
            {
                return Results.NotFound();
            }

            // Check if username and email are duplicated
            var duplicateData = await db.Attendee
                        .Where(a => a.UserName == input.UserName ||
                                    a.EmailAddress == input.EmailAddress)
                        .ToListAsync();

            if (duplicateData.Count() == 1)
            {
                attendee.FirstName = input.FirstName ?? attendee.FirstName;
                attendee.LastName = input.LastName ?? attendee.LastName;
                attendee.UserName = input.UserName ?? attendee.UserName;
                attendee.EmailAddress = input.EmailAddress ?? attendee.EmailAddress;
                attendee.PhoneNumber = input.PhoneNumber ?? attendee.PhoneNumber;

                await db.SaveChangesAsync();

                return Results.NoContent();
            }
            else
            {
                return Results.Conflict();
            }

        })
        .WithTags("Attendee")
        .WithName("UpdateAttendee")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status409Conflict);

        // Delete
        routes.MapDelete("/api/Attendee/{id}", async (int id, ApplicationDbContext db) =>
        {
            // Check if exist
            if (await db.Attendee.SingleOrDefaultAsync(a => a.Id == id) is Data.Attendee attendee)
            {
                db.Attendee.Remove(attendee);
                await db.SaveChangesAsync();
                return Results.Ok();
            }

            return Results.NotFound();
        })
        .WithTags("Attendee")
        .WithName("DeleteAttendee")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
