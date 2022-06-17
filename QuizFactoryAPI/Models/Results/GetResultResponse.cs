namespace QuizFactoryAPI.Models.Results
{
    public class GetResultResponse
    {
        public class QuestionAnswer
        {
            public string Question;
            public string Answer;
            public string CorrectAnswer;

            public QuestionAnswer(string question, string answer, string correctAnswer)
            {
                Question = question;
                Answer = answer;
                CorrectAnswer = correctAnswer;
            }
        }
        public string Username { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public float Grade { get; set; }
        public string TestName { get; set; }
        public DateTime TestDate { get; set; }
        public int TestDuration { get; set; }
        public int ItemCount { get; set; }

        public List<QuestionAnswer> Answers { get; set; }
        public GetResultResponse(string username, string lastName, string firstName, float grade, string testName, DateTime testDate, int testDuration, int itemCount, List<QuestionAnswer> answers)
        {
            Username = username;
            LastName = lastName;
            FirstName = firstName;
            Grade = grade;
            TestName = testName;
            TestDate = testDate;
            TestDuration = testDuration;
            ItemCount = itemCount;
            Answers = answers;
        }
    }
}
