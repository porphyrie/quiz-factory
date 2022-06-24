using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace QuizFactoryAPI.QuestionGenerator
{
    public class Grammar
    {
        private Dictionary<string, List<List<string>>> rules;
        private List<string> terminals;
        private List<string> nonTerminals;

        private List<string> getTerminals(List<List<string>> terminalCategories)
        {
            List<string> terminals = new List<string>();

            for (int i = 0; i < terminalCategories.Count; i++)
                for (int j = 0; j < terminalCategories[i].Count; j++)
                    terminals.AddRange(rules[terminalCategories[i][j]].SelectMany(list => list).ToList());

            return terminals;
        }

        public string isMacro(string symbol)
        {
            if (Regex.IsMatch(symbol, "^rand\\(\\d*,\\d*\\)$"))
                return genNumCharacter(symbol);
            else
                return null;
        }

        public string isTerminal(string symbol)
        {
            if (terminals.Contains(symbol))
                return symbol;
            else
                return null;
        }

        public bool CheckIfTerminalOrMacro(string symbol)
        {
            if (isTerminal(symbol) != null || isMacro(symbol) != null)
                return true;
            else
                return false;
        }

        public bool CheckIfSymbolOrMacro(string symbol)
        {
            if (CheckIfTerminalOrMacro(symbol) || nonTerminals.Contains(symbol))
                return true;
            else
                return false;
        }

        private bool checkGrammarCorrectness()
        {
            foreach (var nonTerminal in nonTerminals)
            {
                List<string> symbols = rules[nonTerminal].SelectMany(list => list).Distinct().ToList();
                foreach (var symbol in symbols)
                {
                    if (!CheckIfSymbolOrMacro(symbol))
                        throw new GrammarException($"The symbol {symbol} does not have an associated rule.");
                }
            }
            return true;
        }

        public Grammar(string grammarJSON)
        {
            var rules = JsonSerializer.Deserialize<Dictionary<string, List<List<string>>>>(grammarJSON);

            this.rules = rules;
            terminals = getTerminals(this.rules["terminals"]);
            nonTerminals = getNonTerminals();
            checkGrammarCorrectness();
        }

        public List<List<string>> GetRules(string ruleName)
        {
            if (nonTerminals.Contains(ruleName))
                return rules[ruleName];
            else
                return null;
        }

        private List<string> getNonTerminals()
        {
            List<string> nonTerminals = new List<string>();

            nonTerminals.AddRange(rules.Keys);
            /*nonTerminals.Remove("terminals");
            var terminalCategories = rules["terminals"].SelectMany(list => list).Distinct().ToList();
            foreach (var terminalCategory in terminalCategories)
                nonTerminals.Remove(terminalCategory);*/

            return nonTerminals;
        }

        private string genNumCharacter(string symbol)
        {
            Regex digitRegex = new Regex("^rand\\((?<firstDigit>\\d*),(?<secondDigit>\\d*)\\)$");
            Match digitMatch = digitRegex.Match(symbol);

            Random rand = new Random();

            if (digitMatch.Success)
            {
                int firstDigit = Int32.Parse(digitMatch.Groups["firstDigit"].Value);
                int secondDigit = Int32.Parse(digitMatch.Groups["secondDigit"].Value);
                return rand.Next(firstDigit, secondDigit + 1).ToString();
            }

            return "";
        }
    }
}
