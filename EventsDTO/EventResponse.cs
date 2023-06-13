namespace EventsDTO;

public class EventResponse : Event
{
    public ICollection<Guest> Guests { get; set; } = new List<Guest>();
    public ICollection<Organization> Organizations { get; set; } = new List<Organization>();
}
