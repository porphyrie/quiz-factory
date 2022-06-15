using QuizFactoryAPI.Data;
using QuizFactoryAPI.Entities;
using QuizFactoryAPI.Helpers;
using QuizFactoryAPI.Models.Courses;
using System.Linq;

namespace QuizFactoryAPI.Services
{
    public interface ICourseService
    {
        void AddCourse(AddCourseRequest model);
        List<GetCourseResponse> GetCourses(string professorUsername);
    }

    public class CourseService : ICourseService
    {
        private QuizFactoryContext _context;

        public CourseService(QuizFactoryContext context)
        {
            _context = context;
        }

        public void AddCourse(AddCourseRequest model)
        {
            // validate
            if (_context.Courses.Any(x => x.CourseName == model.CourseName && x.ProfessorUsername == model.ProfessorUsername))
                throw new AppException("Course '" + model.CourseName + "' is already created");

            // map model to new course object
            var course = new Course()
            {
                CourseName = model.CourseName,
                ProfessorUsername = model.ProfessorUsername,
            };

            // save course
            _context.Courses.Add(course);
            _context.SaveChanges();
        }

        public List<GetCourseResponse> GetCourses(string professorUsername)
        {
            var courses = _context.Courses
                .Where(x => x.ProfessorUsername == professorUsername)
                .Select(x => new { x.Id, x.CourseName }).ToList();

            List<GetCourseResponse> coursesRes = new List<GetCourseResponse>();

            foreach (var course in courses)
            {
                var students = _context.EnrolledStudents
                    .Where(x => x.CourseId == course.Id)
                    .Select(x => new { x.StudentUsername })
                    .Join(_context.Users,
                        enrolledStudent => enrolledStudent.StudentUsername,
                        user => user.Username,
                        (enrolledStudent, user) => new GetCourseResponse.Participant()
                        {
                            Username = enrolledStudent.StudentUsername,
                            LastName = user.LastName,
                            FirstName = user.FirstName
                        }).ToList();

                coursesRes.Add(new GetCourseResponse(course.Id, course.CourseName, students, students.Count));
            }

            return coursesRes;
        }
    }
}
