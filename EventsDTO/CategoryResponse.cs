namespace EventsDTO;

public class CategoryResponse : Category
{
    public ICollection<Talk> Talks { get; set; } = new List<Talk>();
}
