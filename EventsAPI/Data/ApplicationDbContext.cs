using Microsoft.EntityFrameworkCore;

namespace EventsAPI.Data;


public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Create on unique User name
        modelBuilder.Entity<Attendee>().HasIndex(a => a.UserName).IsUnique();

        // Create on unique Corporate name
        modelBuilder.Entity<Organization>().HasIndex(o => o.CorporateName).IsUnique();

        // Many-to-Many
        modelBuilder.Entity<EventAttendee>().HasKey(ea => new { ea.EventId, ea.AttendeeId });
        modelBuilder.Entity<EventGuest>().HasKey(eh => new { eh.EventId, eh.GuestId });
        modelBuilder.Entity<EventOrg>().HasKey(eo => new { eo.EventId, eo.OrganizationId });
        
        modelBuilder.Entity<TalkAttendee>().HasKey(ea => new { ea.TalkId, ea.AttendeeId });
        modelBuilder.Entity<TalkGuest>().HasKey(eh => new { eh.TalkId, eh.GuestId });
        modelBuilder.Entity<TalkOrg>().HasKey(eo => new { eo.TalkId, eo.OrganizationId });
    }

    public DbSet<Attendee> Attendee => Set<Attendee>();
    public DbSet<Category> Category => Set<Category>();
    public DbSet<Event> Event => Set<Event>();
    public DbSet<Guest> Host => Set<Guest>();
    public DbSet<Organization> Organization => Set<Organization>();
    public DbSet<Talk> Talk => Set<Talk>();
}
