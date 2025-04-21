// <copyright file="SpreadsheetEngineTest.cs" company="Jaehong Lee">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngineTests
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading.Tasks;
    using SpreadsheetEngine;

    /// <summary>
    /// Tests public methods in Spreadsheet class.
    /// </summary>
    public class SpreadsheetEngineTest
    {
        /// <summary>
        /// This set up will be made before testing functions, if needed.
        /// </summary>
        [SetUp]
        public void Setup()
        {
        }

        /// <summary>
        /// This tests SpreadShhetNormalTest() case.
        /// </summary>
        [Test]
        public void SpreadsheetNormalTest()
        {
            // Create a spreadsheet 5 X 5 size
            Spreadsheet spreadSheet = new Spreadsheet(4, 5);
            Assert.That(spreadSheet.RowCount, Is.EqualTo(4));
            Assert.That(spreadSheet.ColumnCount, Is.EqualTo(5));
        }

        /// <summary>
        /// This tests to see if a constructor throws an exception when negative values are passed in for rows and columns.
        /// </summary>
        [Test]
        public void SpreadsheetExceptionalTest()
        {
            // Create a spreadsheet 0 X 0 size
            Assert.Throws<ArgumentException>(() => new Spreadsheet(-1, -2));
        }

        /// <summary>
        /// This tests GetCellNormalTest() case.
        /// </summary>
        [Test]
        public void GetCellNormalTest()
        {
            // Create a spreadsheet 5 X 5 size
            Spreadsheet spreadSheet = new Spreadsheet(5, 5);
            Assert.That(spreadSheet.GetCell(2, 4).RowIndex, Is.EqualTo(2));
            Assert.That(spreadSheet.GetCell(3, 4).ColumnIndex, Is.EqualTo(4));
        }

        /// <summary>
        /// This tests an exceptional case for array index = -1.
        /// </summary>
        [Test]
        public void GetCellExceptionalTestNegative()
        {
            // Create a spreadsheet 5 X 5 size
            Spreadsheet spreadSheet = new Spreadsheet(5, 5);
            Assert.That(spreadSheet.GetCell(5, -1), Is.Null);
        }

        /// <summary>
        /// This tests if GetCell() checks if an array index passed in is out of the array size or not.
        /// </summary>
        [Test]
        public void GetCellExceptionalTestOutArrBound()
        {
            // Create a spreadsheet 5 X 5 size
            Spreadsheet spreadSheet = new Spreadsheet(5, 5);
            Assert.That(spreadSheet.GetCell(5, 1), Is.Null);
        }

        /// <summary>
        /// This tests if varName correctly identifies a cell location name formatting or not.
        /// </summary>
        [Test]
        public void IsCellLocationTest()
        {
            Spreadsheet spreadSheet = new Spreadsheet(5, 5);
            Assert.That(spreadSheet.GetCell(1, 1).RowIndex, Is.EqualTo(1));
            Assert.That(spreadSheet.GetCell(1, 1).ColumnIndex, Is.EqualTo(1));
        }

        /// <summary>
        /// This tests if it executes text change command correctly or not.
        /// </summary>
        [Test]
        public void TextChangeCommandExecuteTest()
        {
            Spreadsheet spreadSheet = new Spreadsheet(5, 5);
            ICommand command = new TextChangeCommand(spreadSheet.GetCell(1, 1), "Hello");
            command.Execute();
            Assert.That(spreadSheet.GetCell(1, 1).Text, Is.EqualTo("Hello"));
        }

        /// <summary>
        /// This tests if it unexecutes text change command correctly or not.
        /// </summary>
        [Test]
        public void TextChangeCommandUnExecuteTest()
        {
            Spreadsheet spreadSheet = new Spreadsheet(5, 5);
            ICommand command = new TextChangeCommand(spreadSheet.GetCell(1, 1), "Hello");
            command.UnExecute();
            Assert.That(spreadSheet.GetCell(1, 1).Text, Is.EqualTo(string.Empty));
        }

        /// <summary>
        /// This tests if it executes background color change command correctly or not.
        /// </summary>
        [Test]
        public void ColorChangeCommandExecuteTest()
        {
            Spreadsheet spreadSheet = new Spreadsheet(5, 5);
            ICommand command = new ColorChangeCommand(spreadSheet.GetCell(1, 1), 0x000000FF);
            command.Execute();
            Assert.That(spreadSheet.GetCell(1, 1).BGColor, Is.EqualTo(0x000000FF));
        }

        /// <summary>
        /// This tests if it unexecutes background color change command correctly or not.
        /// </summary>
        [Test]
        public void ColorChangeCommandUnExecuteTest()
        {
            Spreadsheet spreadSheet = new Spreadsheet(5, 5);
            ICommand command = new ColorChangeCommand(spreadSheet.GetCell(1, 1), 0x000000FF);
            command.UnExecute();
            Assert.That(spreadSheet.GetCell(1, 1).BGColor, Is.EqualTo(0xFFFFFFFF));
        }

        /// <summary>
        /// This tests if the spreadsheet xml file is loaded correctly.
        /// </summary>
        [Test]
        public void SpreadSheetFileLoadTest()
        {
            using (StreamReader stream = new StreamReader("xml"))
            {
                // SpreadsheetDataStorage.FileLoad(stream, spreadSheet);
            }

            using (StreamReader reader = new StreamReader("xml"))
            {
                string content = reader.ReadToEnd();
                Assert.IsTrue(content.Contains("<spreadsheet>"));
                Assert.IsTrue(content.Contains("Hello"));
            }
        }

        /// <summary>
        /// This tests if the spreadsheet is saved to a xml file correctly.
        /// </summary>
        [Test]
        public void SpreadSheetFileSaveTest()
        {
            Spreadsheet spreadSheet = new Spreadsheet(5, 5);
            spreadSheet.GetCell(1, 1).Text = "Hello";
            spreadSheet.GetCell(1, 1).BGColor = 0xFFFFFFFF;

            using (StreamWriter stream = new StreamWriter("xml"))
            {
                SpreadsheetDataStorage.FileSave(stream, spreadSheet);
            }

            using (StreamReader reader = new StreamReader("xml"))
            {
                string content = reader.ReadToEnd();
                Assert.IsTrue(content.Contains("<spreadsheet>"));
                Assert.IsTrue(content.Contains("Hello"));
            }

            // save and load in one test case. where there is data and saave
            // clear the spreadsheet and load and check to see if the text that you saved was loaded
        }

        /// <summary>
        /// This tests if the method successfully clears up the spreadsheet.
        /// </summary>
        [Test]
        public void ClearSpreadSheetDataTestNormal()
        {
            Spreadsheet spreadSheet = new Spreadsheet(5, 5);

            spreadSheet.GetCell(1, 1).Text = "Hello";
            spreadSheet.GetCell(1, 2).Text = "Hello";
            spreadSheet.GetCell(1, 2).BGColor = 0x00FF00;
            spreadSheet.GetCell(1, 2).BGColor = 0xFFFF00;

            bool cleared = true;

            spreadSheet.ClearSpreadsheetData();

            foreach (var pair in spreadSheet.Variables)
            {
                if (pair.Value.Text != string.Empty || pair.Value.BGColor.ToString() != "0xFFFFFFFF")
                {
                    cleared = false;
                    break;
                }
            }

            Assert.IsEmpty(spreadSheet.Variables);
            Assert.IsTrue(cleared);
        }
    }
}