using Microsoft.EntityFrameworkCore;
using UniversityDepartmentAPI.Models;

namespace UniversityDepartmentAPI.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Discipline> Disciplines { get; set; }
        public DbSet<Teaching> Teachings { get; set; }
        public DbSet<ExtraWork> ExtraWorks { get; set; }
        public DbSet<TeacherExtraWork> TeacherExtraWorks { get; set; }
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Teacher>()
                .HasOne(t => t.Classroom)
                .WithMany()
                .HasForeignKey(t => t.ClassroomId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Teaching>()
                .HasOne(t => t.Teacher)
                .WithMany(t => t.Teachings)
                .HasForeignKey(t => t.TeacherId);

            modelBuilder.Entity<Teaching>()
                .HasOne(t => t.Discipline)
                .WithMany(d => d.Teachings)
                .HasForeignKey(t => t.DisciplineId);

            modelBuilder.Entity<TeacherExtraWork>()
                .HasOne(te => te.Teacher)
                .WithMany(t => t.TeacherExtraWorks)
                .HasForeignKey(te => te.TeacherId);

            modelBuilder.Entity<TeacherExtraWork>()
                .HasOne(te => te.ExtraWork)
                .WithMany(e => e.TeacherExtraWorks)
                .HasForeignKey(te => te.ExtraWorkId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.Teacher)
                .WithMany()
                .HasForeignKey(u => u.TeacherId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}