namespace QuizFactoryAPI.Models.Subjects
{
    public class GetSubjectResponse
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }

        public GetSubjectResponse(int subjectId, string subjectName)
        {
            SubjectId = subjectId;
            SubjectName = subjectName;
        }
    }
}
