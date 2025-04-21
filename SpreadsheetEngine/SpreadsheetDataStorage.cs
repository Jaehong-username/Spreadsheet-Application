// <copyright file="SpreadsheetDataStorage.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace SpreadsheetEngine
{
    using System;
    using System.Collections.Generic;
    using System.Formats.Tar;
    using System.IO;
    using System.Linq;
    using System.Reflection.PortableExecutable;
    using System.Runtime.CompilerServices;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using System.Xml;
    using SpreadsheetEngine;

    /// <summary>
    /// Responsible for managing data to properly implement load and save methods.
    /// It keeps the logic separate from the UI and spreadsheet engine.
    /// </summary>
    public class SpreadsheetDataStorage
    {
        /// <summary>
        /// Loads the spreadsheet, reading the xml file.
        /// Loading    same but doing reverse.
        /// </summary>
        /// <param name="stream">the file path of xml to be loaded to.</param>
        /// <param name="spreadsheet">the spreadsheet where the xml file is loaded to.</param>
        public static void FileLoad(StreamReader stream, Spreadsheet spreadsheet)
        {
            // First clear up the spreadsheet.
            spreadsheet.ClearSpreadsheetData();

            using (XmlReader xmlReader = XmlReader.Create(stream))
            {
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType == XmlNodeType.Element && xmlReader.Name == "cell")
                    {
                        string? name = xmlReader.GetAttribute("name"); // gets name attribute only, the rest of ununsed tags get ignored.
                        int rowIndex = int.Parse(name.Substring(1)) - 1; // since index starts at 0.
                        int columnIndex = name[0] - 65; // A would become an asci code of 0

                        string text = string.Empty;
                        string color = string.Empty;

                        // Once you find <cell>, you need to keep reading until you finish that <cell> block.
                        // That’s where the inner loop comes in: it handles reading what's inside the cell.
                        while (xmlReader.Read())
                        {
                            if (xmlReader.NodeType == XmlNodeType.EndElement && xmlReader.Name == "cell")
                            {
                                break;
                            }

                            if (xmlReader.NodeType == XmlNodeType.Element)
                            {
                                switch (xmlReader.Name)
                                {
                                    case "text":
                                        text = xmlReader.ReadElementContentAsString(); // handles =A1+6
                                        break;
                                    case "bgcolor":
                                        color = xmlReader.ReadElementContentAsString(); // handles FF8000
                                        break;
                                    default:
                                        xmlReader.Skip(); // built in methos to skip all the unrelated tags.
                                        break;
                                }
                            }
                        }

                        spreadsheet.GetCell(rowIndex, columnIndex).Text = text;
                        spreadsheet.GetCell(rowIndex, columnIndex).BGColor = uint.Parse(color);
                    }
                }
            }
        }

        /// <summary>
        /// Saves the spreadsheet, writing to the xml file.
        /// </summary>
        /// <param name="stream">xml path that will be saved as.</param>
        /// <param name="spreadsheet">spreadsheet that is being saved.</param>
        public static void FileSave(StreamWriter stream, Spreadsheet spreadsheet)
        {
            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Indent = true,
                OmitXmlDeclaration = true,
                NewLineOnAttributes = false, // we don't want to put each attribute on a separate line
            };

            // whens aving sprradsheet write to a xmlwriter  can store as elements.
            XmlWriter xmlWriter = XmlWriter.Create(stream, settings); // passing a stream into this

            // xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("spreadsheet"); // <spreadsheet>.

            // will only iterate value part of the pair.
            foreach (var pair in spreadsheet.Variables)
            {
                xmlWriter.WriteStartElement("cell");
                xmlWriter.WriteAttributeString("name", pair.Key);

                xmlWriter.WriteElementString("bgcolor", pair.Value.BGColor.ToString());
                xmlWriter.WriteElementString("text", pair.Value.Text);

                xmlWriter.WriteEndElement(); // ending the cell element
            }

            xmlWriter.WriteEndElement(); // ending the spreadsheet element. </spreadsheet>.

            // xmlWriter.WriteEndDocument();
            xmlWriter.Close(); // make sure to close it!
        }
    }
}