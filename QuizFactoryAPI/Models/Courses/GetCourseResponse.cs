namespace QuizFactoryAPI.Models.Courses
{
    public class GetCourseResponse
    {
        public class Participant
        {
            public string Username;
            public string LastName;
            public string FirstName;
        }
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public List<Participant> Participants { get; set; }
        public int ParticipantsCount { get; set; }

        public GetCourseResponse(int courseId, string courseName, List<Participant> participants, int participantsCount)
        {
            CourseId = courseId;
            CourseName = courseName;
            Participants = participants;
            ParticipantsCount = participantsCount;
        }
    }
}
