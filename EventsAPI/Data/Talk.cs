namespace EventsAPI.Data;

public class Talk : EventsDTO.Talk
{
    // References Many-to-Many
    public virtual ICollection<TalkAttendee> TalkAttendees { get; set; } = null!;
    public virtual ICollection<TalkGuest> TalkGuests { get; set; } = null!;
    public virtual ICollection<TalkOrg> TalkOrgs { get; set; } = null!;

    // One-to-Many (Child - Reference navigation)
    public Category Category { get; set; } = null!;
}
