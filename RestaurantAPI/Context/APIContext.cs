using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Context
{
    public class APIContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    "Server=.;Database=RestaurantDB;Trusted_Connection=True;TrustServerCertificate=True;",
                    sqlOptions =>
                    {
                        sqlOptions.CommandTimeout(30);  
                    });
                optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking); 
            }
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Chef> Chefs { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<GalleryImage> GalleryImages { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Testimonial> Testimonials { get; set; }
        public DbSet<CompanyInfo> CompanyInfos  { get; set; }
        public DbSet<RestaurantAPI.Entities.Event>Events  { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<AdminLog> AdminLogs { get; set; }
        public DbSet<WebLog> WebLogs { get; set; }
    }
}