namespace EventsDTO;

public class AttendeeResponse : Attendee
{
    public ICollection<Talk> Talks { get; set; } = new List<Talk>();
}
