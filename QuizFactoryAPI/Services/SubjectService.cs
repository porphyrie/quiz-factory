using QuizFactoryAPI.Data;
using QuizFactoryAPI.Entities;
using QuizFactoryAPI.Helpers;
using QuizFactoryAPI.Models.Subjects;

namespace QuizFactoryAPI.Services
{
    public interface ISubjectService
    {
        void AddSubject(AddSubjectRequest model);
        List<GetSubjectResponse> GetSubjects();
    }
    public class SubjectService : ISubjectService
    {
        private QuizFactoryContext _context;

        public SubjectService(QuizFactoryContext context)
        {
            _context = context;
        }

        public void AddSubject(AddSubjectRequest model)
        {
            // validate
            if (_context.Subjects.Any(x => x.SubjectName == model.SubjectName))
                throw new AppException("Subject '" + model.SubjectName + "' already exists");

            // map model to new object
            var subject = new Subject()
            {
                SubjectName = model.SubjectName,
            };

            // save 
            _context.Subjects.Add(subject);
            _context.SaveChanges();
        }

        public List<GetSubjectResponse> GetSubjects()
        {
            var subjects = _context.Subjects.Select(x => new GetSubjectResponse(x.Id, x.SubjectName)).ToList();
            return subjects;
        }
    }
}
