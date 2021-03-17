using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PayrollTool.Context;
using PayrollTool.Models;

namespace PayrollTool.Controllers
{
    public class UserController : Controller
    {
        private readonly PayrollContext _context;

        public UserController(PayrollContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            return View(new User());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignIn(int id, [Bind("UserId,UserName,Password")] User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        if (!UserExists(user.UserName))
                        {
                            throw new Exception("Invalid User Name, please check.");
                        }

                        if (!_context.User.Any(u => u.UserName == user.UserName && u.Password == user.Password)) {
                            throw new Exception("Invalid Login, please check.");
                        }
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!UserExists(user.UserName))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    //HttpContext.Session.SetString("logged", "yes");
                    return RedirectToAction("Index", "AssistanceLog");
                }
                return View(nameof(Index), user);
            }
            catch (Exception ex) {
                ViewBag.ErrorMessage = ex.Message;
                return View(nameof(Index), user);
            }
        }

        private bool UserExists(string userName)
        {
            return _context.User.Any(e => e.UserName == userName);
        }
    }
}
