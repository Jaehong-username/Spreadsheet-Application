// <copyright file="SubtractionOperatorNode.cs" company="Jaehong Lee">
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
    /// This defines an SubtractionOperatorNode, when the binary operator is subtraction.
    /// </summary>
    public class SubtractionOperatorNode : BinaryOperatorNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubtractionOperatorNode"/> class.
        /// </summary>
        /// <param name="left">Left Node of the parent node.</param>
        /// <param name="right">Right Node of the parent node.</param>
        public SubtractionOperatorNode(Node left, Node right)
            : base("-", left, right)
        {
            this.precedence = 1; // this is a lower precedence.
            this.associativity = "left";
        }

        /// <summary>
        /// Evaluates a subtraction operation.
        /// </summary>
        /// <returns>Returns the evaluated result.</returns>
        public override double Evaluate()
        {
            return this.Left.Evaluate() - this.Right.Evaluate();
        }
    }
}
