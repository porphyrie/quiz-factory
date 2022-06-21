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
        List<GetStdCourseResponse> GetAllCourses();
        List<GetStdCourseResponse> GetEnrolledCourses(string studentUsername);

        void AddEnrolledStudent(AddEnrolledStudentRequest model);
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
                .Select(x => new { x.Id, x.CourseName, x.StudentUsernames }).ToList();

            List<GetCourseResponse> coursesRes = new List<GetCourseResponse>();

            foreach (var course in courses)
            {
                var participants = course.StudentUsernames.Select(user => new GetCourseResponse.Participant()
                {
                    Username = user.Username,
                    LastName = user.LastName,
                    FirstName = user.FirstName
                }).ToList();

                coursesRes.Add(new GetCourseResponse(course.Id, course.CourseName, participants, participants.Count));
            }

            return coursesRes;
        }

        public List<GetStdCourseResponse> GetAllCourses()
        {
            var courses = _context.Courses
                .Select(x => new GetStdCourseResponse(x.Id, x.CourseName)).ToList();

            return courses;
        }

        public List<GetStdCourseResponse> GetEnrolledCourses(string studentUsername)
        {
            var user = _context.Users.FirstOrDefault(x => x.Username == studentUsername);

            _context.Entry<User>(user).Collection(u => u.CoursesNavigation).Load();

            var courses = user.CoursesNavigation
                .Select(x => new GetStdCourseResponse( x.Id, x.CourseName)).ToList();

            return courses;
        }

        public void AddEnrolledStudent(AddEnrolledStudentRequest model)
        {
            var course = _context.Courses.FirstOrDefault(x => x.Id == model.CourseId);
            var student = _context.Users.FirstOrDefault(x => x.Username == model.StudentUsername);

            _context.Entry<Course>(course).Collection(c => c.StudentUsernames).Load();

            // validate
            if (course.StudentUsernames.FirstOrDefault(x=>x.Username == model.StudentUsername) == student)
                throw new AppException("Student '" + model.StudentUsername + "' has already been enrolled");

            // save course
            course.StudentUsernames.Add(student);
            _context.SaveChanges();
        }
    }
}
