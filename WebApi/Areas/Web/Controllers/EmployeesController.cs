using Microsoft.AspNetCore.Mvc;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Areas.Web.Controllers
{
    [Area("Web")]
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;
        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<Employee> objEmployeeList = _context.Employees.ToList();
            return View(objEmployeeList);
        }

        //Get
        public IActionResult Create()
        {
            return View();
        }
        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee obj)
        {
            if (obj.FristName == null && obj.FristName=="")
            {
                ModelState.AddModelError("CustomError", "FristName cannot be empty.");
            }
            if (obj.LastName == null && obj.LastName == "")
            {
                ModelState.AddModelError("CustomError", "LastName cannot be empty.");
            }
            if (obj.HiredDate.ToString() == null && !IsDate(obj.HiredDate.ToString()))
            {
                ModelState.AddModelError("CustomError", "Please enter the right time format.");
            }
            if (ModelState.IsValid)
            {
                _context.Employees.Add(obj);
                _context.SaveChanges();
                TempData["Success"] = "Employee created successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //Get
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var employeesFromDb = _context.Employees.Find(id);
            //var employeesFromDb = _context.Employees.FirstOrDefault(c => c.Id == id);
            if (employeesFromDb == null)
            {
                return NotFound();
            }
            return View(employeesFromDb);
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Employee obj)
        {
            if (obj.FristName == null && obj.FristName == "")
            {
                ModelState.AddModelError("CustomError", "FristName cannot be empty.");
            }
            if (obj.LastName == null && obj.LastName == "")
            {
                ModelState.AddModelError("CustomError", "LastName cannot be empty.");
            }
            if (obj.HiredDate.ToString() == null && !IsDate(obj.HiredDate.ToString()))
            {
                ModelState.AddModelError("CustomError", "Please enter the right time format.");
            }
            if (ModelState.IsValid)
            {
                _context.Employees.Update(obj);
                _context.SaveChanges();
                TempData["Success"] = "Employee updated successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //Get
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var employeeFromDb = _context.Employees.Find(id);
            //var employeeFormDb = _db.Employees.FirstOrDefault(c => c.Id == id);
            if (employeeFromDb == null)
            {
                return NotFound();
            }
            return View(employeeFromDb);
        }

        //Post
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _context.Employees.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _context.Employees.Remove(obj);
            _context.SaveChanges();
            TempData["Success"] = "Employee deleted successfully";
            return RedirectToAction("Index");
        }

        //判断时间格式
        public bool IsDate(string strDate)
        {
            try
            {
                DateTime.Parse(strDate);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
