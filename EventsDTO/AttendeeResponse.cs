namespace EventsDTO;

public class AttendeeResponse : Attendee
{
    public ICollection<Event> Events { get; set; } = new List<Event>();
    public ICollection<Talk> Talks { get; set; } = new List<Talk>();
}
