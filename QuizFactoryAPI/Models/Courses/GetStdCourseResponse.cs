namespace QuizFactoryAPI.Models.Courses
    {
        public class GetStdCourseResponse
        {
            public int CourseId { get; set; }
            public string CourseName { get; set; }

            public GetStdCourseResponse(int courseId, string courseName)
            {
                CourseId = courseId;
                CourseName = courseName;
            }
        }
    }
