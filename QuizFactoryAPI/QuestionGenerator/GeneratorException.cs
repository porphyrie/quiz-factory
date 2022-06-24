using System;

namespace QuizFactoryAPI.QuestionGenerator
{
    public class GeneratorException : Exception
    {
        public GeneratorException()
        {
        }

        public GeneratorException(string message)
            : base(message)
        {
        }

        public GeneratorException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
