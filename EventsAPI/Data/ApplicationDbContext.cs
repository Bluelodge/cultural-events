using Microsoft.EntityFrameworkCore;

namespace EventsAPI.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Create on unique User name and Email
        modelBuilder.Entity<Attendee>().HasIndex(a => new { a.UserName, a.EmailAddress}).IsUnique();

        // Create on unique Name
        modelBuilder.Entity<Category>().HasIndex(c => c.Name).IsUnique();

        // Create on unique Title
        modelBuilder.Entity<Event>().HasIndex(e => e.Title).IsUnique();

        // Create on unique Fullname and Position
        modelBuilder.Entity<Guest>().HasIndex(g => new { g.FullName, g.Position }).IsUnique();

        // Create on unique Corporate name
        modelBuilder.Entity<Organization>().HasIndex(o => o.CorporateName).IsUnique();

        // Create on unique Title and EventId
        modelBuilder.Entity<Talk>().HasIndex(t => new { t.Title, t.EventId }).IsUnique();
        
        // Set null on Category delete / Delete on Event delete
        modelBuilder.Entity<Talk>().HasOne(t => t.Category)
                                    .WithMany(Category => Category.Talks)
                                    .HasForeignKey("CategoryId")
                                    .HasConstraintName("FK_Talk_Category_CategoryId")
                                    .IsRequired(false)
                                    .OnDelete(DeleteBehavior.SetNull);
        modelBuilder.Entity<Talk>().HasOne(t => t.Event)
                                    .WithMany(Event => Event.Talks)
                                    .HasForeignKey("EventId")
                                    .HasConstraintName("FK_Talk_Event_EventId")
                                    .IsRequired(true)
                                    .OnDelete(DeleteBehavior.Cascade);

        // Many-to-Many
        modelBuilder.Entity<EventAttendee>().HasKey(ea => new { ea.EventId, ea.AttendeeId });
        modelBuilder.Entity<EventGuest>().HasKey(eg => new { eg.EventId, eg.GuestId });
        modelBuilder.Entity<EventOrg>().HasKey(eo => new { eo.EventId, eo.OrganizationId });
        
        modelBuilder.Entity<TalkAttendee>().HasKey(ta => new { ta.TalkId, ta.AttendeeId });
        modelBuilder.Entity<TalkGuest>().HasKey(tg => new { tg.TalkId, tg.GuestId });
        modelBuilder.Entity<TalkOrg>().HasKey(to => new { to.TalkId, to.OrganizationId });
    }

    public DbSet<Attendee> Attendee => Set<Attendee>();
    public DbSet<Category> Category => Set<Category>();
    public DbSet<Event> Event => Set<Event>();
    public DbSet<Guest> Guest => Set<Guest>();
    public DbSet<Organization> Organization => Set<Organization>();
    public DbSet<Talk> Talk => Set<Talk>();
}
