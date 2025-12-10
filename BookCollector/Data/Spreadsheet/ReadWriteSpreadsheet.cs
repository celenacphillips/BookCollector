using System.Xml;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Cell = DocumentFormat.OpenXml.Spreadsheet.Cell;

namespace BookCollector.Data.Spreadsheet
{
    public class ReadWriteSpreadsheet
    {
        public static string CreateSpreadsheet(string folderPath)
        {
            var filename = $"{GetDate()}-{AppInfo.Current.Name.Replace(" ", string.Empty)}Export.xlsx";
            var filepath = $"{folderPath}/{filename}";

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

        public static List<List<string>> ReadSpreadSheet(string fileName, string sheetName)
        {
            List<List<string>> spreadsheetValues = [];

            // Open the spreadsheet document for read-only access.
            using (SpreadsheetDocument document = SpreadsheetDocument.Open(fileName, false))
            {
                // Retrieve a reference to the workbook part.
                WorkbookPart? workbookPart = document.WorkbookPart;

                // Find the sheet with the supplied name, and then use that
                // Sheet object to retrieve a reference to the first worksheet.
                Sheet? theSheet = workbookPart?.Workbook.Descendants<Sheet>().Where(s => s.Name == sheetName).FirstOrDefault();

                // Throw an exception if there is no sheet.
                if (theSheet is null || theSheet.Id is null)
                {
                    return spreadsheetValues;
                }

                // Retrieve a reference to the worksheet part.
                WorksheetPart worksheetPart = (WorksheetPart)workbookPart!.GetPartById(theSheet.Id!);
                List<Row?>? rows = worksheetPart.Worksheet?.Descendants<Row?>()?.ToList();

                var columnCount = 0;

                if (rows != null)
                {
                    foreach (var row in rows)
                    {
                        if (row != null)
                        {
                            if (row.RowIndex == 1)
                            {
                                columnCount = row.Descendants<Cell>().Count();
                            }

                            if (row.RowIndex != 1)
                            {
                                List<string> cellValues = [];

                                var theCells = row.Descendants<Cell>()?.ToList();

                                int columnIndex = 0;

                                if (theCells != null)
                                {
                                    // TO DO
                                    // There is a bug here for some books. It will add unnecessary empty cell values between the Image string.
                                    foreach (Cell theCell in theCells)
                                    {
                                        string columnValue = SetCurrentColumn(columnIndex);

                                        while (theCell.CellReference != $"{columnValue}{row.RowIndex}")
                                        {
                                            if (!columnValue.Equals("A"))
                                            {
                                                cellValues.Add(string.Empty);
                                                columnIndex++;
                                                columnValue = SetCurrentColumn(columnIndex);
                                            }
                                            else
                                            {
                                                columnValue += "A";
                                            }
                                        }

                                        if (theCell.CellReference == $"{columnValue}{row.RowIndex}")
                                        {
                                            var cellValue = GetCellValue(theCell, workbookPart);
                                            cellValues.Add(cellValue);
                                        }

                                        columnIndex++;
                                    }
                                }

                                if (cellValues.Count != 0)
                                {
                                    while (cellValues.Count <= columnCount)
                                    {
                                        cellValues.Add(string.Empty);
                                    }

                                    spreadsheetValues.Add(cellValues);
                                }
                            }
                        }
                    }
                }
            }

            return spreadsheetValues;
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

        private static string GetDate()
        {
            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month;
            var day = DateTime.Now.Day;

            return $"{year}{month.ToString().PadLeft(2, '0')}{day.ToString().PadLeft(2, '0')}";
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
                _ => "A",
            };
        }

        private static string SetCurrentColumn(int input)
        {
            var convertedInput = input;
            if (input > 26)
            {
                double convert = ((double)input % 26.0) - 1.0;
                convertedInput = (int)convert;
            }

            string? output = convertedInput switch
            {
                1 => "B",
                2 => "C",
                3 => "D",
                4 => "E",
                5 => "F",
                6 => "G",
                7 => "H",
                8 => "I",
                9 => "J",
                10 => "K",
                11 => "L",
                12 => "M",
                13 => "N",
                14 => "O",
                15 => "P",
                16 => "Q",
                17 => "R",
                18 => "S",
                19 => "T",
                20 => "U",
                21 => "V",
                22 => "W",
                23 => "X",
                24 => "Y",
                25 or -1 => "Z",
                _ => "A",
            };
            if (input > 26)
            {
                output = $"A{output}";
            }

            return output;
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
