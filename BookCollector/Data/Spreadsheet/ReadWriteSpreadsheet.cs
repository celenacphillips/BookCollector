// <copyright file="ReadWriteSpreadsheet.cs" company="Castle Software">
// Copyright (c) Castle Software. All rights reserved.
// </copyright>

namespace BookCollector.Data.Spreadsheet
{
    using System.Xml;
    using CommunityToolkit.Mvvm.ComponentModel;
    using DocumentFormat.OpenXml;
    using DocumentFormat.OpenXml.Packaging;
    using DocumentFormat.OpenXml.Spreadsheet;
    using Cell = DocumentFormat.OpenXml.Spreadsheet.Cell;

    /// <summary>
    /// ReadWriteSpreadsheet class.
    /// </summary>
    public class ReadWriteSpreadsheet : ObservableObject
    {
        /// <summary>
        /// Create a spreadsheet workbook with a given filename in a given folder path.
        /// </summary>
        /// <param name="folderPath">Folder path to create the spreadsheet workbook at.</param>
        /// <param name="filename">File name of spreadsheet workbook.</param>
        /// <returns>File path of created spreadsheet workbook.</returns>
        public static async Task<string> CreateSpreadsheet(string folderPath, string filename)
        {
            var filepath = $"{folderPath}/{filename}.xlsx";

            try
            {
                // Create a spreadsheet document by supplying the filepath.
                // By default, AutoSave = true, Editable = true, and Type = xlsx.
                SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Create(filepath, SpreadsheetDocumentType.Workbook);

                var coreFilePropPart = spreadsheetDocument.AddCoreFilePropertiesPart();

                // With DocumentFormat.OpenXml 2.14.0, AddCoreFilePropertiesPart includes an empty core.xml without a root which leads to an error when the generated file is opened in Excel
                using (XmlTextWriter writer = new (coreFilePropPart.GetStream(FileMode.Create), System.Text.Encoding.UTF8))
                {
                    writer.WriteRaw("<?xml version=\"1.0\" encoding=\"UTF-8\"?>\r\n<cp:coreProperties xmlns:cp=\"https://schemas.openxmlformats.org/package/2006/metadata/core-properties\"></cp:coreProperties>");
                    writer.Flush();
                }

                // Add a WorkbookPart to the document.
                WorkbookPart workbookpart = spreadsheetDocument.AddWorkbookPart();
                workbookpart.Workbook = new Workbook();

                // Add a WorksheetPart to the WorkbookPart.
                WorksheetPart worksheetPart = workbookpart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                workbookpart.Workbook.Save();

                // Close the document.
                spreadsheetDocument.Close();

                return filepath;
            }
            catch (UnauthorizedAccessException ex)
            {
                if (ex.Message.Equals($"Access to the path '{filepath}' is denied."))
                {
                    await CreateSpreadsheet(folderPath, $"{filename}-new");
                }

                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Writes to spreadsheet workbook at given file path with given list of items and table name.
        /// </summary>
        /// <param name="filePath">File path of spreadsheet workbook to write to.</param>
        /// <param name="itemsList">Values to write to the spreadsheet workbook.</param>
        /// <param name="tableName">Spreadsheet name.</param>
        public static void WriteToSpreadsheet(string filePath, List<List<string?>> itemsList, string tableName)
        {
            using SpreadsheetDocument spreadSheet = SpreadsheetDocument.Open(filePath, true);
            WorkbookPart workbookPart;

            if (spreadSheet.WorkbookPart == null)
            {
                workbookPart = spreadSheet.AddWorkbookPart();
            }
            else
            {
                workbookPart = spreadSheet.WorkbookPart;
            }

            // Insert a new worksheet.
            WorksheetPart worksheetPart = InsertWorksheet(workbookPart, tableName);
            SheetData sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

            int? rowValue = null;

            foreach (var items in itemsList)
            {
                rowValue = SetRow(rowValue);
                string? columnValue = null;

                Row row;
                row = new Row() { RowIndex = (uint)rowValue };
                sheetData.Append(row);

                foreach (var item in items)
                {
                    columnValue = SetNextColumn(columnValue);

                    // Add the cell to the cell table
                    Cell? refCell = null;
                    Cell newCell = new () { CellReference = $"{columnValue}{rowValue}" };
                    row.InsertBefore(newCell, refCell);

                    // Set the cell value
                    newCell.CellValue = new CellValue(item);
                    newCell.DataType = new EnumValue<CellValues>(CellValues.String);
                }
            }

            // Save the new worksheet.
            worksheetPart.Worksheet.Save();
        }

        /// <summary>
        /// Reads from spreadsheet workbook at given file path with given sheet name and list of column names.
        /// </summary>
        /// <param name="fileName">File path of spreadsheet workbook to read from.</param>
        /// <param name="sheetName">Spreadsheet name.</param>
        /// <param name="requiresMultipleSheets">Indicates whether multiple sheets are required in workbook.</param>
        /// <returns>Spreadsheet values and a message.</returns>
        public static (List<Dictionary<string, string>>, string) ReadSpreadSheet(string fileName, string sheetName, bool requiresMultipleSheets = false)
        {
            List<Dictionary<string, string>> spreadsheetValues = [];

            // Open the spreadsheet document for read-only access.
            using (SpreadsheetDocument document = SpreadsheetDocument.Open(fileName, false))
            {
                var workbookPart = document.WorkbookPart;

                var sheetCount = workbookPart?.Workbook.Descendants<Sheet>().Count() ?? 0;

                if (sheetCount == 0)
                {
                    return (spreadsheetValues, "No sheets found in the workbook.");
                }

                Sheet? sheet = null;

                if (sheetCount > 1)
                {
                    sheet = workbookPart?.Workbook
                        .Descendants<Sheet>()
                        .FirstOrDefault(s => s.Name.ToString().ToLower().Replace(" ", string.Empty).Equals(sheetName.ToLower().Replace(" ", string.Empty)));

                    sheet ??= workbookPart?.Workbook
                        .Descendants<Sheet>()
                        .FirstOrDefault(s => s.Name.ToString().ToLower().Replace(" ", string.Empty).Contains(sheetName.ToLower().Replace(" ", string.Empty)));
                }

                if ((sheetCount == 1 ||
                    sheet == null ||
                    sheet.Id == null) &&
                    !requiresMultipleSheets)
                {
                    sheet = workbookPart?.Workbook.Descendants<Sheet>().FirstOrDefault();
                    sheetName = sheet?.Name ?? sheetName;
                }

                if (sheet == null ||
                    sheet.Id == null)
                {
                    return (spreadsheetValues, $"Sheet '{sheetName}' not found in the workbook.");
                }

                var worksheetPart = (WorksheetPart)workbookPart!.GetPartById(sheet!.Id!);
                var rows = worksheetPart.Worksheet?.Descendants<Row?>()?.ToList();

                var columnCount = 0;

                if (rows != null)
                {
                    var columnNames = new List<string?>();

                    for (int r = 0; r < rows.Count; r++)
                    {
                        var row = rows[r];

                        if (row != null)
                        {
                            // Get the header row column names
                            if (r == 0)
                            {
                                var rowCells = row.Descendants<Cell>()?.ToList();

                                if (rowCells != null)
                                {
                                    columnCount = rowCells.Count;

                                    foreach (var cell in rowCells)
                                    {
                                        var cellValue = GetCellValue(cell, workbookPart).ToLower().Replace(" ", string.Empty);

                                        columnNames.Add(cellValue);
                                    }
                                }
                            }

                            columnCount = columnNames.Count;

                            if (r != 0)
                            {
                                Dictionary<string, string> cellValues = [];

                                var rowCells = row.Descendants<Cell>()?.ToList();

                                if (rowCells != null)
                                {
                                    foreach (var cell in rowCells)
                                    {
                                        int colIndex = GetColumnIndexFromReference(cell.CellReference);

                                        if (colIndex < columnNames.Count)
                                        {
                                            var columnName = columnNames[colIndex];

                                            if (!string.IsNullOrEmpty(columnName))
                                            {
                                                var cellValue = GetCellValue(cell, workbookPart);
                                                cellValues.Add(columnName, cellValue);
                                            }
                                        }
                                    }

                                    spreadsheetValues.Add(cellValues);
                                }
                            }
                        }
                    }
                }
            }

            return (spreadsheetValues, $"Import successful for {sheetName}.");
        }

        // Given a WorkbookPart, inserts a new worksheet.
        private static WorksheetPart InsertWorksheet(WorkbookPart workbookPart, string sheetName)
        {
            // Add a new worksheet part to the workbook.
            WorksheetPart newWorksheetPart = workbookPart.AddNewPart<WorksheetPart>();
            newWorksheetPart.Worksheet = new Worksheet(new SheetData());
            newWorksheetPart.Worksheet.Save();

            Sheets sheets = workbookPart.Workbook.GetFirstChild<Sheets>() ?? workbookPart.Workbook.AppendChild(new Sheets());
            string relationshipId = workbookPart.GetIdOfPart(newWorksheetPart);

            // Get a unique ID for the new sheet.
            uint sheetId = 1;
            if (sheets.Elements<Sheet>().Any())
            {
                sheetId = sheets.Elements<Sheet>().Select<Sheet, uint>(s =>
                {
                    if (s.SheetId is not null && s.SheetId.HasValue)
                    {
                        return s.SheetId.Value;
                    }

                    return 0;
                }).Max() + 1;
            }

            // Append the new worksheet and associate it with the workbook.
            Sheet sheet = new () { Id = relationshipId, SheetId = sheetId, Name = sheetName };
            sheets.Append(sheet);
            workbookPart.Workbook.Save();

            return newWorksheetPart;
        }

        private static string GetCellValue(Cell? theCell, WorkbookPart? workbookPart)
        {
            if (theCell is null || theCell.InnerText.Length < 0)
            {
                return string.Empty;
            }

            string? value = theCell.InnerText;

            // If the cell represents an integer number, you are done.
            // For dates, this code returns the serialized value that
            // represents the date. The code handles strings and
            // Booleans individually. For shared strings, the code
            // looks up the corresponding value in the shared string
            // table. For Booleans, the code converts the value into
            // the words TRUE or FALSE.
            if (theCell.DataType is not null)
            {
                if (theCell.DataType.Value == CellValues.SharedString)
                {
                    // For shared strings, look up the value in the
                    // shared strings table.
                    var stringTable = workbookPart?.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();

                    // If the shared string table is missing, something
                    // is wrong. Return the index that is in
                    // the cell. Otherwise, look up the correct text in
                    // the table.
                    if (stringTable is not null)
                    {
                        value = stringTable.SharedStringTable.ElementAt(int.Parse(value)).InnerText;
                    }
                }
                else if (theCell.DataType.Value == CellValues.Boolean)
                {
                    value = value switch
                    {
                        "0" => "FALSE",
                        _ => "TRUE",
                    };
                }
            }

            return value;
        }

        private static string SetNextColumn(string? input)
        {
            return input switch
            {
                "A" => "B",
                "B" => "C",
                "C" => "D",
                "D" => "E",
                "E" => "F",
                "F" => "G",
                "G" => "H",
                "H" => "I",
                "I" => "J",
                "J" => "K",
                "K" => "L",
                "L" => "M",
                "M" => "N",
                "N" => "O",
                "O" => "P",
                "P" => "Q",
                "Q" => "R",
                "R" => "S",
                "S" => "T",
                "T" => "U",
                "U" => "V",
                "V" => "W",
                "W" => "X",
                "X" => "Y",
                "Y" => "Z",
                "Z" => "AA",
                "AA" => "AB",
                "AB" => "AC",
                "AC" => "AD",
                "AD" => "AE",
                "AE" => "AF",
                "AF" => "AG",
                "AG" => "AH",
                "AH" => "AI",
                "AI" => "AJ",
                "AJ" => "AK",
                "AK" => "AL",
                "AL" => "AM",
                "AM" => "AN",
                "AN" => "AO",
                "AO" => "AP",
                "AP" => "AQ",
                "AQ" => "AR",
                "AR" => "AS",
                "AS" => "AT",
                "AT" => "AU",
                "AU" => "AV",
                "AV" => "AW",
                "AW" => "AX",
                "AX" => "AY",
                "AY" => "AZ",
                "AZ" => "BA",
                _ => "A",
            };
        }

        private static int GetColumnIndexFromReference(string? cellRef)
        {
            if (string.IsNullOrEmpty(cellRef))
            {
                return -1;
            }

            string letters = new ([.. cellRef.TakeWhile(char.IsLetter)]);

            int index = 0;
            foreach (char c in letters)
            {
                index = (index * 26) + (c - 'A' + 1);
            }

            return index - 1; // Return 0-based index
        }

        private static int SetRow(int? input)
        {
            if (input == null)
            {
                return 1;
            }
            else
            {
                var output = (int)input + 1;
                return output;
            }
        }
    }
}
