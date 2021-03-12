using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PayrollTool.Context;
using PayrollTool.Models;

namespace PayrollTool.Controllers
{
    public class AssistanceLogController : Controller
    {
        private readonly PayrollContext _context;

        public AssistanceLogController(PayrollContext context)
        {
            _context = context;
        }

        // GET: AssistanceLog
        public async Task<IActionResult> Index()
        {
            var payrollContext = _context.AssistanceLog
                .Include(a => a.Employee)
                .Where(a=> a.Date.Date == DateTime.Today);
            return View(await payrollContext.ToListAsync());
        }


        // GET: AssistanceLog/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "Name");
            return View(new AssistanceLog() { Date = DateTime.Now});
        }

        // POST: AssistanceLog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AssistanceLogId,Date,EmployeeId")] AssistanceLog assistanceLog)
        {
            if (ModelState.IsValid)
            {
                //assistanceLog.Date = DateTime.Now;
                _context.Add(assistanceLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "Name", assistanceLog.EmployeeId);
            return View(assistanceLog);
        }

        // GET: AssistanceLog/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var assistanceLog = await _context.AssistanceLog
                .Include(a => a.Employee)
                .FirstOrDefaultAsync(m => m.AssistanceLogId == id);
            if (assistanceLog == null)
            {
                return NotFound();
            }

            return View(assistanceLog);
        }

        // POST: AssistanceLog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var assistanceLog = await _context.AssistanceLog.FindAsync(id);
            _context.AssistanceLog.Remove(assistanceLog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AssistanceLogExists(int id)
        {
            return _context.AssistanceLog.Any(e => e.AssistanceLogId == id);
        }
    }
}