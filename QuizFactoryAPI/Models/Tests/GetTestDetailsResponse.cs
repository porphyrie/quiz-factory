namespace QuizFactoryAPI.Models.Tests
{
    public class GetTestDetailsResponse
    {
        public class Result
        {
            public string Username { get; set; }
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public float Grade { get; set; }

            public Result(string username, string lastName, string firstName, float grade)
            {
                Username = username;
                LastName = lastName;
                FirstName = firstName;
                Grade = grade;
            }
        }
        public class QuestionType
        {
            public int Id { get; set; }
            public string TemplateString { get; set; }

            public QuestionType(int id, string templateString)
            {
                Id = id;
                TemplateString = templateString;
            }
        }
        public class Statistics
        {
            public float HighestGrade { get; set; }
            public float LowestGrade { get; set; }
            public float AvgResponseTime { get; set; }
            public List<QuestionType> MostAnsweredQuestions { get; set; }
            public List<QuestionType> LeastAnsweredQuestions { get; set; }

            public Statistics(float highestGrade, float lowestGrade, float avgResponseTime, List<QuestionType> mostAnsweredQuestions, List<QuestionType> leastAnsweredQuestions)
            {
                HighestGrade = highestGrade;
                LowestGrade = lowestGrade;
                AvgResponseTime = avgResponseTime;
                MostAnsweredQuestions = mostAnsweredQuestions;
                LeastAnsweredQuestions = leastAnsweredQuestions;
            }
        }

        public int TestId { get; set; }
        public string TestName { get; set; }
        public DateTime TestDate { get; set; }
        public int TestDuration { get; set; }
        public int TestItemCount { get; set; }
        public List<QuestionType> QuestionTypes { get; set; }
        public List<Result> Results { get; set; }
        public Statistics Stats { get; set; }

        public GetTestDetailsResponse(int testId, string testName, DateTime testDate, int testDuration, List<QuestionType> questionTypes, List<Result> results, Statistics stats)
        {
            TestId = testId;
            TestName = testName;
            TestDate = testDate;
            TestDuration = testDuration;
            QuestionTypes = questionTypes;
            Results = results;
            Stats = stats;
        }
    }
}
