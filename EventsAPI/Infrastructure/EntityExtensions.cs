namespace EventsAPI.Data;

public static class EntityExtensions
{
    // Attendees
    public static EventsDTO.AttendeeResponse MapAttendeeResponse(this Attendee attendee) =>
        new EventsDTO.AttendeeResponse
        {
            Id = attendee.Id,
            UserName = attendee.UserName,
            FirstName = attendee.FirstName,
            LastName = attendee.LastName,
            EmailAddress = attendee.EmailAddress,
            Events = attendee.EventAttendees?
                .Select(ea => new EventsDTO.Event
                {
                    Id = ea.EventId,
                    Title = ea.Event.Title
                })
                .ToList() ?? new(),
            Talks = attendee.TalkAttendees?
                .Select(ta => new EventsDTO.Talk
                {
                    Id = ta.TalkId,
                    Title = ta.Talk.Title
                })
                .ToList() ?? new()
        };

    // Categories
    public static EventsDTO.CategoryResponse MapCategoryResponse(this Category category) =>
        new EventsDTO.CategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            Talks = category.Talks?
                .Select(c => new EventsDTO.Talk
                {
                    Id = c.Id,
                    Title = c.Title
                })
                .ToList() ?? new()
        };

    // Events
    public static EventsDTO.EventResponse MapEventResponse(this Event events) =>
        new EventsDTO.EventResponse
        {
            Id = events.Id,
            Title = events.Title,
            Guests = events.EventGuests?
                .Select(eg => new EventsDTO.Guest
                {
                    Id = eg.GuestId,
                    FullName = eg.Guest.FullName
                })
                .ToList() ?? new(),
            Organizations = events.EventOrgs?
                .Select(eo => new EventsDTO.Organization
                {
                    Id = eo.OrganizationId,
                    Name = eo.Organization.Name
                })
                .ToList() ?? new(),
            Talks = events.Talks?
                .Select(e => new EventsDTO.Talk
                {
                    Id = e.Id,
                    Title = e.Title
                })
                .ToList() ?? new()
        };

    // Guests
    public static EventsDTO.GuestResponse MapGuestResponse(this Guest guest) =>
        new EventsDTO.GuestResponse
        {
            Id = guest.Id,
            FullName = guest.FullName,
            Position = guest.Position,
            Bio = guest.Bio,
            Social = guest.Social,
            WebSite = guest.WebSite,
            Events = guest.EventGuests?
                .Select(eg => new EventsDTO.Event
                {
                    Id = eg.EventId,
                    Title = eg.Event.Title
                })
                .ToList() ?? new(),
            Talks = guest.TalkGuests?
                .Select(gt => new EventsDTO.Talk
                {
                    Id = gt.TalkId,
                    Title = gt.Talk.Title
                })
                .ToList() ?? new()
        };

    // Oranizations
    public static EventsDTO.OrganizationResponse MapOrganizationResponse(this Organization organization) =>
        new EventsDTO.OrganizationResponse
        {
            Id = organization.Id,
            Name = organization.Name,
            CorporateName = organization.CorporateName,
            WebSite = organization.WebSite,
            Events = organization.EventOrgs?
                .Select(eo => new EventsDTO.Event
                {
                    Id = eo.EventId,
                    Title = eo.Event.Title
                })
                .ToList() ?? new(),
            Talks = organization.TalkOrgs?
                .Select(to => new EventsDTO.Talk
                {
                    Id = to.TalkId,
                    Title = to.Talk.Title
                })
                .ToList() ?? new()
        };

    // Talks
    public static EventsDTO.TalkResponse MapTalkResponse(this Talk talk) =>
        new EventsDTO.TalkResponse
        {
            Id = talk.Id,
            Title = talk.Title,
            Summarize = talk.Summarize,
            StartTime = talk.StartTime,
            EndTime = talk.EndTime,
            CategoryId = talk.CategoryId,
            Category = new EventsDTO.Category
            {
                Id = talk.CategoryId!.GetValueOrDefault(),  // Pass int value to nullable int
                Name = talk.Category?.Name                  // Avoid error on create
            },
            EventId = talk.EventId,
            Event = new EventsDTO.Event
            {
                Id = talk.EventId!.GetValueOrDefault(),     // Pass int value to nullable int
                Title = talk.Event?.Title                   // Avoid error on create
            },
            Guests = talk.TalkGuests?
                .Select(tg => new EventsDTO.Guest
                {
                    Id = tg.GuestId,
                    FullName = tg.Guest.FullName
                })
                .ToList() ?? new(),
            Organizations = talk.TalkOrgs?
                .Select(to => new EventsDTO.Organization
                {
                    Id = to.OrganizationId,
                    Name = to.Organization.Name
                })
                .ToList() ?? new()
        };
}