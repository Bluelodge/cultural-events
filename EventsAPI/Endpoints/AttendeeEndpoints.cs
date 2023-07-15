using Microsoft.EntityFrameworkCore;
using EventsAPI.Data;
using EventsDTO;

namespace EventsAPI.Endpoints;

public static class AttendeeEndpoints
{
    public static void MapAttendeeEndpoints (this IEndpointRouteBuilder routes)
    {
        // Get for each attendee including many-to-many
        routes.MapGet("/api/Attendees/{username}", async (string username, ApplicationDbContext db) =>
        {
            return await db.Attendee
                        .Include(a => a.TalkAttendees)
                        .ThenInclude(ta => ta.Talk)
                        .SingleOrDefaultAsync(a => a.UserName == username)
            is Data.Attendee model
                ? Results.Ok(model.MapAttendeeResponse())
                : Results.NotFound(new { Attendee = username });
        })
        .WithTags("Attendee")
        .WithName("GetAttendeeByUsername")
        .Produces<AttendeeResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        // Create
        routes.MapPost("/api/Attendees/", async (EventsDTO.Attendee input, ApplicationDbContext db) =>
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

                return Results.Created($"/api/Attendees/{attendee.Id}", attendee);
            }
            else
            {
                return Results.Conflict(
                    new { Error = $"Attendee with username '{input.UserName}' or email '{input.EmailAddress}' already exists"}
                );
            }

        })
        .WithTags("Attendee")
        .WithName("CreateAttendee")
        .Produces<AttendeeResponse>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status409Conflict);

        // Update
        routes.MapPut("/api/Attendees/{id}", async (int id, EventsDTO.Attendee input, ApplicationDbContext db) =>
        {
            // Check if exists
            var attendee = await db.Attendee.SingleOrDefaultAsync(a => a.Id == id);

            if (attendee is null)
            {
                return Results.NotFound(new { Attendee = id });
            }

            // Check if username and email are duplicated
            var username = input.UserName ?? attendee.UserName;
            var email = input.EmailAddress ?? attendee.EmailAddress;

            // Verify values ignoring own id
            var duplicatedAttendee = await db.Attendee
                        .Where(a => (a.UserName == username ||
                                    a.EmailAddress == email) &&
                                    a.Id != id)
                        .FirstOrDefaultAsync();

            if (duplicatedAttendee == null)
            {
                attendee.FirstName = input.FirstName ?? attendee.FirstName;
                attendee.LastName = input.LastName ?? attendee.LastName;
                attendee.UserName = username;
                attendee.EmailAddress = email;
                attendee.PhoneNumber = input.PhoneNumber ?? attendee.PhoneNumber;

                await db.SaveChangesAsync();

                return Results.NoContent();
            }
            else
            {
                return Results.Conflict(new { Error = $"Another Attendee already has the username '{input.UserName}' or email '{input.EmailAddress}'" });
            }

        })
        .WithTags("Attendee")
        .WithName("UpdateAttendee")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status409Conflict);

        // Delete
        routes.MapDelete("/api/Attendees/{id}", async (int id, ApplicationDbContext db) =>
        {
            // Check if exist
            if (await db.Attendee.SingleOrDefaultAsync(a => a.Id == id) is Data.Attendee attendee)
            {
                db.Attendee.Remove(attendee);
                await db.SaveChangesAsync();
                return Results.Ok();
            }

            return Results.NotFound(new { Attendee = id });
        })
        .WithTags("Attendee")
        .WithName("DeleteAttendee")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        // Get all Talks from Attendee
        routes.MapGet("/api/Attendees/{username}/Talks", async (string username, ApplicationDbContext db) =>
        {
            var talks = await db.Talk
                        .AsNoTracking()
                        .Include(a => a.TalkAttendees)
                        .Where(t => t.TalkAttendees.Any(ta => ta.Attendee.UserName == username))
                        .Select(m => m.MapTalkResponse())
                        .ToListAsync();

            return talks is List<TalkResponse> model
                ? Results.Ok(model)
                : Results.NotFound();
        })
        .WithTags("Attendee")
        .WithName("GetAllTalksFromAttendee")
        .Produces<List<TalkResponse>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        // Add many-to-many with Talk
        routes.MapPost("/api/Attendees/{username}/Talks/{talkId}", async (string username, int talkId, ApplicationDbContext db) =>
        {
            // Check if Attendee exist
            var attendee = await db.Attendee
                            .Include(a => a.TalkAttendees)
                            .SingleOrDefaultAsync(a => a.UserName == username);

            if (attendee == null)
            {
                return Results.NotFound(new { Attendee = username });
            }

            // Check if Talk exist
            var talk = await db.Talk.SingleOrDefaultAsync(t => t.Id == talkId);

            if (talk == null)
            {
                return Results.NotFound(new { Talk = talkId });
            }

            // Check for duplicate
            var talkAttendee = attendee.TalkAttendees.SingleOrDefault(ta => ta.TalkId == talkId);

            // Insert on empty relation
            if (talkAttendee == null)
            {
                attendee.TalkAttendees.Add(new TalkAttendee
                {
                    Talk = talk,
                    Attendee = attendee
                });
            }
            else
            {
                return Results.Conflict(new { Error = $"The relation between Attedee '{username}' and Talk '{talk.Title}' already exists" });
            }

            await db.SaveChangesAsync();

            return Results.Created($"/api/Attendee/{attendee.UserName}", attendee.MapAttendeeResponse());
        })
        .WithTags("Attendee")
        .WithName("AddTalkToAttendee")
        .Produces<AttendeeResponse>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status409Conflict);

        // Delete many-to-many with Talk
        routes.MapDelete("/api/Attendees/{username}/Talks/{talkId}", async (string username, int talkId, ApplicationDbContext db) =>
        {
            // Check if Attendee exists
            var attendee = await db.Attendee
                            .Include(a => a.TalkAttendees)
                            .SingleOrDefaultAsync(a => a.UserName == username);

            if (attendee is Data.Attendee)
            {
                // Check if Talk exist
                if (await db.Talk.SingleOrDefaultAsync(t => t.Id == talkId) is Data.Talk talk)
                {
                    var talkAttendee = attendee.TalkAttendees.SingleOrDefault(ta => ta.TalkId == talkId);

                    // Verify data
                    if (talkAttendee is TalkAttendee)
                    {
                        attendee.TalkAttendees.Remove(talkAttendee);

                        await db.SaveChangesAsync();

                        return Results.Ok();
                    }
                }
            }

            return Results.NotFound();
        })
        .WithTags("Attendee")
        .WithName("RemoveTalkFromAttendee")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

    }
}
