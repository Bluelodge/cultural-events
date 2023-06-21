namespace EventsAPI.Data;

public class Event : EventsDTO.Event
{
    // References Many-to-Many
    public virtual ICollection<EventAttendee> EventAttendees { get; set; } = null!;
    public virtual ICollection<EventGuest> EventGuests { get; set; } = null!;
    public virtual ICollection<EventOrg> EventOrgs { get; set; } = null!;

    // One-to-Many (Parent - Collection navigation)
    public virtual ICollection<Talk> Talks { get; set; } = null!;
}
