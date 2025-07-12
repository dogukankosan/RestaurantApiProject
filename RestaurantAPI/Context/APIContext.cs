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
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<About>().HasData(new About
            {
                AboutID = 1,
                AboutCompanyLogo = new byte[0],
                AboutCompanyName = string.Empty,
                AboutImage1 = new byte[0],
                AboutImage2 = new byte[0],
                AboutDesc = string.Empty,
                AboutWhyChoose = string.Empty,
                AboutReportImage = new byte[0],
                AboutRezervationImage = new byte[0]
            });
            modelBuilder.Entity<CompanyInfo>().HasData(new CompanyInfo
            {
                CompanyInfoID = 1,
                CompanyInfoAddress = string.Empty,
                CompanyInfoPhone = string.Empty,
                CompanyInfoMail = string.Empty,
                CompanyInfoOpenClosed = string.Empty,
                CompanyInfoGithubLink = string.Empty,
                CompanyInfoWebSiteLink = string.Empty,
                CompanyInfoInstagramLink = string.Empty,
                CompanyInfoLinkedinLink = string.Empty,
                CompanyInfoIFrame = string.Empty
            });
            modelBuilder.Entity<Feature>().HasData(new Feature
            {
                FeatureID = 1,
                FeatureTitle = string.Empty,
                FeatureSubTitle = string.Empty,
                FeatureDescription = string.Empty,
                FeatureVideoUrl = string.Empty,
                FeatureImageData = new byte[0]
            });
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
        public DbSet<Icon> Icons { get; set; }
    }
}