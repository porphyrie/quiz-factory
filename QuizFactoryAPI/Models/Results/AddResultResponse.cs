namespace QuizFactoryAPI.Models.Results
{
    public class AddResultResponse
    {
        public int ResultId { get; set; }

        public AddResultResponse(int resultId)
        {
            ResultId = resultId;
        }
    }
}
