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
        //GetAll
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee_Gen>>> GetAllAsync() 
        {
            List<Employee_Gen> employee_Gens = new List<Employee_Gen>();
            var _list = await _context.Employees.ToListAsync();
            foreach (Employee e in _list)
            {
                Employee_Gen _gen = new Employee_Gen();
                _gen.Id = e.Id;
                _gen.FristName = e.FristName;
                _gen.LastName = e.LastName;
                _gen.HiredDate = e.HiredDate;
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
        //Get By Id
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
            _gen.Id = e.Id;
            _gen.FristName = e.FristName;
            _gen.LastName = e.LastName;
            _gen.HiredDate = e.HiredDate;
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
        //Add
        [HttpPost]
        public async Task<ActionResult<Employee_Gen>> PostEmployee_Gen(Employee_Gen employee_Gen)
        {
            Employee _e = new Employee();
            _e.Id = employee_Gen.Id;
            _e.FristName = employee_Gen.FristName;
            _e.LastName = employee_Gen.LastName;
            _e.HiredDate = employee_Gen.HiredDate;
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
            return CreatedAtAction("GetAllAsync", new { id = employee_Gen.Id }, employee_Gen);
        }
    }
}
