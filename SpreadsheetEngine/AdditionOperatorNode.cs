// <copyright file="AdditionOperatorNode.cs" company="Jaehong Lee">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This defines an AdditionOperatorNode, when the binary operator is addition.
    /// </summary>
    public class AdditionOperatorNode : BinaryOperatorNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AdditionOperatorNode"/> class.
        /// </summary>
        /// <param name="left">Left Node of the parent node.</param>
        /// <param name="right">Right Node of the parent node.</param>
        public AdditionOperatorNode(Node left, Node right)
            : base("+", left, right)
        {
            this.precedence = 1; // more complex operators bit shift
            this.associativity = "left";
        }

        /// <summary>
        /// Evaluates an addition operation.
        /// </summary>
        /// <returns>Returns the evaluated result.</returns>
        public override double Evaluate()
        {
            return this.Left.Evaluate() + this.Right.Evaluate();
        }
    }
}
