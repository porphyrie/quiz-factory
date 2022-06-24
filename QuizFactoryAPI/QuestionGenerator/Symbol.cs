using System;
using System.Collections.Generic;

namespace QuizFactoryAPI.QuestionGenerator
{
    public class Symbol
    {
        public Symbol Parent;
        private string identifier;
        public List<Symbol> Children;
        private List<string> terminalDefinition;

        public Symbol(string identifier)
        {
            this.identifier = identifier;
            Children = new List<Symbol>();
            terminalDefinition = new List<string>();
        }

        public Symbol(string identifier, Symbol parent)
        {
            Parent = parent;
            this.identifier = identifier;
            Children = new List<Symbol>();
            terminalDefinition = new List<string>();
        }

        public void Copy(Symbol symbol)
        {
            this.identifier = symbol.identifier;
            Children = new List<Symbol>(symbol.Children);
            terminalDefinition = new List<string>(symbol.terminalDefinition);
        }

        public string GetIdentifier()
        {
            return identifier;
        }

        public List<string> GetTerminalDefinition()
        {
            return terminalDefinition;
        }

        public string GetTerminalDefinitionAsString()
        {
            return string.Join('\n', terminalDefinition).TrimEnd();
        }

        public Symbol AddChild(string identifier)
        {
            var symbol = new Symbol(identifier, this);
            Children.Add(symbol);
            return symbol;
        }

        public void RmvChild(Symbol symbol)
        {
            Children.Remove(symbol);
            BuildTerminalDefinition();
        }

        public void SetTerminalDefinition(string terminalDefinition)
        {
            this.terminalDefinition.Add(terminalDefinition);
        }

        public void BuildTerminalDefinition()
        {
            terminalDefinition.Clear();
            foreach (var child in Children)
                terminalDefinition.AddRange(child.terminalDefinition);
        }

        public bool SearchChild(string identifier)
        {
            foreach (var child in Children)
                if (child.identifier.Equals(identifier))
                    return true;
            return false;
        }

        public bool SearchTerminal(string terminalDefinition)
        {
            foreach (var terminal in this.terminalDefinition)
                if (terminal.Equals(terminalDefinition))
                    return true;
            return false;
        }

        public void ResetContext()
        {
            Children.Clear();
            terminalDefinition.Clear();
        }
    }
}
