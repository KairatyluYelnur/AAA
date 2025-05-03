using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AAA.Data;
using AAA.Models;
using Microsoft.AspNetCore.Authorization;
namespace AAA.Controllers
{
    [Authorize]
    public class EmployeesController : Controller
        {
            private readonly ApplicationDbContext _context;

            public EmployeesController(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IActionResult> Index()
            {
                return View(await _context.Employees.ToListAsync());
            }

            public IActionResult Create()
            {
                return View();
            }





    [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(employee);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(employee);
            }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id)
            {
                var employee = await _context.Employees.FindAsync(id);
                return employee == null ? NotFound() : View(employee);
            }

            [HttpPost]
        [Authorize(Roles = "Admin")][ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Employee employee)
            {
                if (ModelState.IsValid)
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(employee);
            }

            public async Task<IActionResult> Delete(int id)
            {
                var employee = await _context.Employees.FindAsync(id);
                return employee == null ? NotFound() : View(employee);
            }

            [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
            {
                var employee = await _context.Employees.FindAsync(id);
                if (employee != null)
                {
                    _context.Employees.Remove(employee);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }
        }
    }

