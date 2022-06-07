using System.ComponentModel.DataAnnotations;

namespace QuizFactoryAPI.Models.Subjects
{
    public class AddSubjectRequest
    {
        [Required]
        public string SubjectName { get; set; }
    }
}
