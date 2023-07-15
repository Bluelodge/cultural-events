namespace EventsAPI.Data;

public class Attendee : EventsDTO.Attendee
{
    public virtual ICollection<TalkAttendee> TalkAttendees { get; set; } = null!;
}
