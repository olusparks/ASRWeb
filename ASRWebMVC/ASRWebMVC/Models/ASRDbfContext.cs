using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ASRWebMVC.Models
{
    public partial class ASRDbfContext : DbContext
    {
        public ASRDbfContext()
        {
        }

        public ASRDbfContext(DbContextOptions<ASRDbfContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Room> Room { get; set; }
        public virtual DbSet<Slot> Slot { get; set; }
        public virtual DbSet<User> User { get; set; }
		public object Users { get; internal set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-656DMVQ\\SQL2017;initial catalog=ASRDbf;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.1-servicing-10028");

            modelBuilder.Entity<Room>(entity =>
            {
                entity.Property(e => e.RoomId)
                    .HasColumnName("RoomID")
                    .HasMaxLength(10)
                    .ValueGeneratedNever();
            });

            modelBuilder.Entity<Slot>(entity =>
            {
                entity.HasKey(e => new { e.RoomId, e.StartTime });

                entity.Property(e => e.RoomId)
                    .HasColumnName("RoomID")
                    .HasMaxLength(10);

                entity.Property(e => e.BookedInStudentId)
                    .HasColumnName("BookedInStudentID")
                    .HasMaxLength(8);

                entity.Property(e => e.StaffId)
                    .IsRequired()
                    .HasColumnName("StaffID")
                    .HasMaxLength(8);

                entity.HasOne(d => d.BookedInStudent)
                    .WithMany(p => p.SlotBookedInStudent)
                    .HasForeignKey(d => d.BookedInStudentId)
                    .HasConstraintName("FK_Slot_User_Student");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Slot)
                    .HasForeignKey(d => d.RoomId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Slot_Room");

                entity.HasOne(d => d.Staff)
                    .WithMany(p => p.SlotStaff)
                    .HasForeignKey(d => d.StaffId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Slot_User_Staff");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId)
                    .HasColumnName("UserID")
                    .HasMaxLength(8)
                    .ValueGeneratedNever();

                entity.Property(e => e.Email).IsRequired();

                entity.Property(e => e.Name).IsRequired();
            });
        }
    }
}
