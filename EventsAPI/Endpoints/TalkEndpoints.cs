using Microsoft.EntityFrameworkCore;
using EventsAPI.Data;
using EventsDTO;

namespace EventsAPI.Endpoints;

public static class TalkEndpoints
{
    public static void MapTalkEndpoints (this IEndpointRouteBuilder routes)
    {
        // Get all including many-to-many
        routes.MapGet("/api/Talk", async (ApplicationDbContext db) =>
        {
            return await db.Talk.AsNoTracking()
                        .Include(t => t.Category)
                        .Include(t => t.Event)
                        .Include(t => t.TalkGuests)
                        .ThenInclude(tg => tg.Guest)
                        .Include(t => t.TalkOrgs)
                        .ThenInclude(to => to.Organization)
                        .Select(t => t.MapTalkResponse())
                        .ToListAsync()
            is List<TalkResponse> model
                ? Results.Ok(model)
                : Results.NotFound();
        })
        .WithTags("Talk")
        .WithName("GetAllTalks")
        .Produces<List<TalkResponse>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        // Get by id including many-to-many
        routes.MapGet("/api/Talk/{id}", async (int id, ApplicationDbContext db) =>
        {
            return await db.Talk.AsNoTracking()
                        .Include(t => t.Category)
                        .Include(t => t.Event)
                        .Include(t => t.TalkGuests)
                        .ThenInclude(tg => tg.Guest)
                        .Include(t => t.TalkOrgs)
                        .ThenInclude(to => to.Organization)
                        .SingleOrDefaultAsync(t => t.Id == id)
            is Data.Talk model
                ? Results.Ok(model.MapTalkResponse())
                : Results.NotFound();
        })
        .WithTags("Talk")
        .WithName("GetTalkById")
        .Produces<TalkResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        // Create
        routes.MapPost("/api/Talk/", async (EventsDTO.Talk input, ApplicationDbContext db) =>
        {
            // Check if exist
            var existingTalk = await db.Talk
                        .Where(t => t.Title == input.Title)
                        .FirstOrDefaultAsync();

            if (existingTalk == null)
            {
                var talk = new Data.Talk
                {
                    Title = input.Title,
                    Summarize = input.Summarize,
                    StartTime = input.StartTime,
                    EndTime = input.EndTime,
                    CategoryId = input.CategoryId,
                    EventId = input.EventId
                };

                db.Talk.Add(talk);
                await db.SaveChangesAsync();

                return Results.Created($"/api/Talk/{talk.Id}", talk.MapTalkResponse());
            }
            else
            {
                return Results.Conflict();
            }
        })
        .WithTags("Talk")
        .WithName("CreateTalk")
        .Produces<TalkResponse>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status409Conflict);

        // Update
        routes.MapPut("/api/Talk/{id}", async (int id, EventsDTO.Talk input, ApplicationDbContext db) =>
        {
            // Check if exist
            var talk = await db.Talk.SingleOrDefaultAsync(t => t.Id == id);

            if (talk is null)
            {
                return Results.NotFound();
            }

            // Check if Title is duplicated ignoring own id
            var duplicatedTalk = await db.Talk
                        .Where(t => t.Title == input.Title &&
                                    t.Id == id)
                        .FirstOrDefaultAsync();

            if (duplicatedTalk == null)
            {
                talk.Title = input.Title ?? talk.Title;
                talk.Summarize = input.Summarize ?? talk.Summarize;
                talk.StartTime = input.StartTime ?? talk.StartTime;
                talk.EndTime = input.EndTime ?? talk.EndTime;
                talk.CategoryId = input.CategoryId ?? talk.CategoryId;
                talk.EventId = input.EventId ?? talk.EventId;

                await db.SaveChangesAsync();

                return Results.NoContent();
            }
            else
            {
                return Results.Conflict();
            }
        })
        .WithTags("Talk")
        .WithName("UpdateTalk")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound);

        // Delete
        routes.MapDelete("/api/Talk/{id}", async (int id, ApplicationDbContext db) =>
        {
            // Check if exist
            if (await db.Talk.SingleOrDefaultAsync(t => t.Id == id) is Data.Talk talk)
            {
                db.Talk.Remove(talk);
                await db.SaveChangesAsync();
                return Results.Ok();
            }

            return Results.NotFound();
        })
        .WithTags("Talk")
        .WithName("DeleteTalk")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
