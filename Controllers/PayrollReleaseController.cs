using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using PayrollTool.Context;
using PayrollTool.Models;
using static PayrollTool.Helper;

namespace PayrollTool.Controllers
{
    public class PayrollReleaseController : Controller
    {
        private readonly PayrollContext _context;
        private IConfiguration _config;
        private readonly DataAccess _data;

        public PayrollReleaseController(PayrollContext context, IConfiguration config )
        {
            _context = context;
            _config = config;
            _data = new DataAccess(_config);
        }

        // GET: PayrollReleases
        [NoDirectAccessAttribute]
        public async Task<IActionResult> Index()
        {
            var list = new List<PayrollRelease>();
            return View(list.ToList());
        }

        [HttpGet]
        public async Task<IActionResult> GetPayroll(string stDate, string edDate) {

            var dates = new DateRange() {
                startDate = Convert.ToDateTime(stDate),
                endDate = Convert.ToDateTime(edDate)
            };

            ViewBag.stDate = dates.startDate;
            ViewBag.edDate = dates.endDate;

            DataSet ds = _data.GetPayrollResult(dates.startDate, dates.endDate);

            if (_data.HasInfo(ds))
            {
                var payrollContext = _context.PayrollRelease.Include(p => p.Employee)
                .Where(p => p.Date.Date >= dates.startDate && p.Date.Date <= dates.endDate);

                return Json(new
                {
                    isValid = true,
                    html = Helper.RenderRazorViewToString(this, "_ViewPayroll", payrollContext.ToList())
                });
            }
            return View(nameof(Index));
        }

        // GET: PayrollReleases/Edit/5
        public async Task<IActionResult> Edit(int? id = 0)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var payrollRelease = await _context.PayrollRelease.FindAsync(id);
            if (payrollRelease == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "Name", payrollRelease.EmployeeId);

            return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "Edit", payrollRelease) });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PayrollReleaseId,Date,EmployeeId,Ammount")] PayrollRelease payrollRelease)
        {
            if (id != payrollRelease.PayrollReleaseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //take empId 'cause not sent to controller(disabled)
                    var empid = _context.PayrollRelease.Where(p => p.PayrollReleaseId == id)
                        .Select(p => p.EmployeeId).FirstOrDefault();
                    payrollRelease.EmployeeId = empid;

                    _context.Update(payrollRelease);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PayrollReleaseExists(payrollRelease.PayrollReleaseId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return Json(new
                {
                    isValid = true,
                        html = Helper.RenderRazorViewToString(this, "_ViewPayroll",
                        _context.PayrollRelease.Include(p => p.Employee).ToList())
                    });
            }

            ViewData["EmployeeId"] = new SelectList(_context.Employee, "EmployeeId", "Name", payrollRelease.EmployeeId);
            
            return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", payrollRelease) });
        }

        private bool PayrollReleaseExists(int id)
        {
            return _context.PayrollRelease.Any(e => e.PayrollReleaseId == id);
        }

        public IActionResult Calculate()
        {
            return View(
                new DateRange()
                {
                    startDate = DateTime.Now.AddDays(-6),
                    endDate = DateTime.Now.Date
                }
            );
        }


        public async Task<IActionResult> GetReport(string stDate, string edDate)
        {
            var dates = new DateRange()
            {
                startDate = Convert.ToDateTime(stDate),
                endDate = Convert.ToDateTime(edDate)
            };

            //EPPlus ExcelPackage
            string strfileName = "Payroll.xlsx";
            ExcelPackage workbook = new ExcelPackage();
            ExcelWorksheet Sheet = workbook.Workbook.Worksheets.Add("Payroll");

            List<PayrollRelease> payments = new List<PayrollRelease>();

            //payments in the dates range
            payments = _context.PayrollRelease.Include(p=> p.Employee)
                .Where(p => p.Date.Date >= dates.startDate.Date && p.Date.Date <= dates.endDate.Date)
                .ToList();

            Sheet.Cells["A1"].Value = "Employee Name";
            int row = 2, col = 2;

            foreach (var _date in payments.OrderBy(p => p.Date).GroupBy(p => p.Date).Select(p => p.Key)) {
                row = 2;
                foreach (var _person in payments.OrderBy(p => p.Employee.Name).GroupBy(p => p.Employee.Name).Select(p => p.Key)) {

                    Sheet.Cells[1, col].Value = _date.ToString();
                    Sheet.Cells[row, 1].Value = _person;

                    Sheet.Cells[row, col].Value = 
                        payments.Where(p=> p.Date == _date && p.Employee.Name == _person).Sum(p=> p.Ammount);
                    
                    row++;
                }
                col++;
            }

            Sheet.Cells["A:AZ"].AutoFitColumns();

            string handle = Guid.NewGuid().ToString();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                workbook.SaveAs(memoryStream);
                memoryStream.Position = 0;
                TempData[handle] = memoryStream.ToArray();
            }

            byte[] data = TempData[handle] as byte[];
            return File(data, "application/vnd.ms-excel", strfileName);
        }

        [HttpGet]
        public virtual ActionResult completeDownload(string fileGuid, string fileName) {
            if (TempData[fileGuid] != null)
            {
                byte[] data = TempData[fileGuid] as byte[];
                return File(data, "application/vnd.ms-excel", fileName);
            }
            else
            {
                return new EmptyResult();
            }
        }
    }
}
