namespace QuizFactoryAPI.Models.Tests
{
    public class GetTestSummaryResponse
    {
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

        public int TestId { get; set; }
        public string TestName { get; set; }
        public DateTime TestDate { get; set; }
        public int TestDuration { get; set; }
        public int ItemCount { get; set; }
        public List<QuestionType> QuestionTypes { get; set; }

        public GetTestSummaryResponse(int testId, string testName, DateTime testDate, int testDuration, int itemCount, List<QuestionType> questionTypes)
        {
            TestId = testId;
            TestName = testName;
            TestDate = testDate;
            TestDuration = testDuration;
            ItemCount = itemCount;
            QuestionTypes = questionTypes;
        }
    }
}
