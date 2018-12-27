using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace SchoolProject.Models.SchoolDbContext
{
    public partial class SchoolProjectContext : DbContext
    {
        public SchoolProjectContext()
        {
        }

        public SchoolProjectContext(DbContextOptions<SchoolProjectContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Course> Course { get; set; }
        public virtual DbSet<Enrolled> Enrolled { get; set; }
        public virtual DbSet<Section> Section { get; set; }
        public virtual DbSet<Student> Student { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.;Database=SchoolProject;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>(entity =>
            {
                entity.HasKey(e => e.Cname);

                entity.Property(e => e.Cname)
                    .HasColumnName("cname")
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .ValueGeneratedNever();

                entity.Property(e => e.CourseInfo)
                    .IsRequired()
                    .HasColumnName("courseInfo")
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Enrolled>(entity =>
            {
                entity.HasKey(e => new { e.Snum, e.Cname });

                entity.Property(e => e.Snum).HasColumnName("snum");

                entity.Property(e => e.Cname)
                    .HasColumnName("cname")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.CnameNavigation)
                    .WithMany(p => p.Enrolled)
                    .HasForeignKey(d => d.Cname)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Enrolled_Course");

                entity.HasOne(d => d.SnumNavigation)
                    .WithMany(p => p.Enrolled)
                    .HasForeignKey(d => d.Snum)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Enrolled_Student");
            });

            modelBuilder.Entity<Section>(entity =>
            {
                entity.HasKey(e => new { e.Cname, e.MeetsOn });

                entity.Property(e => e.Cname)
                    .HasColumnName("cname")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MeetsOn).HasColumnName("meets_on");

                entity.Property(e => e.EndsAt)
                    .HasColumnName("ends_at")
                    .HasColumnType("time(0)");

                entity.Property(e => e.MeetsAt)
                    .HasColumnName("meets_at")
                    .HasColumnType("time(0)");

                entity.Property(e => e.Room)
                    .IsRequired()
                    .HasColumnName("room")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.HasOne(d => d.CnameNavigation)
                    .WithMany(p => p.Section)
                    .HasForeignKey(d => d.Cname)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Section_Course");
            });

            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.Snum);

                entity.Property(e => e.Snum)
                    .HasColumnName("snum")
                    .ValueGeneratedNever();

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(16)
                    .IsUnicode(false);

                entity.Property(e => e.Sname)
                    .IsRequired()
                    .HasColumnName("sname")
                    .HasMaxLength(80)
                    .IsUnicode(false);
            });
        }
    }
}
