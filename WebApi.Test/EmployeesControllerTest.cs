using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Moq;
using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using WebApi.Controllers;
using WebApi.Data;
using WebApi.Models;
using Xunit;
using Xunit.Abstractions;
using Task = System.Threading.Tasks.Task;

namespace WebApi.Test
{
    public class EmployeesControllerTest
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly EmployeesController _controller;

        private readonly MyWebApplication _application;

        public HttpClient client { get; }
        public ITestOutputHelper Output { get; }

        public EmployeesControllerTest(ITestOutputHelper outputHelper)
        {
            _dbContext = new ApplicationDbContext();
            _controller = new EmployeesController(_dbContext);

            _application = new MyWebApplication();
            client = _application.CreateClient();
            Output = outputHelper;
        }

        [Fact]
        public async Task Get_Employees_ReturnsOkResult()
        {
            var response = await client.GetAsync($"api/Employees");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task Get_EmployeesById_ReturnsOKResult()
        {
            var id = 12;
            var response = await client.GetAsync($"api/Employees/{id}");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async Task Put_ShouldBe_OK()
        {
            var id = 25;
            var content = new StringContent(
                JsonConvert.SerializeObject(new { Id = id, FristName = "xUnit", LastName = "xUnit", HiredDate = "2000-01-01"}),
                Encoding.UTF8,
                MediaTypeNames.Application.Json
                );
            var response = await client.PutAsync($"api/Employees/{id}", content);
            var responseTest = await response.Content.ReadAsStringAsync();
            Output.WriteLine(responseTest);
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            //Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task Post_ShouldBe_OK()
        {
            var content = new StringContent(
                JsonConvert.SerializeObject(new { FristName = "xUnit", LastName = "xUnit", HiredDate = "2000-01-01" }),
                Encoding.UTF8,
                MediaTypeNames.Application.Json
                );
            var response = await client.PostAsync($"api/Employees", content);
            var responseTest = await response.Content.ReadAsStringAsync();
            Output.WriteLine(responseTest);
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        //Demo
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
