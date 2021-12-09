using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Controllers
{
    //列举所有员工及对应任务
    [Route("api/[controller]")]
    [ApiController]
    public class V_EmployeeTasksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public V_EmployeeTasksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/V_EmployeeTasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<V_EmployeeTasks>>> GetV_EmployeeTaskss()
        {
            return await _context.V_EmployeeTasks.ToListAsync();
        }

        // GET: api/V_EmployeeTasks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<V_EmployeeTasks>> GetV_EmployeeTasks(int id)
        {
            var v_EmployeeTasks = await _context.V_EmployeeTasks.FindAsync(id);

            if (v_EmployeeTasks == null)
            {
                return NotFound();
            }

            return v_EmployeeTasks;
        }

        //// PUT: api/V_EmployeeTasks/5
        //// update
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutV_EmployeeTasks(int id, V_EmployeeTasks v_EmployeeTasks)
        //{
        //    if (id != v_EmployeeTasks.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(v_EmployeeTasks).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!V_EmployeeTasksExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/V_EmployeeTasks
        //// Add
        //[HttpPost]
        //public async Task<ActionResult<V_EmployeeTasks>> PostV_EmployeeTasks(V_EmployeeTasks v_EmployeeTasks)
        //{
        //    //_context.V_EmployeeTaskss.Add(v_EmployeeTasks);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetV_EmployeeTasks", new { id = v_EmployeeTasks.Id }, v_EmployeeTasks);
        //}

        //// DELETE: api/V_EmployeeTasks/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteV_EmployeeTasks(int id)
        //{
        //    var v_EmployeeTasks = await _context.V_EmployeeTasks.FindAsync(id);
        //    if (v_EmployeeTasks == null)
        //    {
        //        return NotFound();
        //    }

        //    //_context.V_EmployeeTaskss.Remove(v_EmployeeTasks);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool V_EmployeeTasksExists(int id)
        {
            return _context.V_EmployeeTasks.Any(e => e.Id == id);
        }
    }
}
