// <copyright file="Spreadsheet.cs" company="Jaehong Lee">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics.Metrics;
    using System.Drawing;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    /// <summary>
    /// It will serve as a container for a 2D array of cells. It will also serve as a factory for cells, meaning it is the entity that actually creates all the cells in the spreadsheet.
    /// Create SpreadsheetEngine to decouple logic from the UI.
    /// </summary>
    public class Spreadsheet
    {
        private int columnCount;
        private int rowCount;
        private Dictionary<string, Cell> variables; // dictionary of cell location and value pair.

        // in theory  other types of cell. dont want to limit ourselves we want this to be extensivble
        private Cell[,] spreadSheet; // this not instantiating a class so not thropwing an error even if cell is abstract

        // private SpreadSheetCell[,]

        // public event PropertyChangedEventHandler CellPropertyChanged = delegate { }; //c# way of blank function

        /// <summary>
        /// Initializes a new instance of the <see cref="Spreadsheet"/> class.
        /// </summary>
        /// <param name="rowIndex">Specifies the number of rows in spreadsheet.</param>
        /// <param name="columnIndex">Specifies the number of columns in spreadsheet.</param>
        /// <exception cref="ArgumentException">Throws an exception if rows and columns passed in have negative values.</exception>
        public Spreadsheet(int rowIndex, int columnIndex)
        {
            if (rowIndex <= 0 || columnIndex <= 0)
            {
                throw new ArgumentException("Rows and columns must be positive integers.");
            }

            this.spreadSheet = new Cell[rowIndex, columnIndex];
            this.rowCount = rowIndex;
            this.columnCount = columnIndex;
            this.variables = new Dictionary<string, Cell>();

            for (int i = 0; i < rowIndex; i++)
            {
                for (int j = 0; j < columnIndex; j++)
                {
                    // When instnatiating, may wanna wuse the concrete class
                    this.spreadSheet[i, j] = new SpreadSheetCell(i, j); // Spreadsheet is subscribning to the Cell

                    // Spreadhseet class is subscribing to the property changed event from each one of the cells created.
                    // PropertyChanged is a list of function popinters
                    // This is not invoking OnCellpropertyChange yet!
                    // The moment when we create a spreadsheet cell object  OnCellPropertyChanged will get registered
                    // PropertyChanged is an event. Cell doesn't know care who listens to its events or what they do.
                    this.spreadSheet[i, j].PropertyChanged += this.OnCellPropertyChanged; // Notify external listeners  subscribing to an event

                    // this.spreadSheet[i, j].PropertyChanged += this.UpdateCellValue;
                    // The Spreadsheet subscribes to the Cell's PropertyChanged event by attaching an event handler.
                    // When a Cell changes, the Spreadsheet receives a notification and executes OnCellPropertyChanged
                    // ProperyChanged will call OnCellPropertyChanged  just ing adding to the list that will get called
                    // char letter = (char)('A' + j);
                    // string location = letter.ToString() + (i + 1).ToString();
                    // this.variables.Add(location, "0");

                    // Registering another color event
                    this.spreadSheet[i, j].PropertyChanged += this.ColorCellPropertyChanged;
                }
            }
        }

        /// <summary>
        /// Once invoked, this will notify its subscribers calling a list of pointers inside.
        /// </summary>
        public event PropertyChangedEventHandler CellPropertyChanged = (sender, args) => { }; // I am setting a function that takes sender object and args object.

        // Delegate: list of function? event must take signuatres args that inherit from Event Args class

        /// <summary>
        /// Gets ColumnCount property that is read only.
        /// </summary>
        public int ColumnCount
        {
            get { return this.columnCount; }
        }

        /// <summary>
        /// Gets RowCount property that is read only.
        /// </summary>
        public int RowCount
        {
            get { return this.rowCount; }
        }

        /// <summary>
        /// Gets variables dictionary.
        /// </summary>
        public Dictionary<string, Cell> Variables
        {
            get { return this.variables; }
        }

        /// <summary>
        /// Funtion to retrieve a cell from the spreadsheet.
        /// I declared it as nullable, using?.
        /// Row index and Column Index always starts at zero.
        /// </summary>
        /// <param name="rowIndex">rowsIndex retrieves a row position of a cell.</param>
        /// <param name="columnIndex">columnIndex retrieves a column position of a cell.</param>
        /// <returns>Abstract cell class.</returns>
        public Cell? GetCell(int rowIndex, int columnIndex)
        {
            if (rowIndex >= 0 && columnIndex >= 0 && rowIndex < this.rowCount && columnIndex < this.columnCount)
            {
                return this.spreadSheet[rowIndex, columnIndex];
            }

            return null;
        }

        /// <summary>
        /// Clears up the whole data in the spreadsheet.
        /// </summary>
        public void ClearSpreadsheetData()
        {
            if (this.Variables.Count == 0)
            {
                Console.WriteLine("Spreadsheet data is already empty!");
                return;
            }

            foreach (Cell cell in this.Variables.Values)
            {
                cell.Text = string.Empty;
                cell.BGColor = 0xFFFFFFFF; // setting it to a default value.
            }

            this.Variables.Clear(); // Clear up the dictionary of the updated cells!
        }

        /// <summary>
        /// Event handler that listens for changes to a Cell object's properties.
        /// where it updates the acutal cell value.
        /// </summary>
        /// <param name="sender">sender indicates where the event is coming from. It is a specific cell in our case.</param>
        /// <param name="e">The type of the event that is happening.</param>
        private void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // for loop list of dependent cell vale andthat upadtaes  to let the spreadhset know hwich  things to update.
            // to update the value. here it should directly change the dependent cell text so that it fires the3 evne.t

            // nameof(Cell.Text) is a safe way to refer to the property name.
            // if (e.PropertyName == nameof(Cell.Text)) //Checks if the property that ahs changesd is Cell.Text

            // nameof(SpreadSheetCell.Text)
            // typecheck  if the sender is SpreadsheetCell assign a new variable called cell
            // the event arg thatw egot earlier the name of
            if (sender is SpreadSheetCell cell && e.PropertyName == "Text")
            {
                // cells dont have an expression tree  expresion trree trtiggered when it starts with =
                this.UpdateCellValue(cell); // the name of event argument = "Values"

                // Notifying External Listeners
                // This ->current object  in this case it refers to spreadsheet   the cell obhect we should pass
                this.CellPropertyChanged?.Invoke(cell, new PropertyChangedEventArgs("Values")); // Notify external listeners in our case it will be the UI class

                // Invoking this SheetPropertyChanged.
                // A question to ask yourself: Subscribed to the CellPropertyChanged:.
            }
        }

        // speartion of responsibility   takes a string and evaulates a logic

        /// <summary>
        /// Spreadsheet is the only one that should be able to update Cell Value.
        /// </summary>
        /// <param name="cell">cell where Text change occurs.</param>
        private void UpdateCellValue(SpreadSheetCell cell)
        {
            // somtehting to do with = ?
            // Rule 1. ext of the cell has just changed then the Spreadsheet is responsible for updating the Value of the cel
            if (!cell.Text.StartsWith("="))
            {
                cell.Value = cell.Text; // once update a particular cell value.
                this.UpdateVariables(cell.ColumnIndex, cell.RowIndex);
                this.UpdateRelatedCells(cell); // never update the text of tje cell
                return;

                // Value is interpreted by the engine.
                // Text is the one getting sent to the screen
                // Value is interpreted by the engine and sent it through the text
            }

            ExpressionTree tree = new ExpressionTree(cell.Text);

            foreach (var pair in this.Variables)
            {
                // still A1 and A10
                if (cell.Text.Contains(pair.Key))
                {
                    int colIndex = pair.Key[0] - 'A'; // index starts at 0.
                    int rowIndex = int.Parse(pair.Key.Substring(1)) - 1; // index starts at 0.
                    if (pair.Key[0] >= 'A' && pair.Key[0] <= 'Z' && int.Parse(pair.Key.Substring(1)) >= 1 && int.Parse(pair.Key.Substring(1)) <= this.rowCount + 1
                        && !this.GetCell(rowIndex, colIndex).DependentCells.Contains(cell))
                    {
                        // !this.GetCell(rowIndex, colIndex).DependentCells.Contains(cell)) additional check is needed to make sure while updating the dependent cell, the same cell reference won't be needed.
                        this.GetCell(rowIndex, colIndex).DependentCells.Add(cell);
                    }

                    // checks if its' numeric.
                    if (double.TryParse(pair.Value.Text, out _))
                    {
                        if (colIndex == cell.ColumnIndex && rowIndex == cell.RowIndex)
                        {
                            continue;
                        }

                        tree.SetVariable(pair.Key, pair.Value.Text);
                    }
                }
            }

            // After populating three based on the user expression entered, then we need to update
            // cell location with uopdated value stored in Spreadsheet class Variables dictionary.
            cell.Value = tree.Evaluate().ToString();
            this.UpdateVariables(cell.ColumnIndex, cell.RowIndex); // adding this line of code resolved this!
            return;
        }

        /// <summary>
        /// This will deal with cascade changes to all the related cells.
        /// </summary>
        /// <param name="cell">The cell where changes occured.</param> SpreadSheetCell cell
        private void UpdateRelatedCells(SpreadSheetCell cell) // object sender, PropertyChangedEventArgs e
        {
            foreach (var obj in cell.DependentCells)
            {
                obj.Text = obj.Text; // this should invoke the setter which invokes the event handler.
            }
        }

        /// <summary>
        /// Updates the Variables dictionary so that every spreadsheet cell's updated value can be stored in the dictionary.
        /// </summary>
        /// <param name="colNum">Add 1 to the column index, since the array index starts at 0.</param>
        /// <param name="rowNum">Add 1 to the row index, since the array index starts at 0.</param>
        private void UpdateVariables(int colNum, int rowNum)
        {
            string letter = ((char)(colNum + 65)).ToString(); // column A B C ...
            string key = letter + (rowNum + 1).ToString();

            // it raises an exception what the value was just Hello which is a string.
            // this.variables[key] = cellVal.
            this.variables[key] = this.spreadSheet[rowNum, colNum];

            return;
        }

        /// <summary>
        /// An event handler that responds to a change in the cell color property.
        /// </summary>
        /// <param name="sender">Sender is the cell in which color property is updated.</param>
        /// <param name="e">Color property changed event.</param>
        private void ColorCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // if the event is coming from SpreadSheetCell class and the event name is "Color"
            if (sender is SpreadSheetCell cell && e.PropertyName == "Color")
            {
                this.CellPropertyChanged?.Invoke(cell, new PropertyChangedEventArgs("ColorChanged")); // invoking another event handler to actually change the color in the UI.
            }
        }

        /// <summary>
        /// Init a reference to cell which is of an abstract class.  Private class: Nothinbg else should be used except for the Spreadsheet
        /// It's a private class defined inside of the spreadsheet class so that only the spreadsheet class can access to it.
        /// If its defined outside of the spreadsheet class on this page, nothing can get access to it.
        /// Private class should be inside of a class.
        /// </summary>
        private class SpreadSheetCell : Cell // Since This should only be used cell
        {
            // Private class: we want only spreadsheetclass who can use this obect   can only be accessed by declaring the,
            public SpreadSheetCell(int rowNum, int colNum)
                : base(rowNum, colNum)
            {
            }

            /// <summary>
            /// Gets or sets value is a property in SpreadSheetCell class.
            /// Warning: new keyword Tcreates a completely separate property in the derived class, unrelated to the base class.
            /// </summary>
            public string Value
            {
                get => this.value;
                set
                {
                    // Allow only the spreadsheet class to set the value
                    if (this.value != value)
                    {
                        this.value = value;
                    }
                }
            }
        }
    }
}
