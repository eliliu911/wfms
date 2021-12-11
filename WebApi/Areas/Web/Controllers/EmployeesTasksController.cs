using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Areas.Web.Controllers
{
    [Area("Web")]
    public class EmployeesTasksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeesTasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Get 员工管理主页面
        /// </summary>
        /// <returns>List</returns>
        public async Task<IActionResult> Index()
        {
            List<Employee_Gen> employee_Gens = new();
            var _list = await _context.Employees.ToListAsync();
            foreach (Employee e in _list)
            {
                Employee_Gen _gen = new Employee_Gen();
                _gen.Employee = e;
                List<V_EmployeeTasks> tasks = _context.V_EmployeeTasks.Where(x => x.Eid == e.Id).ToList();
                if (tasks != null)
                {
                    foreach (V_EmployeeTasks v in tasks)
                    {
                        WebApi.Models.Task t = new WebApi.Models.Task();
                        t.Id = v.Id;
                        t.TaskName = v.TaskName;
                        t.StartTime = v.StartTime;
                        t.Deadline = v.Deadline;
                        _gen.Tasks.Add(t);
                    }
                }
                employee_Gens.Add(_gen);
            }
            return View(employee_Gens);
        }

        /// <summary>
        /// Get 根据ID获取员工信息
        /// </summary>
        /// <param name="id">员工ID</param>
        /// <returns>Employee_Gen</returns>
        public async Task<IActionResult> Details(int? id)
        {
            Employee_Gen employee_Gen = new();
            var e = await _context.Employees.FindAsync(id);
            if (e == null)
            {
                return NotFound();
            }
            employee_Gen.Employee = e;

            List<V_EmployeeTasks> tasks = _context.V_EmployeeTasks.Where(x => x.Eid == e.Id).ToList();
            if (tasks != null)
            {
                foreach (V_EmployeeTasks v in tasks)
                {
                    WebApi.Models.Task t = new WebApi.Models.Task();
                    t.Id = v.Id;
                    t.TaskName = v.TaskName;
                    t.StartTime = v.StartTime;
                    t.Deadline = v.Deadline;
                    employee_Gen.Tasks.Add(t);
                }
            }
            return View(employee_Gen);
        }

        /// <summary>
        /// Get 新建员工页面
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Post 新建员工
        /// </summary>
        /// <param name="employee">员工Model</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FristName,LastName,HiredDate")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Employee created successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        /// <summary>
        /// Get 修改员工信息页面
        /// </summary>
        /// <param name="id">员工ID</param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int? id)
        {
            Employee_Gen employee_Gen = new();
            var e = await _context.Employees.FindAsync(id);
            if (e == null)
            {
                return NotFound();
            }
            employee_Gen.Employee = e;

            List<V_EmployeeTasks> tasks = _context.V_EmployeeTasks.Where(x => x.Eid == e.Id).ToList();
            if (tasks != null)
            {
                foreach (V_EmployeeTasks v in tasks)
                {
                    WebApi.Models.Task t = new();
                    t.Id = v.Id;
                    t.TaskName = v.TaskName;
                    t.StartTime = v.StartTime;
                    t.Deadline = v.Deadline;
                    employee_Gen.Tasks.Add(t);
                }
            }
            return View(employee_Gen);
        }

        /// <summary>
        /// Post 修改员工信息
        /// </summary>
        /// <param name="id">员工ID</param>
        /// <param name="employee">员工Model</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FristName,LastName,HiredDate")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Employee updated successfully";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
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
            return View(employee);
        }

        /// <summary>
        /// Get 删除员工页面
        /// </summary>
        /// <param name="id">员工ID</param>
        /// <returns></returns>
        public async Task<IActionResult> Delete(int? id)
        {
            Employee_Gen employee_Gen = new();
            var e = await _context.Employees.FindAsync(id);
            if (e == null)
            {
                return NotFound();
            }
            employee_Gen.Employee = e;

            List<V_EmployeeTasks> tasks = _context.V_EmployeeTasks.Where(x => x.Eid == e.Id).ToList();
            if (tasks != null)
            {
                foreach (V_EmployeeTasks v in tasks)
                {
                    WebApi.Models.Task t = new();
                    t.Id = v.Id;
                    t.TaskName = v.TaskName;
                    t.StartTime = v.StartTime;
                    t.Deadline = v.Deadline;
                    employee_Gen.Tasks.Add(t);
                }
            }
            return View(employee_Gen);
        }

        /// <summary>
        /// Post 删除员工方法
        /// </summary>
        /// <param name="id">员工ID</param>
        /// <returns></returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var employee = await _context.Employees.FindAsync(id);
            //_context.Employees.Remove(employee);
            //await _context.SaveChangesAsync();
            var _v = await _context.V_EmployeeTasks.Where(x => x.Eid == id).ToListAsync();
            var employee = await _context.Employees.FindAsync(id);
            if (_v == null)
            {
                return NotFound();
            }
            if (employee == null)
            {
                return NotFound();
            }
            for (int i = 0; i < _v.Count(); i++)
            {
                var task = _v[i];
                var _task = _context.Tasks.Find(task.Tid);
                if (_task != null)
                {
                    _context.Tasks.Remove(_task); //remove task
                }
                var _relation = _context.Relations.Where(x => x.Eid == task.Eid).ToList();
                if (_relation != null)
                {
                    for (int j = 0; j < _relation.Count(); j++)
                    {
                        var rel = _relation[j];
                        _context.Relations.Remove(rel); //remove relation
                    }
                }
            }
            _context.Employees.Remove(employee); //remove employee
            await _context.SaveChangesAsync(); //save changes
            TempData["Success"] = "Employee deleted successfully";

            return RedirectToAction(nameof(Index));
        }


        /// <summary>
        /// Get 新建任务页面
        /// </summary>
        /// <returns></returns>
        public IActionResult CreateTask()
        {
            return View();
        }
        /// <summary>
        /// Post 新建任务
        /// </summary>
        /// <param name="task">WebApi.Models.Task</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTask([Bind("TaskName,StartTime,Deadline")] WebApi.Models.Task task)
        {
            int id = int.Parse(RouteData.Values["id"].ToString());
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _context.Add(task);
                await _context.SaveChangesAsync(); //异步保存获取新ID
                Relation relation = new Relation();
                relation.Id = 0;
                relation.Eid = employee.Id;
                relation.Tid = task.Id;
                _context.Relations.Add(relation);
                await _context.SaveChangesAsync();
                TempData["Success"] = "New Task created successfully";
                return RedirectToAction(nameof(Edit), new { id = employee.Id });
            }
            return View(task);
        }


        /// <summary>
        /// Get 修改任务页面
        /// </summary>
        /// <param name="id">任务ID</param>
        /// <returns></returns>
        public async Task<IActionResult> EditTask(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks.FindAsync(id);
            if (task == null)
            {
                return NotFound();
            }
            return View(task);
        }
        /// <summary>
        /// Poat 修改任务
        /// </summary>
        /// <param name="id">任务ID</param>
        /// <param name="task">任务Model</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTask(int id, [Bind("Id,TaskName,StartTime,Deadline")] WebApi.Models.Task task)
        {
            if (id != task.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(task);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Task updated successfully";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskExists(task.Id))
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
            return View(task);
        }

        /// <summary>
        /// Get 删除任务页面
        /// </summary>
        /// <param name="id">任务ID</param>
        /// <returns></returns>
        public async Task<IActionResult> DeleteTask(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _context.Tasks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (task == null)
            {
                return NotFound();
            }

            return View(task);
        }

        /// <summary>
        /// Post 删除任务
        /// </summary>
        /// <param name="id">任务ID</param>
        /// <returns></returns>
        [HttpPost, ActionName("DeleteTask")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTaskConfirmed(int id)
        {
            var task = await _context.Tasks.FindAsync(id);
            var ralations = _context.Relations.Where(r => r.Tid == id);
            foreach (WebApi.Models.Relation r in ralations)
            {
                _context.Relations.Remove(r);
            }
            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Employee and Tasks deleted successfully";
            return RedirectToAction(nameof(Index));
        }








        /// <summary>
        /// 判断员工是否存在
        /// </summary>
        /// <param name="id">员工ID</param>
        /// <returns></returns>
        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
        /// <summary>
        /// 判断任务是否存在
        /// </summary>
        /// <param name="id">任务ID</param>
        /// <returns></returns>
        private bool TaskExists(int id)
        {
            return _context.Tasks.Any(e => e.Id == id);
        }
    }
}
