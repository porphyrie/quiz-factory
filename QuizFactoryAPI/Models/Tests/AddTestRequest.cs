using System.ComponentModel.DataAnnotations;

namespace QuizFactoryAPI.Models.Tests
{
    public class AddTestRequest
    {
        [Required]
        public int CourseId { get; set; }
        [Required]
        public string TestName { get; set; }
        [Required]
        public DateTime TestDate { get; set; }
        [Required]
        public int TestDuration { get; set; }
        [Required]
        public int[] Questions  { get; set; }
    }
}
