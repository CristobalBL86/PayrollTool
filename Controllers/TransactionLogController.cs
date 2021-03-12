using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PayrollTool.Context;
using PayrollTool.Models;
using static PayrollTool.Helper;

namespace PayrollTool.Controllers
{
    public class TransactionLogController : Controller
    {
        private readonly PayrollContext _context;

        public TransactionLogController(PayrollContext context)
        {
            _context = context;
        }

        // GET: TransactionLog
        public async Task<IActionResult> Index()
        {
            var payrollContext = _context.TransacionLog
                .Include(t => t.Operation).Include(t => t.Product)
                .Where(t=> t.Date.Date == DateTime.Today);
            return View(await payrollContext.ToListAsync());
        }

        // GET: TransactionLog/Create
        [NoDirectAccess]
        public async Task<IActionResult> AddOrEdit(int id=0)
        {
            var checks = _context.AssistanceLog
                .Where(c => c.Date.Date == DateTime.Today)
                .Count();

            if (checks == 0)
            {
                return Json(new
                {
                    isValid = false,
                    html = Helper.RenderRazorViewToString(this, "_ViewAll",
                     _context.TransacionLog.Include(t => t.Operation).Include(t => t.Product)
                    .Where(t => t.Date.Date == DateTime.Today).ToList())
                });
            }

            if (id == 0)
            {
                TransactionLog trxLog = new TransactionLog();
                trxLog.Date = DateTime.Now;
                ViewData["OperationId"] = new SelectList(_context.Operation, "OperationId", "OperationName");
                ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductName");

                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "AddOrEdit", trxLog) });
            }
            else
            {
                var transactionLog = await _context.TransacionLog.FindAsync(id);
                if (transactionLog == null)
                {
                    return NotFound();
                }
                ViewData["OperationId"] = new SelectList(_context.Operation, "OperationId", "OperationName", transactionLog.OperationId);
                ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductName", transactionLog.ProductId);

                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "AddOrEdit", transactionLog) });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit(int id, [Bind("TransactionLogId,Date,ProductId,OperationId,BoxQty")] TransactionLog transactionLog)
        {
            if (ModelState.IsValid)
            {
                if (id == 0)
                {
                    //transactionLog.Date = DateTime.Now;
                    _context.Add(transactionLog);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    try
                    {
                        _context.Update(transactionLog);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!TransactionLogExists(transactionLog.TransactionLogId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }                    
                }

                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", 
                    _context.TransacionLog.Include(t => t.Operation).Include(t => t.Product)
                    .Where(t => t.Date.Date == DateTime.Today).ToList()) });
            }
            ViewData["OperationId"] = new SelectList(_context.Operation, "OperationId", "OperationName", transactionLog.OperationId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "ProductName", transactionLog.ProductId);

            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "AddOrEdit", transactionLog) });
        }

        // GET: TransactionLog/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var transactionLog = await _context.TransacionLog
                .Include(t => t.Operation)
                .Include(t => t.Product)
                .FirstOrDefaultAsync(m => m.TransactionLogId == id);
            if (transactionLog == null)
            {
                return NotFound();
            }

            return View(transactionLog);
        }

        // POST: TransactionLog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transactionLog = await _context.TransacionLog.FindAsync(id);
            _context.TransacionLog.Remove(transactionLog);
            await _context.SaveChangesAsync();

            return Json(new { html = Helper.RenderRazorViewToString(this, "_ViewAll", _context.TransacionLog.Include(t => t.Operation).Include(t => t.Product).ToList()) });
        }

        private bool TransactionLogExists(int id)
        {
            return _context.TransacionLog.Any(e => e.TransactionLogId == id);
        }
    }
}
