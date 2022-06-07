using System.ComponentModel.DataAnnotations;

namespace QuizFactoryAPI.Models.Questions
{
    public class AddQuestionRequest
    {
        [Required]
        public int SubjectId { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public string QuestionTemplateString { get; set; }
        [Required]
        public string ConfigurationFile { get; set; }
        [Required]
        public string ProducingFile { get; set; }
        public string? GrammarFile { get; set; }
    }
}
