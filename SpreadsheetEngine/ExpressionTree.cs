// <copyright file="ExpressionTree.cs" company="Jaehong Lee">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    /// <summary>
    /// Build a tree based on the expression from the user input.
    /// Implement this constructor to construct the tree from the specific expression.
    /// </summary>
    public class ExpressionTree
    {
        /// <summary>
        /// Sets the specified variable within the ExpressionTree variables dictionary.
        /// </summary>
        public Dictionary<string, string> Variables;

        /// <summary>
        /// A list of tokens from the user expression.
        /// Precondition: No whitespace entered in this assignment.
        /// </summary>
        public List<string> Tokens;

        private Node root;
        private string currentExpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionTree"/> class.
        /// </summary>
        /// <param name="newExpression">New expression entered by an user.</param>
        public ExpressionTree(string newExpression)
        {
            // parse and tranform and build the tree assign in the tree  init  now its prepping   only the UI only call the tree constructor  in hw 7  cell is the one in charge of manipulating the tree
            this.Variables = new Dictionary<string, string>();
            this.Tokens = new List<string>();
            this.root = null;
            this.CurrentExpression = newExpression;
            this.ParsingInput();
            this.BuildTree();
        }

        /// <summary>
        /// Gets or sets a current Expression.
        /// </summary>
        public string CurrentExpression { get; set; }

        /// <summary>
        /// Gets or sets a root property.
        /// </summary>
        public Node Root
        {
            get { return this.root; }
            set { this.root = value; }
        }

        /// <summary>
        /// Builds a Expression Tree based on the tokens.
        /// </summary>
        public void BuildTree()
        {
            // Split method returns an array of string so no need to wrap it again using new string[]
            string[] postfix = ShuntingYard.ConvertInfixToPostfix(this.Tokens).Split(' '); // Debugging! I made it that it excludes parenthesis before it even went through conversion algorithm conver infix to postfix before building the tree, using Shunting Yard algorithm.
            Array.Resize(ref postfix, postfix.Length - 1); // resize the array to exclude the last element.
            foreach (var token in postfix)
            {
                this.Insert(token);
            }

            this.Root = NodeFactory.Stack.Pop();
        }

        /// <summary>
        /// Parses user string input. Example: Hello+World+CPTs+12+WSU+A1+a2.
        /// </summary>
        public void ParsingInput()
        {
            // Warning! Everytime you parse the input with a new user expression, make sure to empty the list and dictionary!
            this.Tokens.Clear();
            this.Variables.Clear();

            // string pattern make sure that it also takes care of parenthesis as well!
            // string[] tokens = this.CurrentExpression.Split(this.delimiters);
            string pattern = @"\d+(\.\d+)?|[a-zA-Z]+\d*|[+\-*/()]"; // @"\d+(\.\d+)?|[a-zA-Z]+\d*|[+\-*/]"; // @"\d+(\.\d+)?|[a-zA-Z]+|[+\-*/]"; // @"[a-zA-Z0-9]+|[+\-*]";

            MatchCollection matches = Regex.Matches(this.CurrentExpression, pattern);

            foreach (Match match in matches)
            {
                string token = match.Value;

                this.Tokens.Add(match.Value);

                // !token.All(char.IsDigit) can't check decimals or negative values.
                if (!"+-*/".Contains(token) && !double.TryParse(token, out _))
                {
                    this.Variables.Add(token, 0.0.ToString());
                }
            }
        }

        /// <summary>
        /// Sets the specified variable within the ExpressionTree variables dictionary.
        /// </summary>
        /// <param name="variableName">Current variable Name from the expression.</param>
        /// <param name="variableValue">The value to replace the variable.</param>
        public void SetVariable(string variableName, string variableValue) // the vaule in the cell could change.
        {
            if (this.Variables.ContainsKey(variableName))
            {
                // throw new KeyNotFoundException($"The key '{variableName}' was not found in the dictionary.");
                this.Variables[variableName] = variableValue; // find the key and update its corresponding value.
            }
            else
            {
                throw new KeyNotFoundException($"The key '{variableName}' was not found in the dictionary.");
            }
        }

        /// <summary>
        /// Implement this method with no parameters that evaluates the expression to a double valuie.
        /// </summary>
        /// <returns>Evaluated Result from the tree.</returns>
        public double Evaluate()
        {
            return this.Root.Evaluate();
        }

        /// <summary>
        /// Inserts a node into Expression Tree. We are not doing exception checks yet.
        /// </summary>
        /// <param name="token">token: a single token.</param>
        private void Insert(string token)
        {
            // otherwise, it gets deallocated which is pointeless of making a node.
            Node node = NodeFactory.CreateNode(token); // after making the node make sure to have a reference to the tree before the fucntion ends.

            if (node is VariableNode variableNode)
            {
                // If true, it assigns node to the variable variableNode.
                variableNode.Variables = this.Variables;
                NodeFactory.Stack.Push(variableNode);
            }
            else if (node is BinaryOperatorNode binaryOperatorNode)
            {
                if (NodeFactory.Stack.Count > 1)
                {
                    binaryOperatorNode.Right = NodeFactory.Stack.Pop();
                    binaryOperatorNode.Left = NodeFactory.Stack.Pop();
                    NodeFactory.Stack.Push(binaryOperatorNode);
                }
                else
                {
                    throw new InvalidOperationException("Stack is empty. Not enough operands for the operator!");
                }
            }
        }
    }
}