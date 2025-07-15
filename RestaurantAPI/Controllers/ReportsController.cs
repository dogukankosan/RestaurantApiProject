using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Context;
using RestaurantAPI.Dtos.ReportDtos;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly APIContext _context;
        public ReportsController(APIContext context)
        {
            _context = context;
        }
        [HttpGet("summary")]
        public async Task<IActionResult> Get()
        {
            int foodCount = await _context.Products.CountAsync(p => p.ProductStatus);
            int chefCount = await _context.Chefs.CountAsync(c => c.ChefStatus);
            int eventCount = await _context.Events.CountAsync(e => e.EventStatus);
            int reservationCount = await _context.Reservations.CountAsync(r => r.ReservationStatus == 2);
            ResultUserReportDto dto = new ResultUserReportDto
            {
                FoodCount = foodCount,
                ChefCount = chefCount,
                EventCount = eventCount,
                RezervationCount = reservationCount
            };
            return Ok(dto);
        }
        [HttpGet("admin")]
        public async Task<IActionResult> GetAdmin()
        {
            DateTime? today = DateTime.Today;
            int pendingReservations = await _context.Reservations.CountAsync(r => r.ReservationStatus == 0);
            int todayReservations = await _context.Reservations.CountAsync(r => r.ReservationDate.Date == today && r.ReservationStatus == 1);
            int todayPeople = await _context.Reservations
                .Where(r => r.ReservationDate.Date == today && r.ReservationStatus == 1)
                .SumAsync(r => (int?)r.ReservationCountOfPeople) ?? 0;
            string? nextCustomer = await _context.Reservations
                .Where(r => r.ReservationDate.Date == today && r.ReservationStatus == 1)
                .OrderBy(r => r.ReservationDate)
                .Select(r => r.ReservationNameSurname)
                .FirstOrDefaultAsync();
            int approved = await _context.Reservations.CountAsync(r => r.ReservationStatus == 1);
            int rejected = await _context.Reservations.CountAsync(r => r.ReservationStatus == 2);
            int arrived = await _context.Reservations.CountAsync(r => r.ReservationStatus == 3);
            int notArrived = await _context.Reservations.CountAsync(r => r.ReservationStatus == 4);
            int todayMessageCount = await _context.Messages.CountAsync(m => m.MessageSendDate.Date == today);
            int totalMessageCount = await _context.Messages.CountAsync();
            int productCount = await _context.Products.CountAsync(p => p.ProductStatus);
            int chefCount = await _context.Chefs.CountAsync(c => c.ChefStatus);
            int categoryCount = await _context.Categories.CountAsync(c => c.CategoryStatus);
            int eventCount = await _context.Events.CountAsync(e => e.EventStatus);
            int galleryCount = await _context.GalleryImages.CountAsync(b => b.ImageStatus);
            int mostProductCategory = await _context.Products
                .Where(p => p.ProductStatus)
                .GroupBy(p => p.CategoryID)
                .OrderByDescending(g => g.Count())
                .Select(g => g.Key)
                .FirstOrDefaultAsync();
            string? mostProductCategoryName = mostProductCategory != null
                ? await _context.Categories.Where(c => c.CategoryID == mostProductCategory).Select(c => c.CategoryName).FirstOrDefaultAsync()
                : "Kategori Yok";
            ResultAdminReportDto report = new ResultAdminReportDto
            {
                PendingReservationCount = pendingReservations,
                TodayReservationCount = todayReservations,
                TodayComingPeopleCount = todayPeople,
                NextCustomerName = nextCustomer ?? "Yok",
                ApprovedReservationCount = approved,
                RejectedReservationCount = rejected,
                ArrivedReservationCount = arrived,
                NotArrivedReservationCount = notArrived,
                TodayMessageCount = todayMessageCount,
                TotalMessageCount = totalMessageCount,
                ProductCount = productCount,
                ChefCount = chefCount,
                CategoryCount = categoryCount,
                EventCount = eventCount,
                GalleryImageCount = galleryCount,
                MostProductCategoryName = mostProductCategoryName
            };
            return Ok(report);
        }
        [HttpGet("monthly-reservations")]
        public async Task<IActionResult> GetMonthlyReservationStats()
        {
            int currentYear = DateTime.Today.Year;
            var reservationData = await _context.Reservations
                .Where(r => r.ReservationDate.Year == currentYear)
                .GroupBy(r => r.ReservationDate.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Arrived = g.Count(x => x.ReservationStatus == 3),
                    NotArrived = g.Count(x => x.ReservationStatus == 4)
                })
                .ToListAsync();
            List<ResultAdminMonthlyReservationStatsDto> allMonths = Enumerable.Range(1, 12).Select(i =>
            {
                var monthName = System.Globalization.CultureInfo
                    .GetCultureInfo("tr-TR")
                    .DateTimeFormat.GetMonthName(i);
                var match = reservationData.FirstOrDefault(x => x.Month == i);
                return new ResultAdminMonthlyReservationStatsDto
                {
                    Month = monthName,
                    Arrived = match?.Arrived ?? 0,
                    NotArrived = match?.NotArrived ?? 0
                };
            }).ToList();
            return Ok(allMonths);
        }
        [HttpGet("monthly-reservation-status")]
        public async Task<IActionResult> GetMonthlyReservationStatus()
        {
            int currentYear = DateTime.Today.Year;
            var reservationData = await _context.Reservations
                .Where(r => r.ReservationDate.Year == currentYear)
                .GroupBy(r => r.ReservationDate.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Approved = g.Count(x => x.ReservationStatus == 1),
                    Rejected = g.Count(x => x.ReservationStatus == 2),
                    Pending = g.Count(x => x.ReservationStatus == 0)
                })
                .ToListAsync();
            List<ResultAdminMonthlyReservationStatusDto> result = Enumerable.Range(1, 12).Select(i =>
            {
                string monthName = System.Globalization.CultureInfo
                    .GetCultureInfo("tr-TR")
                    .DateTimeFormat.GetMonthName(i);

                var match = reservationData.FirstOrDefault(x => x.Month == i);
                return new ResultAdminMonthlyReservationStatusDto
                {
                    Month = monthName,
                    Approved = match?.Approved ?? 0,
                    Rejected = match?.Rejected ?? 0,
                    Pending = match?.Pending ?? 0
                };
            }).ToList();
            return Ok(result);
        }
        [HttpGet("monthly-arrived-people")]
        public async Task<IActionResult> GetMonthlyArrivedPeople()
        {
            int currentYear = DateTime.Today.Year;
            var rawData = await _context.Reservations
                .Where(r => r.ReservationDate.Year == currentYear && r.ReservationStatus == 3)
                .GroupBy(r => r.ReservationDate.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    PeopleCount = g.Sum(x => x.ReservationCountOfPeople)
                })
                .ToListAsync();
            List<ResultAdminMonthlyPeopleStatsDto> result = Enumerable.Range(1, 12).Select(i =>
            {
                string monthName = System.Globalization.CultureInfo
                    .GetCultureInfo("tr-TR")
                    .DateTimeFormat.GetMonthName(i);
                var match = rawData.FirstOrDefault(x => x.Month == i);
                return new ResultAdminMonthlyPeopleStatsDto
                {
                    Month = monthName,
                    PeopleCount = match?.PeopleCount ?? 0
                };
            }).ToList();
            return Ok(result);
        }
        [HttpGet("monthly-message-stats")]
        public async Task<IActionResult> GetMonthlyMessageStats()
        {
            int currentYear = DateTime.Today.Year;
            var messageData = await _context.Messages
                .Where(m => m.MessageSendDate.Year == currentYear)
                .GroupBy(m => m.MessageSendDate.Month)
                .Select(g => new
                {
                    Month = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();
            List<ResultAdminMonthlyMessageStatsDto>? result = Enumerable.Range(1, 12).Select(i =>
            {
                string? monthName = System.Globalization.CultureInfo
                    .GetCultureInfo("tr-TR")
                    .DateTimeFormat.GetMonthName(i);
                var match = messageData.FirstOrDefault(x => x.Month == i);
                return new ResultAdminMonthlyMessageStatsDto
                {
                    Month = monthName,
                    MessageCount = match?.Count ?? 0
                };
            }).ToList();
            return Ok(result);
        }
    }
}