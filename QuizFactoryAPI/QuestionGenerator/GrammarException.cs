using System;

namespace QuizFactoryAPI.QuestionGenerator
{
    public class GrammarException : Exception
    {
        public GrammarException()
        {
        }

        public GrammarException(string message)
            : base(message)
        {
        }

        public GrammarException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
