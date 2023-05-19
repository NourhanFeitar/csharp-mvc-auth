using Microsoft.AspNetCore.Mvc;

namespace MVC_Lab_7.Controllers
{
    public class Test : Controller
    {
        public IActionResult Index()
        {
            int id = 12;
            string name = "Nourhan";
            //HttpContext.Session.SetInt32("id", id);
            //HttpContext.Session.SetString("fname", name);
            Response.Cookies.Append("id", id.ToString());
            Response.Cookies.Append("fname", name);
            return View();
            
        }
        public IActionResult Show()
        {
            //int id = HttpContext.Session.GetInt32("id").Value;
            //string name = HttpContext.Session.GetString("fname");
            int id = int.Parse(Request.Cookies["id"]);
            string name = Request.Cookies["fname"];
            return Content("Show Action" + id.ToString() + ":" + name);
        }
    }
}
