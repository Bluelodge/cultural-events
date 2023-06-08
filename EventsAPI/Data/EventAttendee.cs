namespace EventsAPI.Data;

// Class to join entity - Many-to-Many
public class EventAttendee
{
    // Foreign Keys
    public int EventId { get; set; }
    public int AttendeeId { get; set; }

    // References
    public Event Event { get; set; } = null!;
    public Attendee Attendee { get; set; } = null!;
}
