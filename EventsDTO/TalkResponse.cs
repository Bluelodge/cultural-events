namespace EventsDTO;

public class TalkResponse : Talk
{
    // One-to-Many
    public Category Category { get; set; } = null!;
    public Event Event { get; set; } = null!;

    // Many-to-Many
    public ICollection<Guest> Guests { get; set; } = new List<Guest>();
    public ICollection<Organization> Organizations { get; set; } = new List<Organization>();
}
