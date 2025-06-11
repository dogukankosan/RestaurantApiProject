using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Context;
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
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            int foodCount = await _context.Products.CountAsync(p => p.ProductStatus);
            int chefCount = await _context.Chefs.CountAsync(c => c.ChefStatus);
            int eventCount = await _context.Events.CountAsync(e => e.EventStatus);
            int reservationCount = await _context.Reservations.CountAsync(r => r.ReservationStatus);
            Report dto = new Report
            {
                FoodCount = foodCount,
                ChefCount = chefCount,
                EventCount = eventCount,
                RezervationCount = reservationCount
            };
            return Ok(dto);
        }
    }
}