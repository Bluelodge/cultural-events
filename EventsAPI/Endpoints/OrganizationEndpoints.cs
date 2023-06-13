﻿using Microsoft.EntityFrameworkCore;
using EventsAPI.Data;
using EventsDTO;

namespace EventsAPI.Endpoints;

public static class OrganizationEndpoints
{
    public static void MapOrganizationEndpoints (this IEndpointRouteBuilder routes)
    {
        // Get all including many-to-many
        routes.MapGet("/api/Organization", async (ApplicationDbContext db) =>
        {
            return await db.Organization.AsNoTracking()
                        .Include(o => o.EventOrgs)
                        .ThenInclude(eo => eo.Event)
                        .Include(o => o.TalkOrgs)
                        .ThenInclude(to => to.Talk)
                        .Select(m => m.MapOrganizationResponse())
                        .ToListAsync()
            is List<OrganizationResponse> model
                ? Results.Ok(model)
                : Results.NotFound();
        })
        .WithTags("Organization")
        .WithName("GetAllOrganizations")
        .Produces<List<OrganizationResponse>>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        // Get by id including many-to-many
        routes.MapGet("/api/Organization/{id}", async (int id, ApplicationDbContext db) =>
        {
            return await db.Organization.AsNoTracking()
                        .Include(o => o.EventOrgs)
                        .ThenInclude(eo => eo.Event)
                        .Include(o => o.TalkOrgs)
                        .ThenInclude(to => to.Talk)
                        .SingleOrDefaultAsync(o => o.Id == id)
            is Data.Organization model
                ? Results.Ok(model.MapOrganizationResponse())
                : Results.NotFound();
        })
        .WithTags("Organization")
        .WithName("GetOrganizationById")
        .Produces<OrganizationResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

        // Create
        routes.MapPost("/api/Organization/", async (EventsDTO.Organization input, ApplicationDbContext db) =>
        {
            // Check if Organization (corporatename) already exists
            var existingOrg = await db.Organization
                        .Where(o => o.CorporateName == input.CorporateName)
                        .FirstOrDefaultAsync();

            if (existingOrg == null)
            {
                var organization = new Data.Organization
                {
                    Id = input.Id,
                    Name = input.Name,
                    CorporateName = input.CorporateName,
                    WebSite = input.WebSite
                };

                db.Organization.Add(organization);
                await db.SaveChangesAsync();

                return Results.Created($"/api/Organizations/{organization.Id}", organization.MapOrganizationResponse());
            }
            else
            {
                return Results.Conflict();
            }
        })
        .WithTags("Organization")
        .WithName("CreateOrganization")
        .Produces<OrganizationResponse>(StatusCodes.Status201Created)
        .Produces(StatusCodes.Status409Conflict);

        // Update
        routes.MapPut("/api/Organization/{id}", async (int id, EventsDTO.Organization input, ApplicationDbContext db) =>
        {
            // Check if exist
            var organization = await db.Organization.SingleOrDefaultAsync(o => o.Id == id);

            if (organization is null)
            {
                return Results.NotFound();
            }

            // Check if is duplicated when changing corporatename
            var duplciateOrg = await db.Organization
                        .Where(o => o.CorporateName == input.CorporateName)
                        .ToListAsync();

            if (duplciateOrg.Count == 1)
            {
                organization.Name = input.Name ?? organization.Name;
                organization.CorporateName = input.CorporateName ?? organization.CorporateName;
                organization.WebSite = input.WebSite ?? organization.WebSite;

                await db.SaveChangesAsync();

                return Results.NoContent();
            }
            else
            {
                return Results.Conflict();
            }
        })
        .WithTags("Organization")
        .WithName("UpdateOrganization")
        .Produces(StatusCodes.Status204NoContent)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status409Conflict);

        // Delete
        routes.MapDelete("/api/Organization/{id}", async (int id, ApplicationDbContext db) =>
        {
            // Check if exist
            if (await db.Organization.SingleOrDefaultAsync(o => o.Id == id) is Data.Organization organization)
            {
                db.Organization.Remove(organization);
                await db.SaveChangesAsync();

                return Results.Ok();
            }

            return Results.NotFound();
        })
        .WithTags("Organization")
        .WithName("DeleteOrganization")
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
    }
}