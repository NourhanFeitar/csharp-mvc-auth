using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_Lab_7.Models;

namespace MVC_Lab_7.Controllers
{
    public class UsersController : Controller
    {
        private readonly ITIDbContext _context = new ITIDbContext();

        //public UsersController(ITIDbContext context)
        //{
        //    _context = context;
        //}

        // GET: Users
        public async Task<IActionResult> Index()
        {
              return View(await _context.Users.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            var Roles = _context.Roles.ToList();
            ViewBag.rolesList = new SelectList(Roles, "Id", "RoleName");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Username,Password")] User user, int[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                user.Roles = _context.Roles.Where( role => selectedRoles.Contains(role.Id)).ToList();
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.Include(user => user.Roles).FirstOrDefaultAsync(user => user.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            var allroles = _context.Roles.ToList();

            var myrole = user.Roles.ToList();
            var notmyrole = allroles.Except(myrole);
            
            ViewBag.myrole= new SelectList(myrole, "Id", "RoleName");
            ViewBag.notmyrole= new SelectList(notmyrole, "Id", "RoleName");

            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Password")] User user , int[] roletoremove, int[] roletoadd)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var item in roletoremove)
                    {
                        var role = _context.Roles.FirstOrDefault(a => a.Id == item);
                        user.Roles.Remove(role);
                    }
                    foreach (var item in roletoadd)
                    {
                        var role = _context.Roles.FirstOrDefault(a => a.Id == item);
                        user.Roles.Add(role);
                    }
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'ITIDbContext.Users'  is null.");
            }
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
          return _context.Users.Any(e => e.Id == id);
        }
    }
}
