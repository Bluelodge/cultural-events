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

        // Update many-to-many with Guest
        routes.MapPut("api/Talk/{id}/Guest", async (int id, List<int> inputGuestIds, ApplicationDbContext db) =>
        {
            // Check if Talk exist
            var talk = await db.Talk
                        .AsQueryable()
                        .Include(t => t.TalkGuests)
                        .SingleOrDefaultAsync(t => t.Id == id);

            if (talk == null)
            {
                return Results.NotFound();
            }

            // Get existing TalkGuest relations
            var existingTalkGuest = talk.TalkGuests.ToList();

            // Check if Guest and relations are empty
            if (inputGuestIds.Count == 0 && existingTalkGuest.Count == 0)
            {
                return Results.Ok();
            }

            // Check if all Guest exist
            foreach (var gst in inputGuestIds)
            {
                var guest = await db.Guest.SingleOrDefaultAsync(g => g.Id == gst);

                if (guest == null)
                {
                    return Results.Conflict();
                }
            }

            // Check for Guest duplicates
            if (inputGuestIds.Count != inputGuestIds.Distinct().Count())
            {
                return Results.Conflict();
            }

            // Insert all Guest inputs on empty relations
            if (existingTalkGuest.Count == 0)
            {
                foreach (var gst in inputGuestIds)
                {
                    talk.TalkGuests.Add(new TalkGuest
                    {
                        TalkId = id,
                        GuestId = gst
                    });
                }

                await db.SaveChangesAsync();

                return Results.NoContent();
            }

            // Check for no relation changes
            inputGuestIds.Sort();
            var existingTgIds = existingTalkGuest.Select(tg => tg.GuestId).ToList();
            existingTgIds.Sort();

            if (existingTgIds.SequenceEqual(inputGuestIds))
            {
                return Results.Ok();
            }

            // Add new relations
            foreach (var gst in inputGuestIds)
            {
                if (existingTalkGuest.Where(tg => tg.GuestId == gst).Count() == 0)
                {
                    talk.TalkGuests.Add(new TalkGuest
                    {
                        TalkId = id,
                        GuestId = gst
                    });
                }
            }

            // Delete non existing relations
            foreach (var gst in existingTalkGuest)
            {
                if (inputGuestIds.Contains(gst.GuestId) == false)
                {
                    talk.TalkGuests.Remove(gst);
                }
            }

            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithTags("Talk")
        .WithName("UpdateTalkGuest")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status409Conflict);

        // Update many-to-many with Organization
        routes.MapPut("api/Talk/{id}/Org", async (int id, List<int> inputOrgIds, ApplicationDbContext db) =>
        {
            // Check if Talk exist
            var talk = await db.Talk
                        .AsQueryable()
                        .Include(t => t.TalkOrgs)
                        .SingleOrDefaultAsync(t => t.Id == id);

            if (talk == null)
            {
                return Results.NotFound();
            }

            // Get existing TalkOrg relations
            var existingTalkOrg = talk.TalkOrgs.ToList();

            // Check if Orgs and relations are empty
            if (inputOrgIds.Count == 0 && existingTalkOrg.Count == 0)
            {
                return Results.Ok();
            }

            // Check if all Orgs exist
            foreach (var org in inputOrgIds)
            {
                var organization = await db.Organization.SingleOrDefaultAsync(o => o.Id == org);

                if (organization == null)
                {
                    return Results.Conflict();
                }
            }

            // Check for duplicates
            if (inputOrgIds.Count != inputOrgIds.Distinct().Count())
            {
                return Results.Conflict();
            }

            // Insert all Organization inputs on empty relations
            if (existingTalkOrg.Count == 0)
            {
                foreach (var org in inputOrgIds)
                {
                    talk.TalkOrgs.Add(new TalkOrg
                    {
                        TalkId = id,
                        OrganizationId = org
                    });
                }

                await db.SaveChangesAsync();

                return Results.NoContent();
            }

            // Check for no relation changes
            inputOrgIds.Sort();
            var existingToIds = existingTalkOrg.Select(to => to.OrganizationId).ToList();
            existingToIds.Sort();

            if (existingToIds.SequenceEqual(inputOrgIds))
            {
                return Results.Ok();
            }

            // Add new relations
            foreach (var org in inputOrgIds)
            {
                if (existingTalkOrg.Where(to => to.OrganizationId == org).Count() == 0)
                {
                    talk.TalkOrgs.Add(new TalkOrg
                    {
                        TalkId = id,
                        OrganizationId = org
                    });
                }
            }

            // Delete non existing relations
            foreach (var tOrg in existingTalkOrg)
            {
                if (inputOrgIds.Contains(tOrg.OrganizationId) == false)
                {
                    talk.TalkOrgs.Remove(tOrg);
                }
            }

            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithTags("Talk")
        .WithName("UpdateTalkOrg")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status409Conflict);
    }
}
