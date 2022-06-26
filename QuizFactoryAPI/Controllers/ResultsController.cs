using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using QuizFactoryAPI.Authorization;
using QuizFactoryAPI.Data;
using QuizFactoryAPI.Entities;
using QuizFactoryAPI.Models.Results;
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

        [Authorize(Role.student)]
        [HttpPost]
        public IActionResult AddResultEntry(AddResultRequest model)
        {
            var resultId = _resultService.AddResultEntry(model);
            return Ok(resultId);
        }

        [Authorize(Role.student)]
        [HttpPatch("details/{resultDetailsId}")]
        public IActionResult UpdateResultDetailsEntry(int resultDetailsId, JsonPatchDocument<ResultDetail> patchAnswer)
        {
            _resultService.UpdateResultDetails(resultDetailsId, patchAnswer);
            return Ok(new { message = "The result details have been updated" });
        }

        [Authorize(Role.student)]
        [HttpPatch("{resultId}")]
        public IActionResult UpdateResultEntry(int resultId, JsonPatchDocument<Result> patchFinishTime)
        {
            _resultService.UpdateResult(resultId, patchFinishTime);
            return Ok(new { message = "The result has been updated" });
        }

        [Authorize(Role.student)]
        [HttpGet("answeredtests/{username}")]
        public IActionResult GetAnsweredTests(string username)
        {
            var answeredTests = _resultService.GetAnsweredTests(username);
            return Ok(answeredTests);
        }
    }
}
