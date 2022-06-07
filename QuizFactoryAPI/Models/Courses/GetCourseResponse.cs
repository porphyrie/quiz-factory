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
        public string CourseName { get; set; }
        public List<Participant> Participants { get; set; }
        public int ParticipantsCount { get; set; }

        public GetCourseResponse(string courseName, List<Participant> participants, int participantsCount)
        {
            CourseName = courseName;
            Participants = participants;
            ParticipantsCount = participantsCount;
        }
    }
}
