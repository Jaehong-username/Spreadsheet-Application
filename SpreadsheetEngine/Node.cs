// <copyright file="Node.cs" company="Jaehong Lee">
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
    /// Initializes Node class which is abstract.
    /// </summary>
    public abstract class Node
    {
        /// <summary>
        /// Evaluates a node and returns a current result.
        /// </summary>
        /// <returns>Current Result.</returns>
        public abstract double Evaluate();
    }
}

// Node should evaluate only
// Evaluate abstractmethod    Evaluate method
