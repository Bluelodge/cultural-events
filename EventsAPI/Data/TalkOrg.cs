namespace EventsAPI.Data;

// Class to join entity - Many-to-Many
public class TalkOrg
{
    // Foreign Keys
    public int TalkId { get; set; }
    public int OrganizationId { get; set; }

    // References
    public Talk Talk { get; set; } = null!;
    public Organization Organization { get; set; } = null!;
}
