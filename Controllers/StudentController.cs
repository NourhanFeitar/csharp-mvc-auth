using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MVC_Lab_7.Models;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MVC_Lab_7.Controllers
{
    [Authorize]
    public class StudentController : Controller
    {
        ITIDbContext db = new ITIDbContext();
       
        public IActionResult Index()
        {
            

            return View(db.Students.Include(a => a.Department).ToList());

        }
        public IActionResult Create()
        {

            ViewBag.Departments = new SelectList(db.Departments.ToList(), "DeptId", "DeptName");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync(Student student,IFormFile stdimg)
        {
            if (stdimg.Length > 10)
                ModelState.AddModelError("", "check file size");
            else if (ModelState.IsValid)
            {
                db.Students.Add(student);
                db.SaveChanges();
                if (stdimg != null)
                {
                    string imgname = student.Id.ToString() + "." + stdimg.FileName.Split(".").Last();
                    using (var obj = new FileStream(@".\wwwroot\images\" + imgname, FileMode.Create))
                    {
                        await stdimg.CopyToAsync(obj);
                        student.ImgName = imgname;
                        db.SaveChanges();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Departments = new SelectList(db.Departments.ToList(), "DeptId", "DeptName", student.DeptNo);

            return View(student);
        }

        public IActionResult Details(int id)
        {
            var model = db.Students.FirstOrDefault(a => a.Id == id);
            return View(model);
        }
        public IActionResult Download()
        {
            return File("images/MVC9B.jpg", "image/jpeg", "task.jpg");
        }
    }
}
