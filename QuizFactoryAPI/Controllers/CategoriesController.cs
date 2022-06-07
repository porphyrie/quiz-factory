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
using QuizFactoryAPI.Models.Categories;
using QuizFactoryAPI.Services;

namespace QuizFactoryAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Authorize(Role.admin)]
        [HttpPost]
        public IActionResult AddCategory(AddCategoryRequest model)
        {
            _categoryService.AddCategory(model);
            return Ok(new { message = "The category has been added" });
        }

        [Authorize(Role.admin)]
        [HttpGet("{subjectId}")]
        public IActionResult GetSubjects(int subjectId)
        {
            var categories = _categoryService.GetCategories(subjectId);
            return Ok(categories);
        }
    }
}
