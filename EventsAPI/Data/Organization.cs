namespace EventsAPI.Data;

public class Organization : EventsDTO.Organization
{
    // References Many-to-Many
    public virtual ICollection<TalkOrg> TalkOrgs { get; set; } = null!;
}
