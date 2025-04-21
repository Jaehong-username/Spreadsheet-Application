// <copyright file="ColorChangeCommand.cs" company="Jaehong Lee">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Executes the command related updating a cell's text.
    /// </summary>
    public class ColorChangeCommand : ICommand
    {
        /// <summary>
        /// The cell about to be updated.
        /// </summary>
        private Cell changingCell;

        /// <summary>
        /// The color to update the cell to.
        /// </summary>
        private uint newBGColor;

        /// <summary>
        /// Old color after to get replaced with a new color.
        /// </summary>
        private uint oldBGColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ColorChangeCommand"/> class.
        /// Encapsulates needed information in the cell object before executing it.
        /// </summary>
        /// <param name="cell">cell that's being.</param>
        /// <param name="newBGColor">new color to change to.</param>
        public ColorChangeCommand(Cell cell, uint newBGColor)
        {
            this.changingCell = cell;
            this.oldBGColor = this.changingCell.BGColor;
            this.newBGColor = newBGColor;
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        public void Execute()
        {
            this.oldBGColor = this.changingCell.BGColor;
            this.changingCell.BGColor = this.newBGColor;
        }

        /// <summary>
        /// Unexecutes the command.
        /// </summary>
        public void UnExecute()
        {
            this.changingCell.BGColor = this.oldBGColor;
        }

        // ColorDiaglog is for UI Winform.
    }
}
