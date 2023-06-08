using Microsoft.EntityFrameworkCore;
using EventsAPI.Data;
namespace EventsAPI.Endpoints;

public static class TalkEndpoints
{
    public static void MapTalkEndpoints (this IEndpointRouteBuilder routes)
    {
        routes.MapGet("/api/Talk", async (ApplicationDbContext db) =>
        {
            return await db.Talk.ToListAsync();
        })
        .WithTags("Talk")
        .WithName("GetAllTalks")
        .Produces<List<Talk>>(StatusCodes.Status200OK);

        routes.MapGet("/api/Talk/{id}", async (int Id, ApplicationDbContext db) =>
        {
            return await db.Talk.FindAsync(Id)
                is Talk model
                    ? Results.Ok(model)
                    : Results.NotFound();
        })
        .WithTags("Talk")
        .WithName("GetTalkById")
        .Produces<Talk>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        routes.MapPut("/api/Talk/{id}", async (int Id, Talk talk, ApplicationDbContext db) =>
        {
            var foundModel = await db.Talk.FindAsync(Id);

            if (foundModel is null)
            {
                return Results.NotFound();
            }

            db.Update(talk);

            await db.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithTags("Talk")
        .WithName("UpdateTalk")
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status204NoContent);

        routes.MapPost("/api/Talk/", async (Talk talk, ApplicationDbContext db) =>
        {
            db.Talk.Add(talk);
            await db.SaveChangesAsync();
            return Results.Created($"/Talks/{talk.Id}", talk);
        })
        .WithTags("Talk")
        .WithName("CreateTalk")
        .Produces<Talk>(StatusCodes.Status201Created);

        routes.MapDelete("/api/Talk/{id}", async (int Id, ApplicationDbContext db) =>
        {
            if (await db.Talk.FindAsync(Id) is Talk talk)
            {
                db.Talk.Remove(talk);
                await db.SaveChangesAsync();
                return Results.Ok(talk);
            }

            return Results.NotFound();
        })
        .WithTags("Talk")
        .WithName("DeleteTalk")
        .Produces<Talk>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}
