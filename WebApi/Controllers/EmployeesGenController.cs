using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers
{
    //员工和任务管理CRUD
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesGenController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public EmployeesGenController(ApplicationDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// GetAll
        /// </summary>
        /// <returns></returns>
        [HttpGet]
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
        /// </summary>
        /// <param name="id">Employee id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Employee_Gen>>> GetAllAsync(int id)
        {
            List<Employee_Gen> employee_Gens = new List<Employee_Gen>();
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
            employee_Gens.Add(_gen);
            return employee_Gens;
        }
        /// <summary>
        /// Create Employee and Tasks
        /// </summary>
        /// <param name="employee_Gen">include Employee and List<Task></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Employee_Gen>> PostEmployee_Gen(Employee_Gen employee_Gen)
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
            return CreatedAtAction("GetAllAsync", new { id = employee_Gen.Employee.Id }, employee_Gen);
        }
        /// <summary>
        /// Add Tasks to Employee
        /// </summary>
        /// <param name="id">Emplpyee id</param>
        /// <param name="tasks">New Tasks</param>
        /// <returns></returns>
        [HttpPut]
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
            return CreatedAtAction("GetAllAsync", new { id = employee_Gen.Employee.Id }, employee_Gen);
        }

        /// <summary>
        /// Only update employee and tasks info
        /// </summary>
        /// <param name="id">Employee id</param>
        /// <param name="employee_Gen">include Employee and List<Task></param>
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
        /// Use id to delete task from Table Relations ,Only delete form Relation.
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
                r.Id = v.Id;
                r.Eid = v.Eid;
                r.Tid = v.Tid;
                _context.Relations.Remove(r);
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
