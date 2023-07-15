namespace EventsAPI.Data;

public class Guest : EventsDTO.Guest
{
    // References Many-to-Many
    public virtual ICollection<TalkGuest> TalkGuests { get; set; } = null!;
}
