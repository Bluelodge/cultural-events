namespace EventsAPI.Data;

public class Event : EventsDTO.Event
{
    // One-to-Many (Parent - Collection navigation)
    public virtual ICollection<Talk> Talks { get; set; } = null!;
}
