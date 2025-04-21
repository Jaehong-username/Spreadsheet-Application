// <copyright file="SpreadSheetCommandControl.cs" company="Jaehong Lee">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This spreadsheet control loads all the related commands.
    /// </summary>
    public class SpreadSheetCommandControl
    {
        /// <summary>
        /// Stores Excute method for each of different command class that has inherits Command interface.
        /// In case an user pressed a button multiple times, each will be stored in stack.
        /// </summary>
        // public Queue<ICommand> CommandsQueue; // in case an user pressed a button multiple times, each will be stored in stack.

        /// <summary>
        /// Keeps track of the order of commands to implement Redo dunctionality in the spreadsheet.
        /// </summary>
        private Stack<ICommand> redoStack;

        /// <summary>
        /// Pops the command from the most recently implemented stack.
        /// </summary>
        private Stack<ICommand> undoStack;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpreadSheetCommandControl"/> class.
        /// </summary>
        public SpreadSheetCommandControl()
        {
            this.redoStack = new Stack<ICommand>();
            this.undoStack = new Stack<ICommand>();
        }

        /// <summary>
        /// Event that gets invoked when the stack changes.
        /// </summary>
        public event PropertyChangedEventHandler StackPropertyChanged = (sender, e) => { };

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="command">Command requested by the user.</param>
        public void SetCommand(ICommand command)
        {
            command.Execute();
            this.undoStack.Push(command); // only store the command on the UndoStack.

            this.UndoMenuUpdateEvent();
            this.RedoMenuUpdateEvent();

            // Can't redo after the most recently executed statement.
        }

        /// <summary>
        /// Undoes the most recently implemented command using pop from the undo stack.
        /// </summary>
        public void UndoMenuClicked()
        {
            if (this.undoStack.Count > 0)
            {
                ICommand mostRecentCommand = this.undoStack.Pop();
                mostRecentCommand.UnExecute();
                this.redoStack.Push(mostRecentCommand);
            }

            // we need to make sure to call both Event update methods.
            this.UndoMenuUpdateEvent();
            this.RedoMenuUpdateEvent();
        }

        /// <summary>
        /// Redoes the most recently implemented command using pop from the Redo stack.
        /// </summary>
        public void RedoMenuClicked()
        {
            if (this.redoStack.Count > 0)
            {
                ICommand mostRecentCommand = this.redoStack.Pop();
                mostRecentCommand.Execute();
                this.undoStack.Push(mostRecentCommand);
            }

            // we need to make sure to call both Event update methods. Otherwise, redo menu is not available.
            this.UndoMenuUpdateEvent();
            this.RedoMenuUpdateEvent();
        }

        /// <summary>
        /// Event that notifies of the change in the Undo Stack property.
        /// </summary>
        private void UndoMenuUpdateEvent()
        {
            if (this.undoStack.Count == 0)
            {
                this.StackPropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DisableUndo"));
            }
            else if (this.undoStack.Peek() is TextChangeCommand)
            {
                this.StackPropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TextUndo"));
            }
            else if (this.undoStack.Peek() is ColorChangeCommand)
            {
                this.StackPropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ColorUndo"));
            }
        }

        /// <summary>
        /// Event that notifies of the change in the Redo Stack property.
        /// </summary>
        private void RedoMenuUpdateEvent()
        {
            if (this.redoStack.Count == 0)
            {
                this.StackPropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DisableRedo"));
            }
            else if (this.redoStack.Peek() is TextChangeCommand)
            {
                this.StackPropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TextRedo"));
            }
            else if (this.redoStack.Peek() is ColorChangeCommand)
            {
                this.StackPropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ColorRedo"));
            }
        }
    }
}
