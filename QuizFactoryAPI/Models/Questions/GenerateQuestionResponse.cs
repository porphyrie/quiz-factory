namespace QuizFactoryAPI.Models.Questions
{
    public class GenerateQuestionResponse
    {
        public int ResultId { get; set; }
        public string Question { get; set; }

        public GenerateQuestionResponse(int resultId, string question)
        {
            ResultId = resultId;
            Question = question;
        }
    }
}
