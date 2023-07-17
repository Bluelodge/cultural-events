using Microsoft.EntityFrameworkCore;
using EventsAPI.Data;
using EventsAPI.ResponseExamples;
using EventsDTO;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.Annotations;

namespace EventsAPI.Endpoints;

public static class TalkEndpoints
{
    public static void MapTalkEndpoints (this IEndpointRouteBuilder routes)
    {
        // Get all including many-to-many
        routes.MapGet("/api/Talks",
            [SwaggerOperation(
                Summary = "Get Talks",
                Description = "Returns all Talks"
            )]
            [SwaggerResponse(200, "Talks successfully returned")]
            [SwaggerResponse(404, "Talks don't exist")]
            [SwaggerResponseExample(200, typeof(TalkExample.TalkResponse))]
        async (ApplicationDbContext db) =>
        {
            return await db.Talk
                        .AsNoTracking()
                        .Include(t => t.Category)
                        .Include(t => t.Event)
                        .Include(t => t.TalkGuests)
                        .ThenInclude(tg => tg.Guest)
                        .Include(t => t.TalkOrgs)
                        .ThenInclude(to => to.Organization)
                        .Select(t => t.MapTalkResponse())
                        .ToListAsync()
            is List<TalkResponse> model && model.Count != 0
                ? Results.Ok(model)
                : Results.NotFound();
        })
        .WithTags("Talk")
        .WithName("GetAllTalks")
        .Produces<List<TalkResponse>>(StatusCodes.Status200OK);

        // Get by id including many-to-many
        routes.MapGet("/api/Talks/{id}",
            [SwaggerOperation(
                Summary = "Get Talk by id",
                Description = "Returns a Talk as per id"
            )]
            [SwaggerResponse(200, "Talk successfully returned")]
            [SwaggerResponse(404, "Talk doesn't exist")]
            [SwaggerResponseExample(200, typeof(TalkExample.TalkResponse))]
        async (int id, ApplicationDbContext db) =>
        {
            return await db.Talk
                        .AsNoTracking()
                        .Include(t => t.Category)
                        .Include(t => t.Event)
                        .Include(t => t.TalkGuests)
                        .ThenInclude(tg => tg.Guest)
                        .Include(t => t.TalkOrgs)
                        .ThenInclude(to => to.Organization)
                        .SingleOrDefaultAsync(t => t.Id == id)
            is Data.Talk model
                ? Results.Ok(model.MapTalkResponse())
                : Results.NotFound(new { Talk = id });
        })
        .WithTags("Talk")
        .WithName("GetTalkById")
        .Produces<TalkResponse>(StatusCodes.Status200OK);

        // Create
        routes.MapPost("/api/Talks/",
            [SwaggerOperation(
                Summary = "Create single Talk",
                Description = "Adds new Talk with unique Title and EventId"
            )]
            [SwaggerResponse(201, "Talk successfully created")]
            [SwaggerResponse(409, "Can't create Talk due to conflicts with unique key")]
            [SwaggerResponseExample(201, typeof(TalkExample.Talk))]
        async (EventsDTO.Talk input, ApplicationDbContext db) =>
        {
            // Check if exist and belongs to same Event
            var existingTalk = await db.Talk
                        .Where(t => t.Title == input.Title &&
                                    t.EventId == input.EventId)
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
                return Results.Conflict(new { Error = $"Talk with title '{input.Title}' already exists in event '{input.EventId}'" });
            }
        })
        .WithTags("Talk")
        .WithName("CreateTalk")
        .Produces<TalkResponse>(StatusCodes.Status201Created);

        // Update
        routes.MapPut("/api/Talks/{id}",
            [SwaggerOperation(
                Summary = "Update single Talk by Id",
                Description = "Updates Talk info as per id"
            )]
            [SwaggerResponse(204, "Talk successfully updated")]
            [SwaggerResponse(404, "Talk doesn't exist")]
            [SwaggerResponse(409, "Can't update Talk due to conflicts with unique key")]
        async (int id, EventsDTO.Talk input, ApplicationDbContext db) =>
        {
            // Check if exist
            var talk = await db.Talk.SingleOrDefaultAsync(t => t.Id == id);

            if (talk is null)
            {
                return Results.NotFound(new { Talk = id });
            }

            // Check if Title and EventId are duplicates (composite key)
            var title = input.Title ?? talk.Title;
            var evt = input.EventId ?? talk.EventId;

            // Check if Title is duplicated inside current Event ignoring own id
            var duplicatedTalk = await db.Talk
                        .Where(t => t.Title == title &&
                                    t.EventId == evt &&
                                    t.Id != id)
                        .FirstOrDefaultAsync();

            if (duplicatedTalk == null)
            {
                talk.Title = title;
                talk.Summarize = input.Summarize ?? talk.Summarize;
                talk.StartTime = input.StartTime ?? talk.StartTime;
                talk.EndTime = input.EndTime ?? talk.EndTime;
                talk.CategoryId = input.CategoryId ?? talk.CategoryId;
                talk.EventId = evt;

                await db.SaveChangesAsync();

                return Results.NoContent();
            }
            else
            {
                return Results.Conflict(new { Error = $"Another Talk already has the title '{title}' in event '{evt}'" });
            }
        })
        .WithTags("Talk")
        .WithName("UpdateTalk");

        // Delete
        routes.MapDelete("/api/Talks/{id}",
            [SwaggerOperation(
                Summary = "Remove Talk by Id",
                Description = "Deletes Talk as per id"
            )]
            [SwaggerResponse(200, "Talk successfully deleted")]
            [SwaggerResponse(404, "Talk doesn't exist")]
        async (int id, ApplicationDbContext db) =>
        {
            // Check if exist
            if (await db.Talk.SingleOrDefaultAsync(t => t.Id == id) is Data.Talk talk)
            {
                db.Talk.Remove(talk);
                await db.SaveChangesAsync();
                return Results.Ok();
            }

            return Results.NotFound(new { Talk = id });
        })
        .WithTags("Talk")
        .WithName("DeleteTalk");

        // Update many-to-many with Guest
        routes.MapPut("api/Talks/{id}/Guests",
            [SwaggerOperation(
                Summary = "Update relation Talk-Guest by Id",
                Description = "Updates Talk-Guest relations (creates new and deletes existing) as per Talk's Id and a list of Guest's Id"
            )]
            [SwaggerResponse(200, "Request recieved, no changes applied")]
            [SwaggerResponse(204, "Relations successfully updated")]
            [SwaggerResponse(404, "Talk or Guest don't exist")]
            [SwaggerResponse(409, "Can't update relations due to conflicts with unique key")]
        async (int id, List<int> inputGuestIds, ApplicationDbContext db) =>
        {
            // Check if Talk exist
            var talk = await db.Talk
                        .AsQueryable()
                        .Include(t => t.TalkGuests)
                        .SingleOrDefaultAsync(t => t.Id == id);

            if (talk == null)
            {
                return Results.NotFound(new { Talk = id });
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
                    return Results.NotFound(new { Guest = gst });
                }
            }

            // Check for Guest duplicates
            if (inputGuestIds.Count != inputGuestIds.Distinct().Count())
            {
                return Results.Conflict(new { Error = "Guest ids can't be duplicated" });
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
        .WithName("UpdateGuestsInTalks");

        // Update many-to-many with Organization
        routes.MapPut("api/Talks/{id}/Organizations",
            [SwaggerOperation(
                Summary = "Update relation Talk-Organization by Id",
                Description = "Updates Talk-Organization relations (creates new and deletes existing) as per Talk's Id and a list of Organization's Id"
            )]
            [SwaggerResponse(200, "Request recieved, no changes applied")]
            [SwaggerResponse(204, "Relations successfully updated")]
            [SwaggerResponse(404, "Talk or Organization don't exist")]
            [SwaggerResponse(409, "Can't update relations due to conflicts with unique key")]
        async (int id, List<int> inputOrgIds, ApplicationDbContext db) =>
        {
            // Check if Talk exist
            var talk = await db.Talk
                        .AsQueryable()
                        .Include(t => t.TalkOrgs)
                        .SingleOrDefaultAsync(t => t.Id == id);

            if (talk == null)
            {
                return Results.NotFound(new { Talk = id });
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
                    return Results.NotFound(new { Organization = org });
                }
            }

            // Check for duplicates
            if (inputOrgIds.Count != inputOrgIds.Distinct().Count())
            {
                return Results.Conflict(new { Error = "Organization ids can't be duplicated" });
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
        .WithName("UpdateOrganizationsInTalks");
    }
}
