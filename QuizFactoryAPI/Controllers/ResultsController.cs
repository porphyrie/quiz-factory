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
using QuizFactoryAPI.Services;

namespace QuizFactoryAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private IResultService _resultService;

        public ResultsController(IResultService resultService)
        {
            _resultService = resultService;
        }

        [Authorize(Role.profesor, Role.student)]
        [HttpGet]
        public IActionResult GetResult([FromQuery(Name = "username")] string username, [FromQuery(Name = "testId")] int testId)
        {
            var result = _resultService.GetResult(username, testId);
            return Ok(result);
        }
    }
}
