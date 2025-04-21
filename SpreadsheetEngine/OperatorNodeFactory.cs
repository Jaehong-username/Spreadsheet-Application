// <copyright file="OperatorNodeFactory.cs" company="Jaehong Lee">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class defines OperatorNodeFactory class.
    /// </summary>
    public class OperatorNodeFactory
    {
        // dont want to see this in diffete nplaces. isolate definition of character and usage.
        // private static readonly List<string> Operators = new List<string> { "+", "-", "*", "/" };

        /// <summary>
        /// A dictionary of operators with the corresponding precedence.
        /// </summary>
        public static readonly Dictionary<string, int> Operators = new Dictionary<string, int>
        {
            { "(", 0 },
            { ")", 0 },
            { "+", 1 },
            { "-", 1 },
            { "*", 2 },
            { "/", 2 },
        };

        private static Dictionary<string, Type> dynamicOperators = new Dictionary<string, Type>() { };

        /// <summary>
        /// Initializes a new instance of the <see cref="OperatorNodeFactory"/> class.
        /// </summary>
        public OperatorNodeFactory()
        {
            TraverseAvailableOperators((op, type) => dynamicOperators.Add(op, type)); // passing the delegate as the function.
        }

        /// <summary>
        /// Gets dictionary to include operators that have been dynamically generated.
        /// </summary>
        public static Dictionary<string, Type> DynamicOperators
        {
            get { return dynamicOperators; }
        }

        /// <summary>
        /// operator look up to dynamically populate a new operator using reflection
        /// </summary>
        /// <param name="op">the operator string</param>
        /// <param name="type">Operator type</param>
        private delegate void OnOperator(string op, Type type);

        /// <summary>
        /// Checks if it's an operator symbol.
        /// </summary>
        /// <param name="symbol">Symbol token from the user input.</param>
        /// <returns>Returns boolean if it's an operator symobol.</returns>
        public static bool IsOperatorSymbol(string symbol)
        {
            return Operators.ContainsKey(symbol);
        }

        /// <summary>
        /// Checks if it's the left parenthesis.
        /// </summary>
        /// <param name="symbol">symbol to check.</param>
        /// <returns>boolean.</returns>
        public static bool IsLeftParenthesis(string symbol)
        {
            return symbol == "(";
        }

        /// <summary>
        /// Checks if it's the right parenthesis.
        /// </summary>
        /// <param name="symbol">symbol to check.</param>
        /// <returns>boolean.</returns>
        public static bool IsRightParenthesis(string symbol)
        {
            return symbol == ")";
        }

        /// <summary>
        /// Populates a new binary operator node based on the symbol.
        /// </summary>
        /// <param name="binaryOperator">Biary Operator Symbol.</param>
        /// <param name="left">Left Node of the binary operator node.</param>
        /// <param name="right">Right node of the binary operator node.</param>
        /// <returns>Returns a new binary operator node.</returns>
        /// <exception cref="InvalidOperationException">If the symbol is invalud.</exception>
        public static BinaryOperatorNode CreateBinaryOperatorNode(string binaryOperator, Node left, Node right)
        {
            // if (dynamicOperators.ContainsKey(binaryOperator))
            // {
            //    object operatorNodeObject = Activator.CreateInstance(dynamicOperators[binaryOperator]);
            //    if (operatorNodeObject is BinaryOperatorNode)
            //    {
            //        return (BinaryOperatorNode)operatorNodeObject;
            //    }
            // }

            // throw new Exception("Operator not found!");
            switch (binaryOperator)
            {
                case "+":
                    return new AdditionOperatorNode(left, right);
                case "-":
                    return new SubtractionOperatorNode(left, right);
                case "*":
                    return new MultiplicationOperatorNode(left, right); // Debugging It was AdditionOperatorNode ...
                case "/":
                    return new DivisionOperatorNode(left, right); // Debugging It was AdditionOperatorNode ...

                default:
                    throw new InvalidOperationException($"Unsupported operator: {binaryOperator}");
            }
        } // later replace the code with ditionary  that dynamically populates.

        /// <summary>
        /// To dynamically populate a new operator node in our dictionary using reflection.
        /// This will get invoked only once, once we run the program.
        /// </summary>
        /// <param name="onOperator">the delegate which defines the function signature to look up the operators.</param>
        private static void TraverseAvailableOperators(OnOperator onOperator)
        {
            // get the type declaration of OperatorNode
            Type operatorNodeType = typeof(BinaryOperatorNode);

            // Iterate over all loaded assemblies:
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                // Get all types that inherit from our OperatorNode class using LINQ
                IEnumerable<Type> operatorTypes =
                assembly.GetTypes().Where(type => type.IsSubclassOf(operatorNodeType));

                // subclass of binaryoperatornode, getting all classes that inherit from binaryoperatornode.
                // Iterate over those subclasses of BinaryOperatorNode
                foreach (var type in operatorTypes)
                {
                    // for each subclass, retrieve the Operator property
                    PropertyInfo operatorField = type.GetProperty("content"); // Property name string.

                    if (operatorField != null)
                    {
                        // Get the character of the Operator
                        object value = operatorField.GetValue(type); // getting the value of the content field

                        // If “Operator” property is not static, you will need to create
                        // an instance first and use the following code instead (or similar):
                        // object value = operatorField.GetValue(Activator.CreateInstance(type,
                        // new ConstantNode(0)));
                        if (value is string)
                        {
                            string operatorSymbol = value.ToString();

                            // And invoke the function passed as parameter
                            // with the operator symbol and the operator class
                            onOperator(operatorSymbol, type);
                        }
                    }
                }
            }
        }
    }
}
