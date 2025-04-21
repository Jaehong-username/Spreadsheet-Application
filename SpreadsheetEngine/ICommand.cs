// <copyright file="ICommand.cs" company="Jaehong Lee">
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
    /// This is a command interface class.
    /// Every command must have implementation of the following methods that are Execute() and UnExecute().
    /// </summary>
    public interface ICommand
    {
        /// <summary>
        /// It does the command.
        /// </summary>
        void Execute();

        /// <summary>
        /// It undoes the command.
        /// </summary>
        void UnExecute();
    }
}
