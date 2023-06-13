namespace EventsDTO;

public class TalkResponse : Talk
{
    public Category Category { get; set; } = null!;
    public ICollection<Guest> Guests { get; set; } = new List<Guest>();
    public ICollection<Organization> Organizations { get; set; } = new List<Organization>();
}
