using QuizFactoryAPI.Data;
using QuizFactoryAPI.Entities;
using QuizFactoryAPI.Helpers;
using QuizFactoryAPI.Models.Categories;

namespace QuizFactoryAPI.Services
{
    public interface ICategoryService
    {
        void AddCategory(AddCategoryRequest model);
        List<GetCategoryResponse> GetCategories(int subjectId);
    }
    public class CategoryService : ICategoryService
    {
        private QuizFactoryContext _context;

        public CategoryService(QuizFactoryContext context)
        {
            _context = context;
        }

        public void AddCategory(AddCategoryRequest model)
        {
            // validate
            if (_context.Categories.Any(x => x.SubjectId == model.SubjectId && x.CategoryName == model.CategoryName))
                throw new AppException("Category '" + model.CategoryName + "' already exists");

            // map model to new object
            var category = new Category()
            {
                SubjectId = model.SubjectId,
                CategoryName = model.CategoryName
            };

            // save 
            _context.Categories.Add(category);
            _context.SaveChanges();
        }

        public List<GetCategoryResponse> GetCategories(int subjectId)
        {
            var categories = _context.Categories.Where(x => x.SubjectId == subjectId).Select(x => new GetCategoryResponse(x.Id, x.CategoryName)).ToList();
            return categories;
        }
    }
}
