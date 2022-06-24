using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace QuizFactoryAPI.QuestionGenerator
{
    public class SyntaxTree
    {
        private Grammar programGrammar;
        private Symbol root;
        private Dictionary<string, Tuple<string, bool, bool>> symbolTable;
        private Stack<List<string>> usedExprVarsStack;
        private Queue<List<string>> stmtCache;

        private int x;
        private string name;
        private string objectInPossesion;

        private bool insertTypeAndIdentifierIntoSymbolTable(List<string> declaration)
        {
            var type = declaration[0];
            string identifier;

            if (declaration.Contains("*"))
            {
                identifier = declaration[2];
                if (symbolTable.Keys.Contains(identifier))
                    return false;
                symbolTable.Add(identifier, new Tuple<string, bool, bool>(type, true, false));
            }
            else
            {
                identifier = declaration[1];
                if (symbolTable.Keys.Contains(identifier))
                    return false;
                symbolTable.Add(identifier, new Tuple<string, bool, bool>(type, false, false));
            }

            return true;
        }

        private List<string> processStmtOrExprToTokens(List<string> stmt)
        {
            List<string> stmtSymbols = programGrammar.GetRules("operators").SelectMany(list => list).Distinct().ToList();
            stmtSymbols.AddRange(programGrammar.GetRules("keywords").SelectMany(list => list).Distinct().ToList());
            stmtSymbols.AddRange(programGrammar.GetRules("symbols").SelectMany(list => list).Distinct().ToList());

            List<string> stmtTokens = new List<string>(stmt);

            for (int i = 0; i < stmtTokens.Count; i++)
            {
                if (stmtSymbols.Contains(stmtTokens[i]))
                {
                    stmtTokens.Remove(stmtTokens[i]);
                    i--;
                }
            }

            return stmtTokens;
        }

        private List<string> getVarsFromTokens(List<string> stmtTokens)
        {
            for (int i = 0; i < stmtTokens.Count; i++)
            {
                string stmtToken = stmtTokens[i];
                if (int.TryParse(stmtToken, out _))
                {
                    stmtTokens.Remove(stmtToken);
                    i--;
                }
                else if (stmtToken.Contains("->"))
                    stmtTokens[i] = stmtToken.Split("->").ToList()[0];
            }

            stmtTokens = stmtTokens.Distinct().ToList();

            return stmtTokens;
        }

        private void markStmtOrExprVarsAsUsed(List<string> stmtTokens)
        {
            for (int i = 0; i < stmtTokens.Count; i++)
            {
                var identifier = stmtTokens[i];

                var type = symbolTable[identifier].Item1;
                var pointer = symbolTable[identifier].Item2;
                var used = symbolTable[identifier].Item3;

                if (!used)
                {
                    used = true;
                    symbolTable.Remove(identifier);
                    symbolTable.Add(identifier, new Tuple<string, bool, bool>(type, pointer, used));
                }
            }
        }

        private bool checkIfStmtOrExprVarsInSymbolTable(List<string> stmtTokens)
        {
            for (int i = 0; i < stmtTokens.Count; i++)
            {
                string stmtToken = stmtTokens[i];
                if (!symbolTable.ContainsKey(stmtToken))
                    return false;
            }

            return true;
        }

        private bool checkIfStmtVarsOnTopOfUsedExprVarsStack(List<string> stmtTokens)
        {
            foreach (var stmtToken in stmtTokens)
                if (!usedExprVarsStack.Peek().Contains(stmtToken))
                    return false;

            return true;
        }

        private void checkAndBuildDecl(ref Symbol decl)
        {
            int ruleCount = 3 * programGrammar.GetRules(decl.GetIdentifier()).Count;
            do
            {
                regenerate(ref decl);
                buildTerminalDefinition(ref decl);
                ruleCount--;
            } while (!insertTypeAndIdentifierIntoSymbolTable(decl.GetTerminalDefinition()) && ruleCount != -1);
            if (ruleCount == -1)
                throw new SyntaxTreeException($"Declaration {decl.GetIdentifier()} could not be generated! The variable is already in the symbol table. You can create a similiar declaration with a different identifier.");
        }

        private bool checkIfStmtInStmtCache(List<string> stmtTerminalDefinition)
        {
            foreach (var stmt in stmtCache)
                if (stmt.Count == stmtTerminalDefinition.Count)
                {
                    int test = 0;
                    for (int i = 0; i < stmt.Count; i++)
                        if (stmt[i] != stmtTerminalDefinition[i])
                        {
                            test = 1;
                            break;
                        }
                    if (test == 0)
                        return true;
                }
            return false;
        }

        private void addToStmtCache(List<string> stmtTerminalDefinition)
        {
            if (stmtCache.Count == 2)
                stmtCache.Dequeue();
            stmtCache.Enqueue(stmtTerminalDefinition);
        }

        private void checkAndBuildStmt(ref Symbol stmt)
        {
            int ruleCount = 3 * programGrammar.GetRules(stmt.GetIdentifier()).Count;
            while (ruleCount != -1)
            {
                regenerate(ref stmt);
                buildTerminalDefinition(ref stmt);
                if (!stmt.GetTerminalDefinitionAsString().TrimEnd().EndsWith(";"))
                    throw new SyntaxTreeException($"The syntax of the stmt {stmt.GetIdentifier()} is not correct.");
                if (!checkIfStmtInStmtCache(stmt.GetTerminalDefinition()))
                {
                    List<string> stmtTokens = processStmtOrExprToTokens(stmt.GetTerminalDefinition());
                    stmtTokens = getVarsFromTokens(stmtTokens);
                    if (checkIfStmtOrExprVarsInSymbolTable(stmtTokens))
                    {
                        markStmtOrExprVarsAsUsed(stmtTokens);
                        break;
                    }
                }
                ruleCount--;
            }
            if (ruleCount == -1)
                throw new SyntaxTreeException($"Statement {stmt.GetIdentifier()} could not be generated! Check if the variables used within that statement are declared. You might need to increase the number of statement definitions for a variable because it could have been used previously (1-2 stmts ago). Take that into consideration.");
            addToStmtCache(stmt.GetTerminalDefinition());
        }

        private void checkAndBuildExpr(ref Symbol expr)
        {
            int ruleCount = 3 * programGrammar.GetRules(expr.GetIdentifier()).Count;
            while (ruleCount != -1)
            {
                regenerate(ref expr);
                buildTerminalDefinition(ref expr);

                List<string> exprTokens = processStmtOrExprToTokens(expr.GetTerminalDefinition());
                exprTokens = getVarsFromTokens(exprTokens);
                if (checkIfStmtOrExprVarsInSymbolTable(exprTokens))
                {
                    if (expr.Parent.SearchChild("while"))
                        usedExprVarsStack.Push(exprTokens);
                    markStmtOrExprVarsAsUsed(exprTokens);
                    break;
                }
                ruleCount--;
            }
            if (ruleCount == -1)
                throw new SyntaxTreeException($"Expression {expr.GetIdentifier()} could not be generated! Check if the variables used within that expression are declared.");
        }

        private void checkAndBuildMustStmt(ref Symbol mustStmt)
        {
            int ruleCount = 5 * programGrammar.GetRules(mustStmt.GetIdentifier()).Count; //cu cat sunt mai putine reguli creste probabilitatea sa o aleaga tot pe aia de cateva ori
            while (ruleCount != -1)
            {
                regenerate(ref mustStmt);
                buildTerminalDefinition(ref mustStmt);

                List<string> stmtTokens = processStmtOrExprToTokens(mustStmt.GetTerminalDefinition());
                stmtTokens = getVarsFromTokens(stmtTokens);
                if (checkIfStmtOrExprVarsInSymbolTable(stmtTokens))
                    if (checkIfStmtVarsOnTopOfUsedExprVarsStack(stmtTokens))
                    {
                        usedExprVarsStack.Pop();
                        break;
                    }
                ruleCount--; //must have statements are used only for loops
            }
            if (ruleCount == -1)
                throw new SyntaxTreeException($"Musthavestatement {mustStmt.GetIdentifier()} could not be generated! Check if the variables used within that statement are declared. You might need to check if there is at least one musthavestatement definition for a variable used within the loop {mustStmt.Parent.GetIdentifier()} expression.");
        }

        private void generate(ref Symbol root)
        {
            List<List<string>> ruleList = programGrammar.GetRules(root.GetIdentifier());

            if (ruleList == null)
            {
                if (programGrammar.CheckIfTerminalOrMacro(root.GetIdentifier()))
                    return;
                else
                    throw new GrammarException($"The symbol {root.GetIdentifier()} does not have an associated rule.");
            }

            if (ruleList.Count == 0)
                return;

            Random rand = new Random();
            int index = rand.Next(ruleList.Count);
            List<string> chosenRuleSymbolList = ruleList[index];

            foreach (var symbol in chosenRuleSymbolList)
            {
                var child = root.AddChild(symbol);
                generate(ref child);
            }
        }

        private void regenerate(ref Symbol root)
        {
            root.ResetContext();
            generate(ref root);
        }

        private bool checkTerminalDefRegexMatch(Symbol symbol, Regex expr)
        {
            string terminalDefinition = symbol.GetTerminalDefinitionAsString();
            Match match = expr.Match(terminalDefinition);
            if (match.Success)
                return true;
            else
                return false;
        }

        public void SearchRegexMatch(Symbol symbol, Regex expr, ref List<Symbol> symbolsFound)
        {
            for (int i = 0; i < symbol.Children.Count; i++)
            {
                if (checkTerminalDefRegexMatch(symbol.Children[i], expr))
                    symbolsFound.Add(symbol.Children[i]);
                SearchRegexMatch(symbol.Children[i], expr, ref symbolsFound);
            }
        }

        public void Remove(Symbol symbol)
        {
            Symbol parent = symbol.Parent;
            parent.RmvChild(symbol);
            RebuildTerminalDefinition(parent);
            /*if (parent.Children.Count == 0)
                grandParent = Remove(symbol.Parent);*/
        }

        public void RebuildTerminalDefinition(Symbol symbol)
        {
            symbol.BuildTerminalDefinition();
            if (symbol.Parent == null)
                return;
            else
                RebuildTerminalDefinition(symbol.Parent);
        }

        private void removeUnusedDecls()
        {
            foreach (var identifier in symbolTable.Keys)
            {
                if (!symbolTable[identifier].Item3)
                {
                    var type = symbolTable[identifier].Item1;
                    var pointer = (symbolTable[identifier].Item2 ? "\\*" : "");

                    Regex expr = new Regex(@$"^{type}\n({pointer}\n)?{identifier}\n=\n.*\n;$");

                    List<Symbol> declFound = new List<Symbol>();
                    SearchRegexMatch(root, expr, ref declFound);

                    foreach (var decl in declFound)
                        Remove(decl);

                    symbolTable.Remove(identifier);
                }
            }
        }

        private void buildTerminalDefinition(ref Symbol root)
        {
            if (programGrammar.isTerminal(root.GetIdentifier()) != null)
                root.SetTerminalDefinition(root.GetIdentifier());
            else if (programGrammar.isMacro(root.GetIdentifier()) != null)
                root.SetTerminalDefinition(programGrammar.isMacro(root.GetIdentifier()));
            //daca parintele este init, add, mul executa o functie cu switch care modifica var x
            else
            {
                for (int i = 0; i < root.Children.Count; i++)
                {
                    var child = root.Children[i];
                    if (child.GetIdentifier().StartsWith("decl") && (!child.GetIdentifier().Equals("decls")))
                    {
                        //if (child.GetIdentifier().Equals("decl#delete"))
                        //Console.WriteLine("te-am prins");
                        checkAndBuildDecl(ref child);
                    }
                    else if (child.GetIdentifier().StartsWith("stmt") && (!child.GetIdentifier().Equals("stmts")))
                        checkAndBuildStmt(ref child);
                    else if (child.GetIdentifier().StartsWith("expr") && (!child.GetIdentifier().Equals("exprs")))
                        checkAndBuildExpr(ref child);
                    else if ((child.GetIdentifier().StartsWith("must") && child.GetIdentifier().EndsWith("stmt")))
                        checkAndBuildMustStmt(ref child);
                    else if (child.GetIdentifier().Equals("stmts"))
                    {
                        buildTerminalDefinition(ref child);
                        removeUnusedDecls();
                    }
                    else
                        buildTerminalDefinition(ref child);
                    //if(child.GetIdentifier().StartsWith("decl") || child.GetIdentifier().StartsWith("stmt") || child.GetIdentifier().StartsWith("expr") || child.GetIdentifier().StartsWith("must"))
                    //Console.WriteLine(child.GetTerminalDefinitionAsString().Replace("\n", " "));
                }
                root.BuildTerminalDefinition();
            }
        }

        public string GetTerminalDefinition()
        {
            return root.GetTerminalDefinitionAsString();
        }

        public SyntaxTree(string startSymbol, Grammar programGrammar)
        {
            this.programGrammar = programGrammar;
            root = new Symbol(startSymbol);
            symbolTable = new Dictionary<string, Tuple<string, bool, bool>>();
            stmtCache = new Queue<List<string>>();
            usedExprVarsStack = new Stack<List<string>>();

            var globalDecls = programGrammar.GetRules("globaldecls");
            foreach (var globalDecl in globalDecls)
                insertTypeAndIdentifierIntoSymbolTable(globalDecl);

            generate(ref root);

            buildTerminalDefinition(ref root);
        }
    }
}
