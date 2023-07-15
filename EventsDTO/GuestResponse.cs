namespace EventsDTO;

public class GuestResponse : Guest
{
    public ICollection<Talk> Talks { get; set; } = new List<Talk>();
}
