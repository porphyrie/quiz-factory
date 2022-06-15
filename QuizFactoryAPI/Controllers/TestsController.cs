using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizFactoryAPI.Authorization;
using QuizFactoryAPI.Data;
using QuizFactoryAPI.Entities;
using QuizFactoryAPI.Models.Tests;
using QuizFactoryAPI.Services;

namespace QuizFactoryAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        private ITestService _testService;

        public TestsController(ITestService testService)
        {
            _testService = testService;
        }

        [Authorize(Role.profesor)]
        [HttpPost]
        public IActionResult AddTest(AddTestRequest model)
        {
            _testService.AddTest(model);
            return Ok(new { message = "The test has been created" });
        }
    }
}
