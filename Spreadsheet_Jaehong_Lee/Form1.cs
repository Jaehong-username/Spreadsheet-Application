// <copyright file="Form1.cs" company="Jaehong Lee">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Spreadsheet_Jaehong_Lee
{
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using SpreadsheetEngine;

    /// <summary>
    /// Defines an UI class when an user runs the program.
    /// </summary>
    public partial class Form1 : Form
    {
        private Spreadsheet spreadsheet;
        private SpreadSheetCommandControl spreadSheetCommandControl;

        /// <summary>
        /// Initializes a new instance of the <see cref="Form1"/> class.
        /// </summary>
        public Form1()
        {
            this.InitializeComponent();
            this.spreadsheet = new Spreadsheet(50, 26); // Creating a spreadsheet
            this.InitializeDataGrid();
            this.SubscribeToSpreadsheetEvents();

            this.spreadSheetCommandControl = new SpreadSheetCommandControl(); // loading two commands.
            this.SubscribeToSpreadsheetCommandControlEvents();
        }

        /// <summary>
        /// Once invoked, this will notify its subscribers calling a list of pointers inside.
        /// </summary>
        public event PropertyChangedEventHandler ClickChanged = (sender, e) => { };

        // public event PropertyChangedEventHandler ClickChanged = delegate { };

        /// <summary>
        /// Adds columns on spreadsheets from A to Z programtically
        /// Adds rows from 1 to 50.
        /// </summary>
        private void InitializeDataGrid()
        {
            // Add a listener to each cell for interaction with thre form.
            this.dataGridView1.Columns.Clear(); // To clear any columns that may have been created manually.
            for (char column = 'A'; column <= 'Z'; column++)
            {
                // first string column name and header
                this.dataGridView1.Columns.Add(column.ToString(), column.ToString());
            }

            for (int i = 0; i < 50; i++)
            {
                this.dataGridView1.Rows.Add();
                this.dataGridView1.Rows[i].HeaderCell.Value = (i + 1).ToString(); // Set row number
            }
        }

        /// <summary>
        /// This makes the UI class Subscribe to the Spreadsheet's CellPropertyChanged event.
        /// Once CellPropertyChanged invoked from the spreadhsheet class, Spreadsheet_CellPropertyChanged will get called.
        /// </summary>
        private void SubscribeToSpreadsheetEvents()
        {
            // telling GUI to listen to Spreadsheet CellPropertyChangedevent when ever it happens
            this.spreadsheet.CellPropertyChanged += this.Spreadsheet_CellPropertyChanged;
        }

        /// <summary>
        /// Handles spreadsheet cell changes and updates the DataGridView that we actually can see in the spreadsheet app.
        /// automatically once cell value is changed.
        /// </summary>
        private void Spreadsheet_CellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Sender should be cell instead of the spreadsheetso that it makes it easeier to copy the value where its sending singlas
            if (sender is Cell cell)
            {
                // Update DataGridView to reflect spreadsheet cell value changes
                // copy the value of the cell since thats what we want to show people
                // cell.Value is the updated one by the expression tree.
                // Now the user cell can see the the uopdted cell value on display
                this.dataGridView1.Rows[cell.RowIndex].Cells[cell.ColumnIndex].Value = cell.Value;
            }
        }

        /*
        /// <summary>
        /// Invoked when a cell content is clicked.
        /// </summary>
        /// <param name="sender">where the event is coming from.</param>
        /// <param name="e">event.</param>
        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Notify the spread sheet call
            this.ClickChanged?.Invoke(this, new PropertyChangedEventArgs("Click"));
        }
        */

        /// <summary>
        /// When the user starts editing a cell, this event is triggered.
        /// </summary>
        /// <param name="sender">source of the event. It is the DataGridView object that is raising the event.</param>
        /// <param name="e">event data. such as Row and Column which cell is being edited.</param>
        private void DataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            // cell has nothing to di with spreadsheet class. this cell here is a winform thing.
            var cell = this.dataGridView1[e.ColumnIndex, e.RowIndex];

            // while user typing the selcted cell it nothing happens

            // while editing, show this text property from the user input.
            cell.Value = this.spreadsheet.GetCell(e.RowIndex, e.ColumnIndex).Text; // Text property cant accept null values
        }

        /// <summary>
        /// When I hit enter. Finished edting.
        /// </summary>
        /// <param name="sender">sender is a cell selected.</param>
        /// <param name="e">event refers to the cell property changed.</param>
        private void DataGridView1_CellEndEditClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // var cell = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                // cell.Value = this.spreadsheet.GetCell(e.RowIndex, e.ColumnIndex).Text;

                // updating the actual spreadhsheet cell.
                string newText = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value?.ToString() ?? string.Empty;

                // ENcapsulate it in a command.
                ICommand command = new TextChangeCommand(this.spreadsheet.GetCell(e.RowIndex, e.ColumnIndex), newText);
                this.spreadSheetCommandControl.SetCommand(command);
            }
        }

        /// <summary>
        /// Demo Button.
        /// </summary>
        /// <param name="sender">Sender is a control button that triggered the event.</param>
        /// <param name="e">e defines an event which is a button click.</param>
        private void Button1_Click(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                this.SetTextRandomCells();
                this.SetTextColB();
                this.RefTextColB();
            }
        }

        /// <summary>
        /// This sets a text of "Hello, World" in about 50 random cells on the spread.
        /// </summary>
        private void SetTextRandomCells()
        {
            Random rand = new Random();
            for (int i = 0; i < 50; i++)
            {
                int randNum = rand.Next(0, 26);

                for (int j = 0; j < 26; j++)
                {
                    if (j == randNum)
                    {
                        // This works even if Cell is an abstract class.
                        Cell cell = this.spreadsheet.GetCell(i, j);
                        if (cell != null)
                        {
                            cell.Text = "Hello, World!";
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This will set the text to the entire column B.
        /// </summary>
        private void SetTextColB()
        {
            for (int i = 0; i < 50; i++)
            {
                Cell cell = this.spreadsheet.GetCell(i, 1); // Polymorphism using upcasting.

                // safely checks if cell retrival was successfu.
                if (cell != null)
                {
                    cell.Text = $"This is cell B{i + 1}";
                }
            }
        }

        /// <summary>
        /// This sets the entire column A to refer to columnB with =B# key word.
        /// </summary>
        private void RefTextColB()
        {
            for (int i = 0; i < 50; i++)
            {
                Cell cell = this.spreadsheet.GetCell(i, 0); // Polymorphism using upcasting

                // safely checks if cell retrival was successful.
                if (cell != null)
                {
                    cell.Text = $"=B{i + 1}";
                }
            }
        }

        /// <summary>
        /// Sets the the color of the selected cell.
        /// </summary>
        /// <param name="sender">source of the event.</param>
        /// <param name="e">click event itself.</param>
        private void ChangeBackgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ColorDialog dlg = new ColorDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    Color selectedColor = dlg.Color;
                    uint colorValue = (uint)selectedColor.ToArgb();

                    // dataGridView1.SelectedCells only include cells currently selected by the user.
                    foreach (DataGridViewCell selectedCell in this.dataGridView1.SelectedCells)
                    {
                        int row = selectedCell.RowIndex;
                        int col = selectedCell.ColumnIndex;

                        ICommand command = new ColorChangeCommand(this.spreadsheet.GetCell(row, col), colorValue);
                        this.spreadSheetCommandControl.SetCommand(command);
                    }
                }
            }
        }

        private void UpdateCellColor(object sender, PropertyChangedEventArgs e)
        {
            if (sender is Cell cell && e.PropertyName == "ColorChanged")
            {
                // Convert uInt back to Color value
                Color selectedColor = Color.FromArgb((int)cell.BGColor);
                this.dataGridView1[cell.ColumnIndex, cell.RowIndex].Style.BackColor = selectedColor;
            }
        }

        /// <summary>
        /// Event handler when an user clicks on the Undo Text or Color.
        /// </summary>
        /// <param name="sender">The menu clicked by the user.</param>
        /// <param name="e">User Click Event.</param>
        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.spreadSheetCommandControl.UndoMenuClicked();
        }

        /// <summary>
        /// Event handler when an user clicks on the Redo Text or Color.
        /// </summary>
        /// <param name="sender">The menu clicked by the user.</param>
        /// <param name="e">User Click Event.</param>
        private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.spreadSheetCommandControl.RedoMenuClicked();
        }

        /// <summary>
        /// Subscribe to spreadsheet command control events.
        /// </summary>
        private void SubscribeToSpreadsheetCommandControlEvents()
        {
            this.spreadSheetCommandControl.StackPropertyChanged += this.UpdateUndo;
            this.spreadSheetCommandControl.StackPropertyChanged += this.UpdateRedo;
            this.spreadsheet.CellPropertyChanged += this.UpdateCellColor;
        }

        private void UpdateUndo(object sender, PropertyChangedEventArgs e)
        {
            if (sender is SpreadSheetCommandControl && e.PropertyName == "TextUndo")
            {
                this.undoToolStripMenuItem.Text = "Undo text change";
                this.undoToolStripMenuItem.Enabled = true;
            }
            else if (sender is SpreadSheetCommandControl && e.PropertyName == "ColorUndo")
            {
                this.undoToolStripMenuItem.Text = "Undo background color change";
                this.undoToolStripMenuItem.Enabled = true;
            }
            else if (sender is SpreadSheetCommandControl && e.PropertyName == "DisableUndo")
            {
                this.undoToolStripMenuItem.Text = "Undo";
                this.undoToolStripMenuItem.Enabled = false;
            }
        }

        private void UpdateRedo(object sender, PropertyChangedEventArgs e)
        {
            if (sender is SpreadSheetCommandControl && e.PropertyName == "TextRedo")
            {
                this.redoToolStripMenuItem.Text = "Redo text change";
                this.redoToolStripMenuItem.Enabled = true;
            }
            else if (sender is SpreadSheetCommandControl && e.PropertyName == "ColorRedo")
            {
                this.redoToolStripMenuItem.Text = "Redo background color change";
                this.redoToolStripMenuItem.Enabled = true;
            }
            else if (sender is SpreadSheetCommandControl && e.PropertyName == "DisableRedo")
            {
                this.redoToolStripMenuItem.Text = "Redo";
                this.redoToolStripMenuItem.Enabled = false;
            }
        }

        /// <summary>
        /// Loads the spreadsheet file.
        /// </summary>
        /// <param name="sender">User click.</param>
        /// <param name="e">Event when user clicks on Load.</param>
        private void LoadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog loadFileDialog = new OpenFileDialog())
            {
                loadFileDialog.Filter = "XML files (*.xml)|*.xml";
                loadFileDialog.Title = "Load XML File";
                loadFileDialog.RestoreDirectory = true;

                if (loadFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Creates a new StreamReader object to read from the newly opened file
                    using (StreamReader sr = new StreamReader(loadFileDialog.FileName))
                    {
                        // Call this method and update the spreadshhet.
                        SpreadsheetDataStorage.FileLoad(sr, this.spreadsheet);
                    }
                }
            }
        }

        /// <summary>
        /// Saves the spreadsheet.
        /// </summary>
        /// <param name="sender">User click.</param>
        /// <param name="e">Event when user clicks on Save.</param>
        private void SaveFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Code related to saving in the UI such as saveFileDialog.
            using (SaveFileDialog saveFileDialog = new SaveFileDialog()) // Create SaveFileDialog object.
            {
                saveFileDialog.Filter = "XML files (*.xml)|*.xml"; // save only xml files.
                saveFileDialog.Title = "Save XML File";
                saveFileDialog.InitialDirectory = "c:\\"; // set the initial directory when saving.
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // create streamwrite object to save it in a xml file.
                    using (StreamWriter outStream = new StreamWriter(saveFileDialog.FileName))
                    {
                        SpreadsheetDataStorage.FileSave(outStream, this.spreadsheet);
                    }

                    MessageBox.Show($"The spreadsheet was successfully saved to {saveFileDialog.FileName}!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}