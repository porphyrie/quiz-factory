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
using QuizFactoryAPI.Models.Questions;
using QuizFactoryAPI.Services;

namespace QuizFactoryAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private IQuestionService _questionService;

        public QuestionsController(IQuestionService questionService)
        {
            _questionService = questionService;
        }

        [Authorize(Role.admin)]
        [HttpPost]
        public IActionResult AddQuestion(AddQuestionRequest model)
        {
            _questionService.AddQuestion(model);
            return Ok(new { message = "The question has been added" });
        }

        [Authorize(Role.admin, Role.profesor)]
        [HttpGet]
        public IActionResult GetQuestions([FromQuery(Name = "subjectId")] int subjectId, [FromQuery(Name = "categoryId")] int categoryId)
        {
            var questions = _questionService.GetQuestions(subjectId, categoryId);
            return Ok(questions);
        }

        [Authorize(Role.student)]
        [HttpGet("generate")]
        public IActionResult GenerateQuestion([FromQuery(Name = "testId")] int testId, [FromQuery(Name = "username")] string username, [FromQuery(Name = "questionTypeId")] int questionTypeId)
        {
            var question = _questionService.GenerateQuestion(testId, username, questionTypeId);
            return Ok(question);
        }

    }
}
