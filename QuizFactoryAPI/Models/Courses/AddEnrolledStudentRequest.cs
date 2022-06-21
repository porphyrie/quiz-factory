using System.ComponentModel.DataAnnotations;

namespace QuizFactoryAPI.Models.Courses
{
    public class AddEnrolledStudentRequest
    {
        [Required]
        public int CourseId { get; set; }
        [Required]
        public string StudentUsername { get; set; }
    }
}
