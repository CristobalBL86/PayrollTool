using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PayrollTool.Context;
using PayrollTool.Models;

namespace PayrollTool.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PayrollContext _context;
        private readonly DataAccess _data;
        private IConfiguration _config;

        public HomeController(ILogger<HomeController> logger, PayrollContext context, IConfiguration config)
        {
            _logger = logger;
            _context = context;
            _config = config;
            _data = new DataAccess( _config);
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetTransactionLog() {
            DataSet ds = _data.GetTransactionLog(DateTime.Today.AddDays(-1));
            if (_data.HasInfo(ds))
            {
                List<TransactionLogView> logs = new List<TransactionLogView>();

                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    logs.Add(
                        new TransactionLogView()
                        {
                            ID = Convert.ToInt32(item["ID"]),
                            Date = item["Date"].ToString(),
                            Product = item["Product"].ToString(),
                            Operation = item["Operation"].ToString(),
                            BoxQty = Convert.ToInt32(item["BoxQty"])
                        });
                }
                return Json(new { data = logs });
            }
            return View("Index.cshtml");
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }


}
