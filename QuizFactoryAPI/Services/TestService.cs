using QuizFactoryAPI.Data;
using QuizFactoryAPI.Entities;
using QuizFactoryAPI.Models.Tests;

namespace QuizFactoryAPI.Services
{
    public interface ITestService
    {
       void AddTest(AddTestRequest model);
        //List<GetSubjectResponse> GetSubjects();
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

            //foreach (var questionTypeId in model.Questions) {
            //    var testQuestion = new TestQuestionType()
            //    {
            //        QuestionTypeId = questionTypeId,
            //        Test = test
            //    };
            //    _context.TestQuestionTypes.Add(testQuestion);
            //}


        }

        //public List<GetSubjectResponse> GetSubjects()
        //{
        //    var subjects = _context.Subjects.Select(x => new GetSubjectResponse(x.Id, x.SubjectName)).ToList();
        //    return subjects;
        //}
    }
}
