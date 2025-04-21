// <copyright file="TextChangeCommand.cs" company="Jaehong Lee">
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
    /// Command class related to updating cell text.
    /// </summary>
    public class TextChangeCommand : ICommand
    {
        /// <summary>
        /// The cell about to be updated.
        /// </summary>
        public Cell ChangingCell;

        /// <summary>
        /// Text to update the cell to.
        /// </summary>
        private string newText;

        /// <summary>
        /// Old text after to get replaced with a new text.
        /// </summary>
        private string oldText;

        /// <summary>
        /// Initializes a new instance of the <see cref="TextChangeCommand"/> class.
        /// Encapsulates needed information in the cell object before executing it.
        /// </summary>
        /// <param name="cell">cell that's being.</param>
        /// <param name="newText">new text to change to.</param>
        public TextChangeCommand(Cell cell, string newText)
        {
            this.ChangingCell = cell;
            this.oldText = cell.Text;
            this.newText = newText;
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        public void Execute()
        {
            this.ChangingCell.Text = this.newText;
        }

        /// <summary>
        /// Unexecutes the command.
        /// </summary>
        public void UnExecute()
        {
            this.ChangingCell.Text = this.oldText;
        }
    }
}
