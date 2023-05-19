using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using MVC_Lab_7.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace MVC_Lab_7.Controllers
{
    public class AccountController : Controller
    {
        private ITIDbContext _context = new ITIDbContext();

        //public AccountController(ITIDbContext dbContext) { 
        //    _context = dbContext;
        //}

        public IActionResult Index()
        {

            return View();
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnurl)
        {
            if (ModelState.IsValid)
            {

                var user = await _context.Users.Include(user => user.Roles).FirstOrDefaultAsync(a => a.Username == model.Name);

                if (user == null)
                {
                    ModelState.AddModelError("", "invalid user name or password");
                    return View(model);
                }

                ClaimsIdentity ci = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                ci.AddClaim(new Claim(ClaimTypes.Name, model.Name));
                foreach (var role in user.Roles)
                {
                    ci.AddClaim(new Claim(ClaimTypes.Role, role.RoleName));
                }
                ClaimsPrincipal cp = new ClaimsPrincipal(ci);
                // add data to cookies
                await HttpContext.SignInAsync(cp);//htt
                if (returnurl != null)
                    return LocalRedirect(returnurl);
                return RedirectToAction("Index", "home");
            }
            return View(model);

        }
        public async Task<IActionResult> LogOut()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
                await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "home");

        }

        public async Task<IActionResult> CreateUser()
        {
            var Roles = _context.Roles.ToList();
            ViewBag.rolesList = new SelectList(Roles, "Id", "RoleName");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(string password, string username, int[] selectedrole)
        {
            if (ModelState.IsValid)
            {
                User newuser=new User();
                newuser.Username = username;
                newuser.Password = password;
                List<Role> roles = _context.Roles.Where( role => selectedrole.Contains(role.Id)).ToList();
                newuser.Roles = roles;
                _context.Users.Add(newuser);
                _context.SaveChanges();
            }
            return RedirectToAction("Index", "home");
        }
    }
}
