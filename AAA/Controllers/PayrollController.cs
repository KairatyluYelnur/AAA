using AAA.Data;
using AAA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace AAA.Controllers
{
    public class PayrollController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PayrollController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(DateTime? from, DateTime? to)
        {
            if (from == null || to == null)
            {
                from = DateTime.Today.AddDays(-30);
                to = DateTime.Today;
            }

            var logs = await _context.WorkLogs
    .Include(w => w.Employee)
    .Where(w => w.StartTime >= from && w.StartTime <= to)
    .ToListAsync();

#pragma warning disable CS8602 
            var payroll = logs
    .GroupBy(w => w.Employee)
    .Where(g => g.Key != null) 
    .Select(g => new Payroll
    {
        Employee = g.Key,
        PeriodStart = from.Value,
        PeriodEnd = to.Value,
        TotalHours = g.Sum(w => w.Duration),
        TotalPay = (decimal)g.Sum(w => w.Duration) * g.Key.HourlyRate
    }).ToList();
#pragma warning restore CS8602 

            return View(payroll);
        }
    }
}
