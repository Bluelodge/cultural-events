namespace EventsAPI.Data;

// Class to join entity - Many-to-Many
public class EventGuest
{
    // Foreign Keys
    public int EventId { get; set; }
    public int GuestId { get; set; }

    // References
    public Event Event { get; set; } = null!;
    public Guest Guest { get; set; } = null!;
}
