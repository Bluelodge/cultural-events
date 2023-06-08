namespace EventsAPI.Data;

// Class to join entity - Many-to-Many
public class EventOrg
{
    // Foreign Keys
    public int EventId { get; set; }
    public int OrganizationId { get; set; }

    // References
    public Event Event { get; set; } = null!;
    public Organization Organization { get; set; } = null!;
}
