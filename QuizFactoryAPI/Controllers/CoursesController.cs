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
using QuizFactoryAPI.Models.Courses;
using QuizFactoryAPI.Services;

namespace QuizFactoryAPI.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private ICourseService _courseService;

        public CoursesController(ICourseService courseService)
        {
            _courseService = courseService;
        }

        [Authorize(Role.profesor)]
        [HttpPost]
        public IActionResult AddCourse(AddCourseRequest model)
        {
            _courseService.AddCourse(model);
            return Ok(new { message = "The course has been created" });
        }

        [Authorize(Role.student)]
        [HttpPost("enrollment")]
        public IActionResult AddEnrolledStudent(AddEnrolledStudentRequest model)
        {
            _courseService.AddEnrolledStudent(model);
            return Ok(new { message = "The student has been enrolled" });
        }

        [Authorize(Role.profesor, Role.student)]
        [HttpGet("{username}")]
        public IActionResult GetCourses(string username)
        {
            var currentUser = (User)HttpContext.Items["User"];
            var role = currentUser.Role.ToString();

            if (role == Role.profesor.ToString())
            {
                var courses = _courseService.GetCourses(username);
                return Ok(courses);
            }
            else
            {
                var courses = _courseService.GetEnrolledCourses(username);
                return Ok(courses);
            }
        }

        [Authorize(Role.profesor, Role.student)]
        [HttpGet]
        public IActionResult GetAllCourses()
        {
            var courses = _courseService.GetAllCourses();
            return Ok(courses);
        }

        //// GET: api/Courses
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        //{
        //  if (_context.Courses == null)
        //  {
        //      return NotFound();
        //  }
        //    return await _context.Courses.ToListAsync();
        //}

        //// GET: api/Courses/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Course>> GetCourse(int id)
        //{
        //  if (_context.Courses == null)
        //  {
        //      return NotFound();
        //  }
        //    var course = await _context.Courses.FindAsync(id);

        //    if (course == null)
        //    {
        //        return NotFound();
        //    }

        //    return course;
        //}

        //// PUT: api/Courses/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutCourse(int id, Course course)
        //{
        //    if (id != course.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(course).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CourseExists(id))
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

        //// POST: api/Courses
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<Course>> PostCourse(Course course)
        //{
        //  if (_context.Courses == null)
        //  {
        //      return Problem("Entity set 'QuizFactoryContext.Courses'  is null.");
        //  }
        //    _context.Courses.Add(course);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetCourse", new { id = course.Id }, course);
        //}

        //// DELETE: api/Courses/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteCourse(int id)
        //{
        //    if (_context.Courses == null)
        //    {
        //        return NotFound();
        //    }
        //    var course = await _context.Courses.FindAsync(id);
        //    if (course == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Courses.Remove(course);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool CourseExists(int id)
        //{
        //    return (_context.Courses?.Any(e => e.Id == id)).GetValueOrDefault();
        //}
    }
}
