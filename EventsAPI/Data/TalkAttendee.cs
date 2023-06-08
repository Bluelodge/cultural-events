using EventsDTO;

namespace EventsAPI.Data;

// Class to join entity - Many-to-Many
public class TalkAttendee
{
    // Foreign Keys
    public int TalkId { get; set; }
    public int AttendeeId { get; set; }

    // References
    public Talk Talk { get; set; } = null!;
    public Attendee Attendee { get; set; } = null!;
}
