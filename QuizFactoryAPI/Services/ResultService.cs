using QuizFactoryAPI.Data;
using QuizFactoryAPI.Models.Results;

namespace QuizFactoryAPI.Services
{
    public interface IResultService
    {
        void AddResult(AddResultRequest model);
        //List<GetSubjectResponse> GetSubjects();
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

        //public List<GetSubjectResponse> GetSubjects()
        //{
        //    var subjects = _context.Subjects.Select(x => new GetSubjectResponse(x.Id, x.SubjectName)).ToList();
        //    return subjects;
        //}
    }
}
