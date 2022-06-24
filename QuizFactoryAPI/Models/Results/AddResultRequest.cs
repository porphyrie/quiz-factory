using System.ComponentModel.DataAnnotations;

namespace QuizFactoryAPI.Models.Results
{
    public class AddResultRequest
    {
        [Required]
        public int TestId { get; set; }
        [Required]
        public string StudentUsername { get; set; }
    }
}
