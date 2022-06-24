using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizFactoryAPI.Data;
using QuizFactoryAPI.Entities;
using QuizFactoryAPI.Helpers;
using QuizFactoryAPI.Models.Results;
using System.Web.Mvc;

namespace QuizFactoryAPI.Services
{
    public interface IResultService
    {
        AddResultResponse AddResultEntry(AddResultRequest model);
        GetResultResponse GetResult(string username, int testId);
        void UpdateResultDetails(int resultId, JsonPatchDocument<ResultDetail> patchAnswer);
        void UpdateResult(int resultId, JsonPatchDocument<Result> patchAnswer);
    }
    public class ResultService : IResultService
    {
        private QuizFactoryContext _context;

        public ResultService(QuizFactoryContext context)
        {
            _context = context;
        }

        public AddResultResponse AddResultEntry(AddResultRequest model)
        {
            // validate
            var existingResult = _context.Results.Include(r => r.ResultDetails).FirstOrDefault(x => x.StudentUsername == model.StudentUsername && x.TestId == model.TestId);
            if (existingResult != null)
            {
                //throw new AppException("Student '" + username + "' already started the test");
                existingResult.ResultDetails.ToList().ForEach(rd => _context.ResultDetails.Remove(rd));
                _context.Results.Remove(existingResult);
                _context.SaveChanges();
            }

            // map model to new object
            var result = new Result()
                {
                    StudentUsername = model.StudentUsername,
                    TestId = model.TestId
                };

            // save 
            _context.Results.Add(result);
            _context.SaveChanges();

            return new AddResultResponse(result.Id);
        }

        public GetResultResponse GetResult(string username, int testId)
        {
            var userData = _context.Users.FirstOrDefault(x => x.Username == username);

            var test = _context.Tests.FirstOrDefault(x => x.Id == testId);

            var grade = (float)test.Results.First(x => x.StudentUsername == username).Grade;

            var questions = test.Results.First(x => x.StudentUsername == username).ResultDetails.Select(x => new GetResultResponse.QuestionAnswer(x.Question, x.CorrectAnswer, x.Answer)).ToList();

            return new GetResultResponse(userData.Username, userData.LastName, userData.FirstName, grade, test.TestName, test.TestDate, test.TestDuration, test.TestQuestionTypes.Count, questions);
        }

        public void UpdateResultDetails(int resultId, JsonPatchDocument<ResultDetail> patchAnswer)
        {
            //if (patch)
            var resultDetails = _context.ResultDetails.FirstOrDefault(x => x.Id == resultId);
            patchAnswer.ApplyTo(resultDetails);
            _context.SaveChanges();
        }

        public void UpdateResult(int resultId, JsonPatchDocument<Result> patchAnswer)
        {
            var result = _context.Results.Include(r => r.ResultDetails).FirstOrDefault(x => x.Id == resultId);
            patchAnswer.ApplyTo(result);
            _context.SaveChanges();

            var itemCount = _context.Tests.Include(t => t.TestQuestionTypes).FirstOrDefault(x => x.Id == result.TestId).TestQuestionTypes.Count();
            var correctAnswers = result.ResultDetails.Where(x => x.CorrectAnswer == x.Answer).Count();

            result.Grade=(float)correctAnswers/itemCount;

            _context.SaveChanges();
        }
    }
}
