// <copyright file="ConstNumericNode.cs" company="Jaehong Lee">
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
    /// Initializes ConstNumericNode class which inherits from Node abstract class.
    /// </summary>
    public class ConstNumericNode : Node
    {
        private double content;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstNumericNode"/> class.
        /// </summary>
        /// <param name="numeric">numeric value from the user string token.</param>
        public ConstNumericNode(double numeric)
        {
            this.content = numeric;
        }

        /// <summary>
        /// Gets an attribute for a content.
        /// </summary>
        public double Content
        {
            get { return this.content; } // this should have only a getter as the its a constant numeric value.
        }

        /// <summary>
        /// Evaluates a const numeric value stored in the node.
        /// </summary>
        /// <returns>Returns a constant value in the double.</returns>
        public override double Evaluate()
        {
            return this.Content; // Converts string to Double to be used in the expression directly.
        }
    }
}
