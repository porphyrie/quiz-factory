using QuizFactoryAPI.Data;
using QuizFactoryAPI.Models.Results;

namespace QuizFactoryAPI.Services
{
    public interface IResultService
    {
        void AddResult(AddResultRequest model);
        GetResultResponse GetResult(string username, int testId);
    }
    public class ResultService : IResultService
    {
        private QuizFactoryContext _context;

        public ResultService(QuizFactoryContext context)
        {
            _context = context;
        }

        public void AddResult(AddResultRequest model)
        {
            //// validate
            //if (_context.Subjects.Any(x => x.SubjectName == model.SubjectName))
            //    throw new AppException("Subject '" + model.SubjectName + "' already exists");

            //// map model to new object
            //var subject = new Subject()
            //{
            //    SubjectName = model.SubjectName,
            //};

            //// save 
            //_context.Subjects.Add(subject);
            //_context.SaveChanges();
        }
        public GetResultResponse GetResult(string username, int testId)
        {
            var userData = _context.Users.FirstOrDefault(x => x.Username == username);

            var test = _context.Tests.FirstOrDefault(x => x.Id == testId);

            var grade = (float)test.Results.First(x => x.StudentUsername == username).Grade;

            var questions = test.Results.First(x => x.StudentUsername == username).ResultDetails.Select(x => new GetResultResponse.QuestionAnswer(x.Question, x.CorrectAnswer, x.Answer)).ToList();

            return new GetResultResponse(userData.Username, userData.LastName, userData.FirstName, grade, test.TestName, test.TestDate, test.TestDuration, test.TestQuestionTypes.Count, questions);
        }
    }
}
