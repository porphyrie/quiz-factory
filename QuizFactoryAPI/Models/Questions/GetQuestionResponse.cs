namespace QuizFactoryAPI.Models.Questions
{
    public class GetQuestionResponse
    {
        public int QuestionId { get; set; }
        public string QuestionTemplateString { get; set; }

        public GetQuestionResponse (int questionId, string questionTemplateString)
        {
            QuestionId = questionId;
            QuestionTemplateString = questionTemplateString;
        }
    }
}
