namespace EventsAPI.Data;

// Class to join entity - Many-to-Many
public class TalkGuest
{
    // Foreign Keys
    public int TalkId { get; set; }
    public int GuestId { get; set; }

    // References
    public Talk Talk { get; set; } = null!;
    public Guest Guest { get; set; } = null!;
}
