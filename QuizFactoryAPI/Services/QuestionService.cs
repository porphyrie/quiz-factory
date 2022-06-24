using QuizFactoryAPI.Data;
using QuizFactoryAPI.Entities;
using QuizFactoryAPI.Helpers;
using QuizFactoryAPI.Models.Questions;
using QuizFactoryAPI.QuestionGenerator;

namespace QuizFactoryAPI.Services
{
    public interface IQuestionService
    {
        void AddQuestion(AddQuestionRequest model);
        List<GetQuestionResponse> GetQuestions(int subjectId, int categoryId);
        GenerateQuestionResponse GenerateQuestion(int testId, string username, int questionTypeId);
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

        public GenerateQuestionResponse GenerateQuestion(int testId, string username, int questionTypeId)
        {
            var questionType = _context.QuestionTypes.FirstOrDefault(x => x.Id == questionTypeId);
            var configJSON = questionType.ConfigurationFile;
            var template = questionType.ProducingFile;
            var grammarJSON = "";
            if(questionType.GrammarFile != null)
                grammarJSON = questionType.GrammarFile;

            var globalConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var currTime = DateTime.Now.Ticks.ToString();
            string customizedTemplatePath = globalConfig["Outputs:CustomizedTemplates"]+currTime+".cpp";
            string executablePath = globalConfig["Outputs:Executables"]+currTime+".exe";

            string question, programCode, answer;

            var generator = new Generator(configJSON, template, customizedTemplatePath, executablePath);
            (question, programCode, answer) = generator.Generate(grammarJSON);

            File.Delete(customizedTemplatePath);
            File.Delete(executablePath);

            if(questionType.GrammarFile != null) 
                question = question + "\n" + programCode;

            var result = _context.Results.FirstOrDefault(x => x.TestId == testId && x.StudentUsername == username);

            ResultDetail resultDetails = new ResultDetail{
                ResultId = result.Id,
                QuestionTypeId = questionTypeId,
                Question = question,
                CorrectAnswer = answer
            };

            _context.ResultDetails.Add(resultDetails);
            _context.SaveChanges();

            return new GenerateQuestionResponse(resultDetails.Id, question);
        }
    }
}
