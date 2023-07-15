﻿// <auto-generated />
using System;
using EventsAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EventsAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("EventsAPI.Data.Attendee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("UserName", "EmailAddress")
                        .IsUnique();

                    b.ToTable("Attendee");
                });

            modelBuilder.Entity("EventsAPI.Data.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Category");
                });

            modelBuilder.Entity("EventsAPI.Data.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("Title")
                        .IsUnique();

                    b.ToTable("Event");
                });

            modelBuilder.Entity("EventsAPI.Data.EventAttendee", b =>
                {
                    b.Property<int>("EventId")
                        .HasColumnType("int");

                    b.Property<int>("AttendeeId")
                        .HasColumnType("int");

                    b.HasKey("EventId", "AttendeeId");

                    b.HasIndex("AttendeeId");

                    b.ToTable("EventAttendee");
                });

            modelBuilder.Entity("EventsAPI.Data.EventGuest", b =>
                {
                    b.Property<int>("EventId")
                        .HasColumnType("int");

                    b.Property<int>("GuestId")
                        .HasColumnType("int");

                    b.HasKey("EventId", "GuestId");

                    b.HasIndex("GuestId");

                    b.ToTable("EventGuest");
                });

            modelBuilder.Entity("EventsAPI.Data.EventOrg", b =>
                {
                    b.Property<int>("EventId")
                        .HasColumnType("int");

                    b.Property<int>("OrganizationId")
                        .HasColumnType("int");

                    b.HasKey("EventId", "OrganizationId");

                    b.HasIndex("OrganizationId");

                    b.ToTable("EventOrg");
                });

            modelBuilder.Entity("EventsAPI.Data.Guest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Bio")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar(400)");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar(400)");

                    b.Property<string>("Social")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("WebSite")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.HasKey("Id");

                    b.HasIndex("FullName", "Position")
                        .IsUnique();

                    b.ToTable("Guest");
                });

            modelBuilder.Entity("EventsAPI.Data.Organization", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("CorporateName")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("WebSite")
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.HasKey("Id");

                    b.HasIndex("CorporateName")
                        .IsUnique();

                    b.ToTable("Organization");
                });

            modelBuilder.Entity("EventsAPI.Data.Talk", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("EndTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.Property<DateTimeOffset?>("StartTime")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Summarize")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("EventId");

                    b.HasIndex("Title", "EventId")
                        .IsUnique();

                    b.ToTable("Talk");
                });

            modelBuilder.Entity("EventsAPI.Data.TalkAttendee", b =>
                {
                    b.Property<int>("TalkId")
                        .HasColumnType("int");

                    b.Property<int>("AttendeeId")
                        .HasColumnType("int");

                    b.HasKey("TalkId", "AttendeeId");

                    b.HasIndex("AttendeeId");

                    b.ToTable("TalkAttendee");
                });

            modelBuilder.Entity("EventsAPI.Data.TalkGuest", b =>
                {
                    b.Property<int>("TalkId")
                        .HasColumnType("int");

                    b.Property<int>("GuestId")
                        .HasColumnType("int");

                    b.HasKey("TalkId", "GuestId");

                    b.HasIndex("GuestId");

                    b.ToTable("TalkGuest");
                });

            modelBuilder.Entity("EventsAPI.Data.TalkOrg", b =>
                {
                    b.Property<int>("TalkId")
                        .HasColumnType("int");

                    b.Property<int>("OrganizationId")
                        .HasColumnType("int");

                    b.HasKey("TalkId", "OrganizationId");

                    b.HasIndex("OrganizationId");

                    b.ToTable("TalkOrg");
                });

            modelBuilder.Entity("EventsAPI.Data.EventAttendee", b =>
                {
                    b.HasOne("EventsAPI.Data.Attendee", "Attendee")
                        .WithMany("EventAttendees")
                        .HasForeignKey("AttendeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EventsAPI.Data.Event", "Event")
                        .WithMany("EventAttendees")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Attendee");

                    b.Navigation("Event");
                });

            modelBuilder.Entity("EventsAPI.Data.EventGuest", b =>
                {
                    b.HasOne("EventsAPI.Data.Event", "Event")
                        .WithMany("EventGuests")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EventsAPI.Data.Guest", "Guest")
                        .WithMany("EventGuests")
                        .HasForeignKey("GuestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("Guest");
                });

            modelBuilder.Entity("EventsAPI.Data.EventOrg", b =>
                {
                    b.HasOne("EventsAPI.Data.Event", "Event")
                        .WithMany("EventOrgs")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EventsAPI.Data.Organization", "Organization")
                        .WithMany("EventOrgs")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("Organization");
                });

            modelBuilder.Entity("EventsAPI.Data.Talk", b =>
                {
                    b.HasOne("EventsAPI.Data.Category", "Category")
                        .WithMany("Talks")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("FK_Talk_Category_CategoryId");

                    b.HasOne("EventsAPI.Data.Event", "Event")
                        .WithMany("Talks")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_Talk_Event_EventId");

                    b.Navigation("Category");

                    b.Navigation("Event");
                });

            modelBuilder.Entity("EventsAPI.Data.TalkAttendee", b =>
                {
                    b.HasOne("EventsAPI.Data.Attendee", "Attendee")
                        .WithMany("TalkAttendees")
                        .HasForeignKey("AttendeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EventsAPI.Data.Talk", "Talk")
                        .WithMany("TalkAttendees")
                        .HasForeignKey("TalkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Attendee");

                    b.Navigation("Talk");
                });

            modelBuilder.Entity("EventsAPI.Data.TalkGuest", b =>
                {
                    b.HasOne("EventsAPI.Data.Guest", "Guest")
                        .WithMany("TalkGuests")
                        .HasForeignKey("GuestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EventsAPI.Data.Talk", "Talk")
                        .WithMany("TalkGuests")
                        .HasForeignKey("TalkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Guest");

                    b.Navigation("Talk");
                });

            modelBuilder.Entity("EventsAPI.Data.TalkOrg", b =>
                {
                    b.HasOne("EventsAPI.Data.Organization", "Organization")
                        .WithMany("TalkOrgs")
                        .HasForeignKey("OrganizationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("EventsAPI.Data.Talk", "Talk")
                        .WithMany("TalkOrgs")
                        .HasForeignKey("TalkId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Organization");

                    b.Navigation("Talk");
                });

            modelBuilder.Entity("EventsAPI.Data.Attendee", b =>
                {
                    b.Navigation("EventAttendees");

                    b.Navigation("TalkAttendees");
                });

            modelBuilder.Entity("EventsAPI.Data.Category", b =>
                {
                    b.Navigation("Talks");
                });

            modelBuilder.Entity("EventsAPI.Data.Event", b =>
                {
                    b.Navigation("EventAttendees");

                    b.Navigation("EventGuests");

                    b.Navigation("EventOrgs");

                    b.Navigation("Talks");
                });

            modelBuilder.Entity("EventsAPI.Data.Guest", b =>
                {
                    b.Navigation("EventGuests");

                    b.Navigation("TalkGuests");
                });

            modelBuilder.Entity("EventsAPI.Data.Organization", b =>
                {
                    b.Navigation("EventOrgs");

                    b.Navigation("TalkOrgs");
                });

            modelBuilder.Entity("EventsAPI.Data.Talk", b =>
                {
                    b.Navigation("TalkAttendees");

                    b.Navigation("TalkGuests");

                    b.Navigation("TalkOrgs");
                });
#pragma warning restore 612, 618
        }
    }
}
