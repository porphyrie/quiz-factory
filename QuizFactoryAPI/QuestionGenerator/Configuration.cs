using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace QuizFactoryAPI.QuestionGenerator
{
    public class Configuration
    {
        private int minLineCount;
        private int maxLineCount;
        private float minUniqueness;
        //private string templatePath;
        private string customizedTemplatePath;
        private string executablePath;
        private string question;
        private string patternToReplaceInQuestion;
        private string patternToDetectHypothesisContent;
        private string patternToDetectCorrectAnswer;
        //private string grammarPath;
        private string startSymbol;
        //private Dictionary<string, string> clangformat;
        //private Dictionary<string, string> gcompiler;

        public Configuration(string configJSON)
        {
            var configuration = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(configJSON);

            try
            {
                Dictionary<string, string> generatorParameters = configuration["parameters"]["generator"];

                minLineCount = (Int32.TryParse(generatorParameters["minLineCount"], out minLineCount) ? minLineCount : 0);
                maxLineCount = (Int32.TryParse(generatorParameters["maxLineCount"], out maxLineCount) ? maxLineCount : 100);
                minUniqueness = (float.TryParse(generatorParameters["minUniqueness"], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out minUniqueness) ? minUniqueness : 0);

                //if (!generatorParameters["templatePath"].Equals(""))
                //    templatePath = generatorParameters["templatePath"];
                //else
                //    throw new Exception("The template path is empty!");

                if (!generatorParameters["question"].Equals(""))
                    question = generatorParameters["question"];
                else
                    throw new Exception("The question is empty!");

                if (!generatorParameters["patternToReplaceInQuestion"].Equals(""))
                    patternToReplaceInQuestion = generatorParameters["patternToReplaceInQuestion"];
                else
                    throw new Exception("The pattern to replace is empty!");

                if (!generatorParameters["patternToDetectHypothesisContent"].Equals(""))
                    patternToDetectHypothesisContent = generatorParameters["patternToDetectHypothesisContent"];
                else
                    throw new Exception("The hypothesis pattern is empty!");

                if (!generatorParameters["patternToDetectCorrectAnswer"].Equals(""))
                    patternToDetectCorrectAnswer = generatorParameters["patternToDetectCorrectAnswer"];
                else
                    throw new Exception("The answer pattern is empty!");

                Dictionary<string, string> syntaxTreeParameters = configuration["parameters"]["syntaxTree"];

                //if (!syntaxTreeParameters["grammarPath"].Equals(""))
                //    grammarPath = syntaxTreeParameters["grammarPath"];
                //else
                //    throw new Exception("The grammar path is empty!");

                //if (!syntaxTreeParameters["startSymbol"].Equals(""))
                    startSymbol = syntaxTreeParameters["startSymbol"];
                //else
                    //throw new Exception("The start symbol is empty!");


                //this.clangformat = new Dictionary<string, string>();

                //Dictionary<string, string> clangformat = configuration["dependencies"]["clang-format"];
                //this.clangformat["path"] = clangformat["path"];
                //this.clangformat["args"] = clangformat["args"];

                //this.gcompiler = new Dictionary<string, string>();

                //Dictionary<string, string> gcompiler = configuration["dependencies"]["g++"];
                //this.gcompiler["path"] = gcompiler["path"];
                //this.gcompiler["args"] = gcompiler["args"];
            }
            catch (KeyNotFoundException keyNotFoundException)
            {
                throw new GeneratorException("The configuration is not correct. Check if the keys match the basic template configuration.");
            }
        }

        public int GetMinLineCount()
        {
            return minLineCount;
        }

        public int GetMaxLineCount()
        {
            return maxLineCount;
        }

        public float GetMinUniqueness()
        {
            return minUniqueness;
        }

        //public string GetGrammarPath()
        //{
        //    return grammarPath;
        //}

        //public string GetTemplatePath()
        //{
        //    return templatePath;
        //}

        //public string GetClangFormatPath()
        //{
        //    return clangformat["path"];
        //}

        //public string GetClangFormatArgs()
        //{
        //    return clangformat["args"];
        //}

        //public string GetGCompilerPath()
        //{
        //    return gcompiler["path"];
        //}

        //public string GetGCompilerArgs()
        //{
        //    return gcompiler["args"];
        //}

        public void SetCustomizedTemplatePath(string customizedTemplatePath)
        {
            this.customizedTemplatePath = customizedTemplatePath;
        }

        public void SetExecutablePath(string executablePath)
        {
            this.executablePath = executablePath;
        }

        public string GetCustomizedTemplatePath()
        {
            return customizedTemplatePath;
        }

        public string GetExecutablePath()
        {
            return executablePath;
        }

        public string GetQuestion()
        {
            return question;
        }

        public string GetPatternToDetectHypothesisContent()
        {
            return patternToDetectHypothesisContent;
        }

        public string GetPatternToDetectCorrectAnswer()
        {
            return patternToDetectCorrectAnswer;
        }

        public string GetStartSymbol()
        {
            return startSymbol;
        }

        public string GetPatternToReplaceInQuestion()
        {
            return patternToReplaceInQuestion;
        }

    }
}
