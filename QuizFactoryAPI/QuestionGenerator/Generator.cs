using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace QuizFactoryAPI.QuestionGenerator
{
    public class Generator
    {
        private readonly IConfiguration globalConfig;
        private Configuration configuration;
        private string template;

        public Generator(string configJSON, string template, string customizedTemplatePath, string executablePath)
        {
            globalConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            int ok = 0;
            StringReader sr = new StringReader(template);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if (line.Contains("main"))
                {
                    ok = 1;
                    break;
                }
            }
            if (ok == 0)
                throw new GeneratorException("Template file does not contain a main function.");


            configuration = new Configuration(configJSON);
            configuration.SetCustomizedTemplatePath(customizedTemplatePath);
            configuration.SetExecutablePath(executablePath);
            this.template = template;
        }

        private bool checkLineCount(List<string> programCodeLines)
        {
            if ((programCodeLines.Count > configuration.GetMinLineCount()) && (programCodeLines.Count < configuration.GetMaxLineCount()))
                return true;
            else
                return false;
        }

        private bool checkUniqueness(List<string> programCodeLines)
        {
            for (int i = 0; i < programCodeLines.Count; i++)
                programCodeLines[i] = programCodeLines[i].TrimStart();
            //programCodeLines.RemoveAll(line => line.Equals("{"));
            //programCodeLines.RemoveAll(line => line.Equals("}"));
            //programCodeLines.RemoveAll(line => line.Contains("else"));
            //programCodeLines.Remove(programCodeLines[0]);

            int lineCount = programCodeLines.Count;
            int uniqueLineCount = programCodeLines.Distinct().ToList().Count();
            float uniqueness = (float)uniqueLineCount / lineCount;

            if (uniqueness > configuration.GetMinUniqueness())
                return true;
            else
                return false;
        }

        private bool checkFunction(string programCode)
        {
            List<string> programCodeLines = programCode.TrimEnd().Split('\n').ToList();

            if (checkLineCount(programCodeLines) && checkUniqueness(programCodeLines))
                return true;
            else
                return false;
        }

        public string GenFunction(string startSymbol, Grammar programGrammar)
        {
            string programCode = null;

            while (true)
            {
                try
                {
                    SyntaxTree syntaxTree = new SyntaxTree(startSymbol, programGrammar);

                    programCode = syntaxTree.GetTerminalDefinition();
                    programCode = formatTerminalDefinition(programCode);

                    if (!checkFunction(programCode))
                        throw new GeneratorException("The generated function didn't meet the configuration criteria.");

                    break;
                }
                catch (SyntaxTreeException e)
                {
                    //Console.WriteLine(e.Message);
                    continue;
                }
                catch (GeneratorException e)
                {
                    //Console.WriteLine(e.Message);
                    continue;
                }

            };

            return programCode;
        }

        private string formatTerminalDefinition(string programCode)
        {
            string output;

            var processStartInfo = new ProcessStartInfo()
            {
                FileName = globalConfig["Dependencies:clangformat:path"],
                Arguments = globalConfig["Dependencies:clangformat:args"],
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                UseShellExecute = false
            };

            var process = Process.Start(processStartInfo);
            process.StandardInput.WriteLine(programCode);
            process.StandardInput.Close();
            output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return output;
        }

        public void CustomizeTemplate(string programCode)
        {
            var sourceCode = new StringBuilder();
            var stringReader = new StringReader(template);
            string line;
            while ((line = stringReader.ReadLine()) != null)
            {
                if (line.Contains("main"))
                    sourceCode.AppendLine(programCode);
                sourceCode.AppendLine(line);
            }
            File.WriteAllText(configuration.GetCustomizedTemplatePath(), sourceCode.ToString());
        }

        public void CompileCustomizedTemplate()
        {
            var processStartInfo = new ProcessStartInfo()
            {
                FileName = @$"{globalConfig["Dependencies:gplusplus:path"]}",
                Arguments = @$"{configuration.GetCustomizedTemplatePath()} -o {configuration.GetExecutablePath()}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };
            var process = Process.Start(processStartInfo);
            var output = process.StandardOutput.ReadToEnd();
            var err = process.StandardError.ReadToEnd();
            process.WaitForExit();

            if (err.Length > 0)
                throw new GeneratorException("The customized template can't be compiled.\n" + err);
        }

        public string RunCustomizedTemplate()
        {
            var processStartInfo = new ProcessStartInfo()
            {
                FileName = @$"{configuration.GetExecutablePath()}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false
            };
            var process = Process.Start(processStartInfo);
            if (!process.WaitForExit(2000))
                process.Kill();
            var output = process.StandardOutput.ReadToEnd();
            var err = process.StandardError.ReadToEnd();
            int exitCode = process.ExitCode;

            if (exitCode != 1)
                throw new GeneratorException("The customized template had a runtime error.");

            return output;
        }

        private (string question, string answer) processOutput(string output)
        {
            string question = configuration.GetQuestion();
            string patternToReplaceInQuestion = configuration.GetPatternToReplaceInQuestion();
            string patternToDetectHypothesisContent = configuration.GetPatternToDetectHypothesisContent();
            string patternToDetectCorrectAnswer = configuration.GetPatternToDetectCorrectAnswer();

            Regex hypothesisRegex = new Regex($"{patternToDetectHypothesisContent}(?<hypothesisContent>(.|\n|\r)*){patternToDetectHypothesisContent}");
            Match hypothesisMatch = hypothesisRegex.Match(output);

            Regex answerRegex = new Regex($"{patternToDetectCorrectAnswer}(?<correctAnswer>(.|\n|\r)*){patternToDetectCorrectAnswer}");
            Match answerMatch = answerRegex.Match(output);

            if (!hypothesisMatch.Success || !answerMatch.Success)
                throw new GeneratorException("The output of the program does not match the patterns provided.");

            string hypothesisContent = hypothesisMatch.Groups["hypothesisContent"].Value.Trim();
            string correctAnswer = answerMatch.Groups["correctAnswer"].Value.Trim();

            question = Regex.Replace(question, patternToReplaceInQuestion, hypothesisContent);

            return (question, correctAnswer);
        }

        public (string question, string programCode, string answer) Generate(string grammarJSON)
        {
            string programCode, output;

            Grammar programGrammar = new Grammar(grammarJSON);
            string startSymbol = configuration.GetStartSymbol();

            while (true)
            {
                try
                {
                    CancellationTokenSource tokenSource = new CancellationTokenSource();
                    var token = tokenSource.Token;

                    Task<string>[] taskArray = new Task<string>[3];
                    for (int i = 0; i < taskArray.Length; i++)
                        taskArray[i] = Task<string>.Run(() => GenFunction(startSymbol, programGrammar).Trim(), token);

                    var idx = Task<string>.WaitAny(taskArray);
                    tokenSource.Cancel();

                    programCode = taskArray[idx].Result;

                    CustomizeTemplate(programCode);
                    CompileCustomizedTemplate();
                    output = RunCustomizedTemplate();

                    break;
                }
                catch (GeneratorException e)
                {
                    //Console.WriteLine(e.Message);
                    continue;
                }
            };

            string question, answer;
            (question, answer) = processOutput(output);

            return (question, programCode, answer);
        }

        public (string question, string answer) Generate()
        {
            string output = "";

            try
            {
                CompileCustomizedTemplate();
                output = RunCustomizedTemplate();
            }
            catch (GeneratorException e)
            {
                Console.WriteLine(e.Message);
            }

            string question, answer;
            (question, answer) = processOutput(output);

            return (question, answer);
        }
    }
}
