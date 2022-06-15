using QuizFactoryAPI.Data;
using QuizFactoryAPI.Entities;
using QuizFactoryAPI.Helpers;
using QuizFactoryAPI.Models.Questions;

namespace QuizFactoryAPI.Services
{
    public interface IQuestionService
    {
        void AddQuestion(AddQuestionRequest model);

        List<GetQuestionResponse> GetQuestions(int subjectId, int categoryId);
    }
    public class QuestionService : IQuestionService
    {
        private QuizFactoryContext _context;

        public QuestionService(QuizFactoryContext context)
        {
            _context = context;
        }

        public void AddQuestion(AddQuestionRequest model)
        {
            // validate
            if (_context.QuestionTypes.Any(x => x.SubjectId == model.SubjectId && x.CategoryId == model.CategoryId && x.QuestionTemplateString == model.QuestionTemplateString))
                throw new AppException("Question '" + model.QuestionTemplateString + "' already exists");

            // map model to new object
            var question = new QuestionType()
            {
                SubjectId = model.SubjectId,
                CategoryId = model.CategoryId,
                QuestionTemplateString = model.QuestionTemplateString,
                ConfigurationFile = model.ConfigurationFile,
                ProducingFile = model.ProducingFile,
                GrammarFile = model.GrammarFile
            };

            // save 
            _context.QuestionTypes.Add(question);
            _context.SaveChanges();
        }

        public List<GetQuestionResponse> GetQuestions(int subjectId, int categoryId)
        {
            var questions = _context.QuestionTypes.Where(x => x.SubjectId == subjectId && x.CategoryId == categoryId).Select(x => new GetQuestionResponse(x.Id, x.QuestionTemplateString)).ToList();
            return questions;
        }
    }
}
