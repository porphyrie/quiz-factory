using Microsoft.EntityFrameworkCore;
using QuizFactoryAPI.Data;
using QuizFactoryAPI.Entities;
using QuizFactoryAPI.Models.Tests;

namespace QuizFactoryAPI.Services
{
    public interface ITestService
    {
        void AddTest(AddTestRequest model);
        List<GetTestResponse> GetTests(string username, string role);
        GetTestDetailsResponse GetTestDetails(int testId);
        GetTestSummaryResponse GetTestSummary(int testId);
    }
    public class TestService : ITestService
    {
        private QuizFactoryContext _context;

        public TestService(QuizFactoryContext context)
        {
            _context = context;
        }

        public void AddTest(AddTestRequest model)
        {
            // map model to new object
            var test = new Test()
            {
                CourseId = model.CourseId,
                TestName = model.TestName,
                TestDate = model.TestDate,
                TestDuration = model.TestDuration,

            };

            // save 
            _context.Tests.Add(test);
            _context.SaveChanges();

            foreach (var questionTypeId in model.Questions)
            {
                var testQuestion = new TestQuestionType()
                {
                    TestId = test.Id,
                    QuestionTypeId = questionTypeId,
                };
                _context.TestQuestionTypes.Add(testQuestion);
            }

            // save 
            _context.SaveChanges();
        }

        public List<GetTestResponse> GetTests(string username, string role)
        {
            List<Course> courses = new List<Course>();

            if (role == Role.profesor.ToString())
                courses = _context.Courses.Where(x => x.ProfessorUsername == username).ToList();
            else if (role == Role.student.ToString())
            {
                var user = _context.Users.FirstOrDefault(x => x.Username == username);
                _context.Entry<User>(user).Collection(u => u.CoursesNavigation).Load();
                courses = user.CoursesNavigation.ToList();
            }

            List<GetTestResponse> tests = new List<GetTestResponse>();

            foreach (var course in courses)
            {
                var courseTests = _context.Tests.Where(x => x.CourseId == course.Id).Select(x => new GetTestResponse(x.CourseId, course.CourseName, x.Id, x.TestName, x.TestDate, x.TestDuration, x.TestQuestionTypes.Count));
                tests.AddRange(courseTests);
            }

            return tests;
        }

        public GetTestDetailsResponse GetTestDetails(int testId)
        {
            var test = _context.Tests.Include(t => t.TestQuestionTypes).ThenInclude(qt => qt.QuestionType).Include(t => t.Results).ThenInclude(r=>r.ResultDetails).FirstOrDefault(x => x.Id == testId);

            var questionTypes = test.TestQuestionTypes.Select(x => new GetTestDetailsResponse.QuestionType(x.QuestionTypeId, x.QuestionType.QuestionTemplateString)).ToList();

            var results = test.Results.ToList();

            List<GetTestDetailsResponse.Result> stdResults;
            GetTestDetailsResponse.Statistics stats;

            if (results.Count > 0)
            {
                var initCorrectAnswers = new int[questionTypes.Count];
                var correctAnswers = new int[questionTypes.Count];

                stdResults = new List<GetTestDetailsResponse.Result>();

                foreach (var result in results)
                {
                    var resultDetails = result.ResultDetails.ToList();
                    var correctItems = 0;
                    foreach (var item in resultDetails)
                        if (item.Answer == item.CorrectAnswer)
                        {
                            correctItems++;
                            var idx = questionTypes.FindIndex(x => x.Id == item.QuestionTypeId);
                            correctAnswers[idx]++;
                        }
                    float grade = (float)result.Grade;

                    var studentData = _context.Users.FirstOrDefault(x => x.Username == result.StudentUsername);

                    var stdResult = new GetTestDetailsResponse.Result(studentData.Username, studentData.LastName, studentData.FirstName, grade);
                    stdResults.Add(stdResult);
                }

                var highestGrade = (float)results.Max(x => x.Grade);
                var lowestGrade = (float)results.Min(x => x.Grade);
                var avgResponseTime = (float)results.Average(x => (x.FinishTime - test.TestDate).Value.TotalSeconds/60);
                var maxQuestionsData = correctAnswers.Select((x, idx) => new { correctAnswersCount = x, index = idx });
                var maxQuestionTypeIdx = maxQuestionsData.Where((x, i) => x.correctAnswersCount == correctAnswers.Except(initCorrectAnswers).Max()).Select(x=>x.index);
                var maxQuestionTypes = questionTypes.Where((x, idx) => maxQuestionTypeIdx.Contains(idx)).ToList();
                var minQuestionData = correctAnswers.Select((x, idx) => new { correctAnswersCount = x, index = idx });
                var minQuestionTypeIdx = minQuestionData.Where((x,i) => x.correctAnswersCount == correctAnswers.Except(initCorrectAnswers).Min()).Select(x=>x.index);
                var minQuestionTypes = questionTypes.Where((x, idx) => minQuestionTypeIdx.Contains(idx)).ToList();

                stats = new GetTestDetailsResponse.Statistics(highestGrade, lowestGrade, avgResponseTime, maxQuestionTypes, minQuestionTypes);
            }
            else
            {
                stdResults = new List<GetTestDetailsResponse.Result>();
                stats = new GetTestDetailsResponse.Statistics();
            }

            return new GetTestDetailsResponse(test.Id, test.TestName, test.TestDate, test.TestDuration, questionTypes.Count, questionTypes, stdResults, stats);
        }

        public GetTestSummaryResponse GetTestSummary(int testId)
        {
            var test = _context.Tests.Include(t => t.TestQuestionTypes).ThenInclude(qt => qt.QuestionType).FirstOrDefault(x => x.Id == testId);
            var questionTypes = test.TestQuestionTypes.Select(x => new GetTestSummaryResponse.QuestionType(x.QuestionTypeId, x.QuestionType.QuestionTemplateString)).ToList();
            return new GetTestSummaryResponse(test.Id, test.TestName, test.TestDate, test.TestDuration, questionTypes.Count, questionTypes);
        }
    }
}
