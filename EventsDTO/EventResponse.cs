namespace EventsDTO;

public class EventResponse : Event
{
    // One-to-Many
    public ICollection<Talk> Talks { get; set; } = new List<Talk>();
}
