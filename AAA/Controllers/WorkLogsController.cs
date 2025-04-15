using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using AAA.Data;
using Microsoft.EntityFrameworkCore;
using AAA.Models;

namespace AAA.Controllers
{
    public class WorkLogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WorkLogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var logs = _context.WorkLogs.Include(w => w.Employee);
            return View(await logs.ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(WorkLog workLog)
        {
            if (ModelState.IsValid)
            {
                _context.Add(workLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", workLog.EmployeeId);
            return View(workLog);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var log = await _context.WorkLogs.FindAsync(id);
            if (log == null) return NotFound();

            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", log.EmployeeId);
            return View(log);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(WorkLog workLog)
        {
            if (ModelState.IsValid)
            {
                _context.Update(workLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", workLog.EmployeeId);
            return View(workLog);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var log = await _context.WorkLogs.Include(w => w.Employee).FirstOrDefaultAsync(w => w.Id == id);
            return log == null ? NotFound() : View(log);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var log = await _context.WorkLogs.FindAsync(id);
            if (log != null)
            {
                _context.WorkLogs.Remove(log);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
