using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Moq;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebApi.Controllers;
using WebApi.Data;
using WebApi.Models;
using Xunit;
using Task = System.Threading.Tasks.Task;

namespace WebApi.Test
{
    public class EmployeesControllerTest
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly EmployeesController _controller;
        public EmployeesControllerTest()
        {
            _dbContext = new ApplicationDbContext();
            _controller = new EmployeesController(_dbContext);
        }

        [Fact]
        public async Task Get_Employees_ReturnsOkResult()
        {
            var application = new MyWebApplication();
            var client = application.CreateClient();
            var response = await client.GetAsync($"api/Employees");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }



        [Fact]
        public void GetAll()
        {
            var okResult = _controller.GetEmployees();
            Assert.NotNull(okResult);
        }


        //Demo
        [Fact]
        public void TestEqual()
        {
            int a = 10, b = 20;
            Assert.Equal(30, Add(a, b));

        }
        private int Add(int a, int b)
        {
            return a + b;
        }
    }
}
