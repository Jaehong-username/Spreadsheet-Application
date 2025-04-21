// <copyright file="DivisionOperatorNode.cs" company="Jaehong Lee">
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
    /// This defines a DivisionOperatorNode, when the binary operator is addition.
    /// </summary>
    public class DivisionOperatorNode : BinaryOperatorNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DivisionOperatorNode"/> class.
        /// </summary>
        /// <param name="left">Left Node of the parent node.</param>
        /// <param name="right">Right Node of the parent node.</param>
        public DivisionOperatorNode(Node left, Node right)
            : base("/", left, right)
        {
            this.precedence = 2;
            this.associativity = "left";
        }

        /// <summary>
        /// Evaluates a division operation.
        /// </summary>
        /// <returns>Returns the evaluated result.</returns>
        public override double Evaluate()
        {
            return this.Left.Evaluate() / this.Right.Evaluate();
        }
    }
}
