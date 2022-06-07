using System.ComponentModel.DataAnnotations;

namespace QuizFactoryAPI.Models.Categories
{
    public class AddCategoryRequest
    {
        [Required]
        public int SubjectId { get; set; }
        [Required]
        public string CategoryName { get; set; }
    }
}
