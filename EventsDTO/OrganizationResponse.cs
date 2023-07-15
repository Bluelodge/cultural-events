namespace EventsDTO;

public class OrganizationResponse : Organization
{
    public ICollection<Talk> Talks { get; set; } = new List<Talk>();
}
