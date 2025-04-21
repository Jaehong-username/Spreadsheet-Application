// <copyright file="NoCommand.cs" company="Jaehong Lee">
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
    /// This NoCommand wil do nothing.
    /// </summary>
    public class NoCommand : ICommand
    {
        // No commandso that if no input is coming, it doesn;t need to wait for inputs.

        /// <summary>
        /// This execute does nothing.
        /// </summary>
        public void Execute()
        {
        }

        /// <summary>
        /// this unexecute does nothing like execute.
        /// </summary>
        public void UnExecute()
        {
        }
    }
}
