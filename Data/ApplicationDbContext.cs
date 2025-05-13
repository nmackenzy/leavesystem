using Microsoft.EntityFrameworkCore;

namespace simple_api.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<simple_api.Models.LeaveRequest>? LeaveRequests { get; set; }
        public DbSet<simple_api.Models.User>? Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // explicitly configure LeaveRequest's primary key
            modelBuilder.Entity<simple_api.Models.LeaveRequest>()
                .HasKey(lr => lr.RequestId);

            // state relationship
            modelBuilder.Entity<simple_api.Models.LeaveRequest>()
                .HasOne(lr => lr.User)           
                .WithMany()                       
                .HasForeignKey(lr => lr.UserId);
        }

    }
}