// <copyright file="NodeFactory.cs" company="Jaehong Lee">
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
    /// Manages tree nodes before building the expression tree.
    /// </summary>
    public class NodeFactory
    {
        /// <summary>
        /// Stack to store tree nodes.
        /// </summary>
        public static Stack<Node> Stack = new Stack<Node>();

        /// <summary>
        /// Checks if the value is numeric or not.
        /// </summary>
        /// <param name="value">value to test.</param>
        /// <returns>Returns if it's numeric.</returns>
        public static bool IsNumeric(string value)
        {
            return double.TryParse(value, out _);
        }

        /// <summary>
        /// Checks if the value is a variable or not.
        /// </summary>
        /// <param name="value">value to test.</param>
        /// <returns>Returns if it's a variable.</returns>
        public static bool IsVariable(string value)
        {
            var regex = new Regex(@"^(?![-+]?\d*\.?\d+$)[a-zA-Z0-9_]+$"); // Feedback: hard coded this.  the key characte value name of the class. instead of a list it will dictionary
            return regex.IsMatch(value);

            // only using the dictionary
        }

        /// <summary>
        /// Creates a different kind of node depending on the token from the user input.
        /// </summary>
        /// <param name="token">Token from the user input.</param>
        /// <returns>A specific type of node created.</returns>
        public static Node CreateNode(string token)
        {
            if (NodeFactory.IsNumeric(token))
            {
                Node node = new ConstNumericNode(double.Parse(token));
                Stack.Push(node);
                return node;
            }
            else if (NodeFactory.IsVariable(token))
            {
                return new VariableNode(token, null);
            }
            else if (OperatorNodeFactory.IsOperatorSymbol(token))
            {
                return OperatorNodeFactory.CreateBinaryOperatorNode(token, null, null);
            }
            else
            {
                throw new Exception($"Unsupported token: {token}");
            }
        }
    }
}
