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
using QuizFactoryAPI.Models.Subjects;
using QuizFactoryAPI.Services;

namespace QuizFactoryAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class SubjectsController : ControllerBase
    {
        private ISubjectService _subjectService;

        public SubjectsController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [Authorize(Role.admin)]
        [HttpPost]
        public IActionResult AddSubject(AddSubjectRequest model)
        {
            _subjectService.AddSubject(model);
            return Ok(new { message = "The subject has been added" });
        }

        [Authorize(Role.admin, Role.profesor)]
        [HttpGet]
        public IActionResult GetSubjects()
        {
            var subjects = _subjectService.GetSubjects();
            return Ok(subjects);
        }
    }
}
