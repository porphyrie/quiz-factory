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

        [Authorize(Role.profesor, Role.student)]
        [HttpGet("{username}")]
        public IActionResult GetTests(string username)
        {
            var currentUser = (User)HttpContext.Items["User"];
            var tests = _testService.GetTests(username, currentUser.Role.ToString());
            return Ok(tests);
        }

        [Authorize(Role.profesor)]
        [HttpGet("{testId}")]
        public IActionResult GetTestDetails(int testId)
        {
            var testDetails = _testService.GetTestDetails(testId);
            return Ok(testDetails);
        }
    }
}
