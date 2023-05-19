using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_Lab_7.Models;

namespace MVC_Lab_7.Controllers
{
    [Authorize]
    public class DepartmentsController : Controller
    {
        private readonly ITIDbContext _context =new ITIDbContext();

        //public DepartmentsController(ITIDbContext context)
        //{
        //    _context = context;
        //}

        // GET: Departments

      
        public async Task<IActionResult> Index()
        {
              return _context.Departments != null ? 
                          View(await _context.Departments.ToListAsync()) :
                          Problem("Entity set 'ITIDbContext.Departments'  is null.");
        }

        // GET: Departments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Departments == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .FirstOrDefaultAsync(m => m.DeptId == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // GET: Departments/Create
        [Authorize(Roles = "Instructor")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DeptId,DeptName,Capacity")] Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        // GET: Departments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Departments == null)
            {
                return NotFound();
            }

            var department = await _context.Departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DeptId,DeptName,Capacity")] Department department)
        {
            if (id != department.DeptId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.DeptId))
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
            return View(department);
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Departments == null)
            {
                return NotFound();
            }

            var department = await _context.Departments
                .FirstOrDefaultAsync(m => m.DeptId == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Departments == null)
            {
                return Problem("Entity set 'ITIDbContext.Departments'  is null.");
            }
            var department = await _context.Departments.FindAsync(id);
            if (department != null)
            {
                _context.Departments.Remove(department);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(int id)
        {
          return (_context.Departments?.Any(e => e.DeptId == id)).GetValueOrDefault();
        }


        [HttpPost]
        public IActionResult UpdateCourses(int id, int[] coursesToRemove, int[] coursesToAdd)
        {
            var dept = _context.Departments.Include(a => a.Courses).FirstOrDefault(a => a.DeptId == id);
            foreach (var item in coursesToRemove)
            {
                var crs = _context.Courses.FirstOrDefault(a => a.Crs_Id == item);
                dept.Courses.Remove(crs);
            }
            foreach (var item in coursesToAdd)
            {
                var crs = _context.Courses.FirstOrDefault(a => a.Crs_Id == item);
                dept.Courses.Add(crs);
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult UpdateCourses(int id)
        {
            var dept = _context.Departments.Include(a => a.Courses).FirstOrDefault(a => a.DeptId == id);
            var allcourses = _context.Courses.ToList();
            var coursesInDept = dept.Courses.ToList();
            var coursesNotInDept = allcourses.Except(coursesInDept).ToList();
            ViewBag.coursesInDept = new SelectList(coursesInDept, "Crs_Id", "Crs_Name");
            ViewBag.coursesNotInDept = new SelectList(coursesNotInDept, "Crs_Id", "Crs_Name");
            ViewBag.deptid = dept.DeptId;
            return View();

        }


        public IActionResult ShowCourses(int id)
        {
            var model = _context.Departments.Include(a => a.Courses).FirstOrDefault(a => a.DeptId == id);
            return View(model);
        }





        public IActionResult UpdatStudentDegree(int Id, int crsid)
        {
            var model = _context.Students.Where(a => a.DeptNo == Id).ToList();
            ViewBag.crsid = crsid;
            return View(model);
        }


        [HttpPost]
        public IActionResult UpdatStudentDegree(int Id, int crsid, Dictionary<int, int> stdDegree)
        {

            foreach (var item in stdDegree)

            {

                var z = _context.StudentCourses.FirstOrDefault(a => a.CrsId == crsid && a.StudentId == item.Key);
                if (z == null)
                {
                    _context.StudentCourses.Add(new StudentCourse() { CrsId = crsid, StudentId = item.Key, Degree = item.Value });
                }

                else
                    z.Degree = item.Value;
            }
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
            //return RedirectToAction("UpdatStudentDegree", new { id = Id, crsid = crsid });
        }

    }
}
