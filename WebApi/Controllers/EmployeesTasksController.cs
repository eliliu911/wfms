using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers
{
    //员工和任务管理CRUD
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmployeesTasksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public EmployeesTasksController(ApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// GetAll 
        /// --获取所有员工数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName(nameof(GetAllAsync))]
        public async Task<ActionResult<IEnumerable<Employee_Gen>>> GetAllAsync() 
        {
            List<Employee_Gen> employee_Gens = new List<Employee_Gen>();
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
                        Models.Task t = new Models.Task();
                        t.Id = v.Id;
                        t.TaskName = v.TaskName;
                        t.StartTime = v.StartTime;
                        t.Deadline = v.Deadline;
                        _gen.Tasks.Add(t);
                    }
                }
                employee_Gens.Add(_gen);
            }
            return employee_Gens;
        }
        /// <summary>
        /// Get By ID 
        /// --根据id获取员工数据
        /// </summary>
        /// <param name="id">Employee id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ActionName(nameof(GetAllAsync))]
        public async Task<ActionResult<Employee_Gen>> GetAllAsync(int id)
        {
            var e = await _context.Employees.FindAsync(id);
            if (e == null)
            {
                return NotFound();
            }
            Employee_Gen _gen = new Employee_Gen();
            _gen.Employee = e;
            
            List<V_EmployeeTasks> tasks = _context.V_EmployeeTasks.Where(x => x.Eid == e.Id).ToList();
            if (tasks != null)
            {
                foreach (V_EmployeeTasks v in tasks)
                {
                    Models.Task t = new Models.Task();
                    t.Id = v.Id;
                    t.TaskName = v.TaskName;
                    t.StartTime = v.StartTime;
                    t.Deadline = v.Deadline;
                    _gen.Tasks.Add(t);
                }
            }
            return _gen;
        }
        /// <summary>
        /// Create Employee and Tasks 
        /// --同时创建员工和任务
        /// </summary>
        /// <param name="employee_Gen">include Employee and List<Task></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName(nameof(CreateEmployees_Tasks))]
        public async Task<ActionResult<Employee_Gen>> CreateEmployees_Tasks(Employee_Gen employee_Gen)
        {
            Employee _e = new Employee();
            _e = employee_Gen.Employee;
            _context.Employees.Add(_e);
            await _context.SaveChangesAsync(); //异步保存获取新ID
            if (employee_Gen.Tasks != null)
            {
                foreach (Models.Task _task in employee_Gen.Tasks)
                {
                    _context.Tasks.Add(_task);
                    await _context.SaveChangesAsync(); //异步保存获取新ID
                    Relation relation = new Relation();
                    relation.Id = 0;
                    relation.Eid = _e.Id;
                    relation.Tid = _task.Id; 
                    _context.Relations.Add(relation);
                }
            }
            await _context.SaveChangesAsync(); //异步保存ALL
            return CreatedAtAction(nameof(CreateEmployees_Tasks), new { id = employee_Gen.Employee.Id }, employee_Gen);
        }
        /// <summary>
        /// Add Tasks to Employee 
        /// --通过Employee ID向其添加任务
        /// </summary>
        /// <param name="id">Emplpyee id</param>
        /// <param name="tasks">New Tasks</param>
        /// <returns></returns>
        [HttpPost("{id}")]
        [ActionName(nameof(AddTasktoEmployee))]
        public async Task<ActionResult<Employee_Gen>> AddTasktoEmployee(int id, List<Models.Task> tasks)
        {
            var employee = await _context.Employees.FindAsync(id);
            Employee_Gen employee_Gen = new Employee_Gen();
            if (employee == null)
            {
                return NotFound();
            }
            else
            {
                if (tasks != null)
                {
                    foreach (Models.Task _task in tasks)
                    {
                        _context.Tasks.Add(_task);
                        await _context.SaveChangesAsync(); //异步保存获取新ID
                        Relation relation = new Relation();
                        relation.Id = 0;
                        relation.Eid = id;
                        relation.Tid = _task.Id;
                        _context.Relations.Add(relation);
                    }
                    await _context.SaveChangesAsync(); //异步保存ALL
                    employee_Gen.Employee = employee;
                    employee_Gen.Tasks = tasks;
                }
            }
            return CreatedAtAction(nameof(AddTasktoEmployee), new { id = employee_Gen.Employee.Id }, tasks);
        }

        /// <summary>
        /// Update employee and task
        /// --更新员工和任务数据
        /// </summary>
        /// <param name="id">员工id</param>
        /// <param name="employee_Gen">员工model</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee_Gen(int id, Employee_Gen employee_Gen)
        {
            if (id != employee_Gen.Employee.Id)
            {
                return BadRequest();
            }
            _context.Entry(employee_Gen.Employee).State = EntityState.Modified;
            foreach (Models.Task task in employee_Gen.Tasks)
            {
                _context.Entry(task).State = EntityState.Modified;
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        /// <summary>
        /// Delete all by id
        /// --删除员工及其任务
        /// </summary>
        /// <param name="id">Employee id</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee_Gen(int id)
        {
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
                var _relation = _context.Relations.Where(x=>x.Eid == task.Eid).ToList();
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

            return NoContent();
        }
        /// <summary>
        /// Use id to delete task. 
        /// --根据任务ID删除任务
        /// </summary>
        /// <param name="id">Employee Id</param>
        /// <param name="tid">Task id</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteTaskFromEmployee(int id, int tid)
        {
            var _v = _context.V_EmployeeTasks.Where(x => x.Eid == id && x.Tid == tid).ToList();
            foreach (V_EmployeeTasks v in _v)
            {
                Relation r = new Relation();
                var task = _context.Tasks.Find(v.Tid);
                r.Id = v.Id;
                r.Eid = v.Eid;
                r.Tid = v.Tid;
                _context.Relations.Remove(r);
                _context.Tasks.Remove(task);
            }
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }

    }
}
