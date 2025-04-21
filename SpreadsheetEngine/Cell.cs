// <copyright file="Cell.cs" company="Jaehong Lee">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    using System.ComponentModel;
    using System.Xml.Linq;
    using static System.Net.Mime.MediaTypeNames;

    /// <summary>
    /// Cell abstract class to represent a Cell in Spreadsheet Application.
    /// Note: Cell doesn't need to have a tree attribute. We only need the tree when
    /// evaluating a text that starts with "=".
    /// </summary>
    public abstract class Cell // A1  = B3+5   B1 = A1+2   B2= A1+2        A1 cell object (B1, B2)
    {
        /// <summary>
        /// This is a list of dependent cells.
        /// For example, if other cells reference a particular cell location in their expression, this cell will include their reference.
        /// This is a list of cell that is dependent on you in their expression.
        /// </summary>
        public List<Cell> DependentCells = new List<Cell>();

        /// <summary>
        /// text specifies the user input into the cell.
        /// </summary>
        protected string text;

        /// <summary>
        /// value specifies the final output after it gets evaluated by the spreadsheet engine.
        /// protected ExpressionTree tree; decided to make it protected so that SpreadsheetCell class can access the tree.
        /// </summary>
        protected string value;

        private int rowIndex;
        private int columnIndex;
        private uint bgColor;

        /// <summary>
        /// Initializes a new instance of the <see cref="Cell"/> class.
        /// </summary>
        /// <param name="row">row specifies a row position in the spreadsheet.</param>
        /// <param name="column">column specifies a row position in the spreadsheet.</param>
        public Cell(int row, int column)
        {
            this.rowIndex = row;
            this.columnIndex = column;
            this.Text = string.Empty;
            this.value = string.Empty;
            this.bgColor = 0xFFFFFFFF; // Default color is white
        }

        /// <summary>
        /// PropertyChanged uses delegate which uses a giant list of function Pointers
        /// first argument: sender is the source of the event.
        /// second argument: carrys event-specific data.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { };

        // public event PropertyChangedEventHandler PropertyChanged = delegate { };

        /// <summary>
        /// Gets or sets text Property: Represents the actuall cell that's typed into the cell.
        /// </summary>
        public string Text
        {
            get => this.text;
            set
            {
                this.text = value;

                // gets invoked when user manually changes the ctext.
                // this.tree = new ExpressionTree(this.Text); // HW7 building a new expression tree based on text that has been updated.

                // is the exact moment when the cell notifies its subscribers that its "Text" property has changed.
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Text"));

                // Cells shouldnt need to know tht spreadsheet subscribed
                // Traverse through a list of events passing these two arguments
                // this: current cell object     just like any other objects an object used when calling events for properties that are changed  whatever listening to event what priopertie has cnaged
                // This is how the PropertyChanged event is triggered in the INotifyPropertyChanged interface when the Text property changes.
                // Signaling to subscribers that Text Property has been updated
                // Ask yourself where does this line of code lead to Ask yourself!
                // Who subscribved to the event called PropertyChanged!!! Spreadsheet! So Now Let's go to the Spreadsheet classs
            }
        }

        /// <summary>
        /// Gets Setting up RowIndex property that is read-only. Set in the consturcotr and returned through the get.
        /// </summary>
        public int RowIndex
        {
            get => this.rowIndex;
        }

        /// <summary>
        /// Gets Setting up ColumnIndex property that is read-only. Set in the consturcotr and returned through the get.
        /// </summary>
        public int ColumnIndex
        {
            get => this.columnIndex;
        }

        /// <summary>
        /// Gets this represents the “evaluated” value of the cell. It will just be the Text property if the
        /// text doesn’t start with the ‘=’ character.Otherwise, it will represent the evaluation of
        /// the formula that’s type in the cel. It is read-only.
        /// </summary>
        public string Value => this.value;

        /// <summary>
        /// Gets or sets BGColor property.
        /// </summary>
        public uint BGColor
        {
            get => this.bgColor;
            set
            {
                this.bgColor = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Color"));
            }
        }
    }
}