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
                    "Server=db23515.databaseasp.net; Database=db23515; User Id=db23515; Password=Fi4-@Co9Rx7=; Encrypt=False; MultipleActiveResultSets=True;",
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
            modelBuilder.Entity<Email>().HasData(new Email
            {
                EmailID = 1,
                EmailBox = string.Empty,
                EmailPassword = string.Empty,
                EmailServer = string.Empty,
                EmailSSl = 0,
                EmailCompanyName = string.Empty,
                EmailAddress = string.Empty,
                EmailPhone = string.Empty,
                EmailImage = new byte[0]
            });
        }
        public void SeedIcons()
        {
            string sql = @"
SET IDENTITY_INSERT [dbo].[Icons] ON;
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (1, 'bi bi-alarm', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (2, 'bi bi-alarm-fill', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (3, 'bi bi-align-center', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (4, 'bi bi-align-end', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (5, 'bi bi-align-start', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (6, 'bi bi-app', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (7, 'bi bi-app-indicator', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (8, 'bi bi-archive', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (9, 'bi bi-arrow-up', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (10, 'bi bi-arrow-down', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (11, 'bi bi-bag', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (12, 'bi bi-bag-yen', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (13, 'bi bi-bank', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (14, 'bi bi-basket', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (15, 'bi bi-basket-fill', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (16, 'bi bi-battery', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (17, 'bi bi-bell', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (18, 'bi bi-bell-fill', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (19, 'bi bi-bicycle', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (20, 'bi bi-binoculars', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (21, 'bi bi-blockquote-left', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (22, 'bi bi-blockquote-right', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (23, 'bi bi-book', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (24, 'bi bi-book-fill', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (25, 'bi bi-bookmark', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (26, 'bi bi-bookmark-fill', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (27, 'bi bi-briefcase', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (28, 'bi bi-brush', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (29, 'bi bi-bug', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (30, 'bi bi-building', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (31, 'bi bi-calendar', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (32, 'bi bi-calendar-fill', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (33, 'bi bi-camera', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (34, 'bi bi-camera-fill', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (35, 'bi bi-cash', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (36, 'bi bi-caret-down', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (37, 'bi bi-caret-up', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (38, 'bi bi-chat', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (39, 'bi bi-chat-fill', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (40, 'bi bi-check-circle', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (41, 'bi bi-check-circle-fill', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (42, 'bi bi-chevron-down', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (43, 'bi bi-chevron-up', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (44, 'bi bi-clipboard', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (45, 'bi bi-clipboard-data', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (46, 'bi bi-clock', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (47, 'bi bi-cloud', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (48, 'bi bi-cloud-download', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (49, 'bi bi-code', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (50, 'bi bi-cup', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (51, 'bi bi-currency-dollar', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (52, 'bi bi-display', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (53, 'bi bi-envelope', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (54, 'bi bi-envelope-fill', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (55, 'bi bi-exclamation-triangle', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (56, 'bi bi-eye', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (57, 'bi bi-eye-fill', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (58, 'bi bi-file', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (59, 'bi bi-file-earmark', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (60, 'bi bi-gear', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (61, 'bi bi-gem', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (62, 'bi bi-globe', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (63, 'bi bi-heart', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (64, 'bi bi-heart-fill', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (65, 'bi bi-info-circle', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (66, 'bi bi-lightning', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (67, 'bi bi-lock', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (68, 'bi bi-lock-fill', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (69, 'bi bi-magic', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (70, 'bi bi-mailbox', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (71, 'bi bi-menu-button', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (72, 'bi bi-mic', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (73, 'bi bi-mic-fill', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (74, 'bi bi-mortarboard', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (75, 'bi bi-phone', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (76, 'bi bi-play-circle', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (77, 'bi bi-plus-circle', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (78, 'bi bi-question-circle', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (79, 'bi bi-receipt', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (80, 'bi bi-search', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (81, 'bi bi-send', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (82, 'bi bi-server', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (83, 'bi bi-shield-check', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (84, 'bi bi-star', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (85, 'bi bi-star-fill', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (86, 'bi bi-telephone', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (87, 'bi bi-telephone-fill', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (88, 'bi bi-trash', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (89, 'bi bi-trash-fill', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (90, 'bi bi-upload', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (91, 'bi bi-view-list', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (92, 'bi bi-wallet', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (93, 'bi bi-zoom-in', 1);
INSERT INTO [dbo].[Icons] (IconID, IconURL, IconStatus) VALUES (94, 'bi bi-zoom-out', 1);
SET IDENTITY_INSERT [dbo].[Icons] OFF;";
            if (!Icons.Any())
                this.Database.ExecuteSqlRaw(sql);
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
        public DbSet<CompanyInfo> CompanyInfos { get; set; }
        public DbSet<RestaurantAPI.Entities.Event> Events { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<AdminLog> AdminLogs { get; set; }
        public DbSet<WebLog> WebLogs { get; set; }
        public DbSet<Icon> Icons { get; set; }
        public DbSet<Email> Emails { get; set; }
    }
}