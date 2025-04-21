// <copyright file="BinaryOperatorNode.cs" company="Jaehong Lee">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Initializes BinaryOperatorNode class which inherits abstract class Node.
    /// Needs to have child node attributes.
    /// </summary>
    public class BinaryOperatorNode : Node
    {
        /// <summary>
        /// Precedence of the binary operator.
        /// </summary>
        protected int precedence;

        /// <summary>
        /// Associativity of the binary operator.
        /// </summary>
        protected string associativity;

        private string content;
        private Node left;
        private Node right;

        /// <summary>
        /// Initializes a new instance of the <see cref="BinaryOperatorNode"/> class.
        /// </summary>
        /// <param name="value">value is an operator.</param>
        /// <param name="left">Represents a left node.</param>
        /// <param name="right">right represents a right node.</param>
        public BinaryOperatorNode(string value, Node left, Node right)
        {
            this.Content = value;
            this.Left = left;
            this.Right = right;
        }

        /// <summary>
        /// Gets an attribute for precedence.
        /// </summary>
        public int Precedence
        {
            get { return this.precedence; }
        }

        /// <summary>
        /// Gets an attribute for associativity.
        /// </summary>
        public string Associativity
        {
            get { return this.associativity; }
        }

        /// <summary>
        /// Gets or sets an attribute for a Content attribute.
        /// </summary>
        public string Content
        {
            get { return this.content; }
            set { this.content = value; }
        }

        /// <summary>
        /// Gets or sets an attribute for a left node.
        /// </summary>
        public Node Left
        {
            get { return this.left; }
            set { this.left = value; }
        }

        /// <summary>
        /// Gets or sets an attribute for a right node.
        /// </summary>
        public Node Right
        {
            get { return this.right; }
            set { this.right = value; }
        }

        /// <summary>
        /// Evaluate method in the binaryOperatorNode.
        /// </summary>
        /// <returns>Returns an evaluated result.</returns>
        /// <exception cref="NotImplementedException">Signals that derived classes must override this Evaluate method.</exception>
        public override double Evaluate()
        {
            throw new NotImplementedException("BinaryOperatorNode Evaluate method must be overwritten");
        }
    }
}


// () ^    /0   when you test things, make sure to focus on things that will break.