namespace ContosoUniversityCore.Features.Home
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Infrastructure;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        private readonly SchoolContext _db;

        public HomeController(SchoolContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var courses = await _db.Courses.ToListAsync();
            ViewBag.Courses = courses;
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
