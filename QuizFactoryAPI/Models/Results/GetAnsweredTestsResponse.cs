namespace QuizFactoryAPI.Models.Results
{
    public class GetAnsweredTestsResponse
    {
        public List<int> AnsweredTests { get; set; }
        public GetAnsweredTestsResponse(List<int> answeredTests)
        {
            AnsweredTests = answeredTests;
        }
    }
}
