using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using EquipmentDistribution.Models;

namespace EquipmentDistribution;

internal static class SpreadsheetActions
    {
        public static List<Equipment> ReadEquipmentSpreadsheet(string path)
        {
            var workbook = new XLWorkbook(path);
            var worksheet = workbook.Worksheet(1);

            List<string> columns = [];
            
            for (int i = 1; i <= worksheet.LastColumnUsed()!.ColumnNumber(); i++)
                columns.Add(worksheet.Read(1, i));

            int placeCol = columns.IndexOf("Помещение") + 1;
            int nameCol = columns.FindIndex(o => o.StartsWith("Наименование")) + 1;
            int countCol = columns.IndexOf("КОЛ-ВО на кабинет") + 1;
            int unitCol = columns.IndexOf("Ед. изм.") + 1;
            
            List<Equipment> equipments = [];
            
            for (int i = 2; i <= worksheet.LastRowUsed()!.RowNumber(); i++)
            {
                equipments.Add(new()
                {
                    Name = worksheet.Read(i, nameCol),
                    Place = worksheet.Read(i, placeCol),
                    CountPerClassroom = Convert.ToInt32(worksheet.Read(i, countCol)),
                    Unit = worksheet.Read(i, unitCol)
                });
            }
            
            workbook.Dispose();
            
            return equipments;
        }
        
        public static void SaveEquipmentSpreadsheet(List<DistributedEquipment> equipment, string model, string path)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(model);
            
            worksheet.Cell("A1").Value = "Кабинет";
            worksheet.Cell("B1").Value = "Наименование";
            worksheet.Cell("C1").Value = "Количество";
            worksheet.Cell("D1").Value = "Ед. изм.";

            worksheet.Cell("A2").InsertData(equipment.Select(o => o.Equipment.Classroom?.Name ?? ""));
            worksheet.Cell("B2").InsertData(equipment.Select(o => o.Equipment.Name));
            worksheet.Cell("C2").InsertData(equipment.Select(o => o.Count));
            worksheet.Cell("D2").InsertData(equipment.Select(o => o.Equipment.Unit));

            worksheet.Columns().AdjustToContents(10.0, 700.0);
            
            worksheet.Range(1, 1, equipment.Count + 1, 4).CreateTable();
            
            workbook.SaveAs(path);
            workbook.Dispose();
        }

        public static string Read(this IXLWorksheet ws, int row, int column) => ws.Cell(row, column).GetString();
    }