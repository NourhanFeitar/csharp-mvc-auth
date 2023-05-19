using Microsoft.EntityFrameworkCore;
namespace MVC_Lab_7.Models
{
    public class ITIDbContext : DbContext
    {
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<StudentCourse> StudentCourses { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=MSI;Database=MVC_Lab7;Trusted_Connection=True");
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StudentCourse>()
                .HasKey(x => new { x.StudentId, x.CrsId });

            modelBuilder.Entity<Course>()
                .HasKey(a => a.Crs_Id);

            modelBuilder.Entity<Course>()
                .Property(a => a.Crs_Name)
                .IsRequired()
                .HasMaxLength(20);


            base.OnModelCreating(modelBuilder);
        }
    }
}
