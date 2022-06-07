using System.ComponentModel.DataAnnotations;

namespace QuizFactoryAPI.Models.Courses
{
    public class AddCourseRequest
    {
        [Required]
        public string CourseName { get; set; }
        [Required]
        public string ProfessorUsername { get; set; }
    }
}
