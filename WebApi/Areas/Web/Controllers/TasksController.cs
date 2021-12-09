using Microsoft.AspNetCore.Mvc;
using WebApi.Data;

namespace WebApi.Areas.Web.Controllers
{
    [Area("Web")]
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _context;
        public TasksController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
