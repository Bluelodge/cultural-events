namespace EventsDTO;

public class CategoryResponse : Category
{
    // One-to-Many
    public ICollection<Talk> Talks { get; set; } = new List<Talk>();
}
