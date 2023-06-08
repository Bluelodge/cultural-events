namespace EventsAPI.Data;

public class Attendee : EventsDTO.Attendee
{
    public virtual ICollection<EventAttendee> EventAttendees { get; set; } = null!;
    public virtual ICollection<TalkAttendee> TalkAttendees { get; set; } = null!;
}
