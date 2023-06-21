namespace EventsDTO;

public class EventResponse : Event
{
    // Many-to-Many
    public ICollection<Guest> Guests { get; set; } = new List<Guest>();
    public ICollection<Organization> Organizations { get; set; } = new List<Organization>();

    // One-to-Many
    public ICollection<Talk> Talks { get; set; } = new List<Talk>();
}
