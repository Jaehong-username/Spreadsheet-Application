// <copyright file="VariableNode.cs" company="Jaehong Lee">
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
    /// Initializes VariableNode class which inherits abstract class Node.
    /// </summary>
    public class VariableNode : Node
    {
        private string content;
        private Dictionary<string, string> variables; // it will create a reference to the tree's variable dictionary.

        /// <summary>
        /// Initializes a new instance of the <see cref="VariableNode"/> class.
        /// </summary>
        /// <param name="name">name: the name of variable.</param>
        /// <param name="variables">Key - Value pairs: The name - corresponding value.</param>
        public VariableNode(string name, Dictionary<string, string> variables)
        {
            this.content = name;
            this.variables = variables; // storing it in the private reference.

            // this.Result = 0.0; // from the abstract class. Once we create a variable node, the default value should be 0.0 as in the instructions.
        }

        /// <summary>
        /// Gets an attribute for a Content attribute.
        /// </summary>
        public string Content
        {
            get { return this.content; }
        }

        /// <summary>
        /// Gets or sets an operand attribute. It is read only.
        /// </summary>
        public Dictionary<string, string> Variables
        {
            get { return this.variables; }
            set { this.variables = value; }
        }

        /// <summary>
        /// Returns its corresponding value.
        /// </summary>
        /// <returns>The corresponding value.</returns>
        public override double Evaluate()
        {
            // if variable not have add checks this.
            return double.Parse(this.Variables[this.Content]); // throws an exception here
        }
    }
}
