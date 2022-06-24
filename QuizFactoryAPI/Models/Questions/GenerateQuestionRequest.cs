using System.ComponentModel.DataAnnotations;

namespace QuizFactoryAPI.Models.Questions
{
    public class GenerateQuestionRequest
    {
        [Required]
        public int TestId { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public int QuestionTypeId { get; set; }
    }
}
