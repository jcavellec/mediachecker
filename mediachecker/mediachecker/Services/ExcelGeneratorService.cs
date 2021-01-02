using System;
using System.Collections.Generic;
using System.IO;
using mediachecker.Models;
using OfficeOpenXml;

namespace mediachecker.Services
{
    public class ExcelGeneratorService
    {
        public static void GenerateExcelFile(List<MediaFileModel> objs, string outputFolder)
        {
            using var excel = new ExcelPackage();
            excel.Workbook.Worksheets.Add("MOVIES");
            var headerRow = new List<string[]>()
            {
                new string[] { "ID", "NAME", "DATE", "EXTENSION", "SIZE" }
            };
  
            // Determine the header range (e.g. A1:D1)
            string headerRange = "A1";
            string startingRange = "A3";

            // Target a worksheet
            var worksheet = excel.Workbook.Worksheets["MOVIES"];
                
            // Populate header row
            worksheet.Cells[headerRange].LoadFromArrays(headerRow);

            // Populate the rows
            worksheet.Cells[startingRange].LoadFromCollection(objs);

            FileInfo excelFile = new FileInfo(outputFolder+Path.DirectorySeparatorChar+@"ListOfMedia.xlsx");
            excel.SaveAs(excelFile);
        }
    }
}