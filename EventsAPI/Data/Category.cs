namespace EventsAPI.Data;

public class Category : EventsDTO.Category
{
    // One-to-Many (Parent - Collection navigation)
    public virtual ICollection<Talk> Talks { get; set; } = null!;
}
