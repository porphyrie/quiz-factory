namespace QuizFactoryAPI.Models.Tests
{
    public class GetTestResponse
    {
        public int CourseId { get; set; }
        public int TestId { get; set; }
        public string TestName { get; set; }
        public DateTime TestDate { get; set; }
        public int TestDuration { get; set; }
        public int ItemCount { get; set; }
        public GetTestResponse(int courseId, int testId, string testName, DateTime testDate, int testDuration, int itemCount)
        {
            CourseId = courseId;
            TestId = testId;
            TestName = testName;
            TestDate = testDate;
            TestDuration = testDuration;
            ItemCount = itemCount;
        }
    }
}
