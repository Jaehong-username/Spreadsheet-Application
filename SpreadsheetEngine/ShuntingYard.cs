// <copyright file="ShuntingYard.cs" company="Jaehong Lee">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Converts infix expression to postfix expression using Shunting Yard Algorithm.
    /// </summary>
    public class ShuntingYard
    {
        // remove these replace dictionarit  operator with in the factory.     what is the chaacater and what is the class
        // factory can provide an iterface for shungitng yard to usr.
        // vshungint yard only need to call the method from the factory class.

        /// <summary>
        /// Converts infix to post fix expression.
        /// </summary>
        /// <param name="tokens">tokens of Infix expression from the user input.</param>
        /// <returns>Returns post fix expression.</returns>
        public static string ConvertInfixToPostfix(List<string> tokens)
        {
            Stack<string> stack = new Stack<string>();
            string postfix = string.Empty;
            foreach (string token in tokens)
            {
                if (!OperatorNodeFactory.IsOperatorSymbol(token))
                {
                    // if it's an operand Append the digit to the postfix expression
                    postfix += token + " ";
                }

                // token == "(" No hardcoding.
                else if (OperatorNodeFactory.IsLeftParenthesis(token))
                {
                    // push the left parenthesis onto the stack.
                    stack.Push(token);
                }

                // token == ")" No hardcoding.
                else if (OperatorNodeFactory.IsRightParenthesis(token))
                {
                    // stack.Peek() != "(".
                    // Pop all operators until we encounter a matching parenthesis
                    while (stack.Count > 0 && !OperatorNodeFactory.IsLeftParenthesis(stack.Peek()))
                    {
                        postfix += stack.Pop() + " ";
                    }

                    // once it breaks out of the while loop.
                    if (stack.Count == 0)
                    {
                        throw new Exception("No matching parenthesis!");
                    }
                    else
                    {
                        // pop the left parenthesis.
                        stack.Pop();
                    }
                } // Make sure to put Binary Operator check at the bottom!!!
                else if (OperatorNodeFactory.IsOperatorSymbol(token))
                {
                    // its a binary operator
                    while (stack.Count > 0 && OperatorNodeFactory.Operators[stack.Peek()] >= OperatorNodeFactory.Operators[token])
                    {
                        postfix += stack.Pop() + " ";
                    }

                    stack.Push(token);
                }
            }

            // Error!!
            // pop remaining operators from the stack. // converted infix to postfix, need for precedence during conversion.
            // make sure to include an if statement only pop the remaining element when it's not emty(only one symbol).
            if (stack.Count > 0)
            {
                postfix += stack.Pop() + " ";
            }

            if (stack.Count > 0)
            {
                throw new Exception("Invalid expression!");
            }

            return postfix;
        }
    }
}
