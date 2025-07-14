using DGPCE.Sigemad.Identity.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ClosedXML.Excel;

using DGPCE.Sigemad.Application.Record;

namespace DGPCE.Sigemad.Identity.Services
{
    public class ExcelExportService : IExcelExportService
    {
        public async Task<FileContentResult> ExportAsync(string title, List<ColumnInfo> columns, List<Dictionary<string, object>> data, List<string>? filtrosTexto = null)
        {
            using var wb = new XLWorkbook();

            // 🛡️ Validar y limpiar nombre de hoja
            string CleanSheetName(string name)
            {
                var cleaned = System.Text.RegularExpressions.Regex.Replace(name, @"[:\\\/\?\*\[\]]", "_");
                if (cleaned.Length > 31) cleaned = cleaned[..31];
                return string.IsNullOrWhiteSpace(cleaned) ? "Hoja1" : cleaned;
            }

            var sheetName = CleanSheetName(title);
            int counter = 1;
            while (wb.Worksheets.Any(ws => ws.Name == sheetName))
            {
                string suffix = $"_{counter++}";
                int maxLen = 31 - suffix.Length;
                sheetName = CleanSheetName(title[..Math.Min(title.Length, maxLen)]) + suffix;
            }

            var ws = wb.Worksheets.Add(sheetName);
            int currentRow = 1;

            // 🖋️ Filtros aplicados (si hay)
            if (filtrosTexto?.Any() == true)
            {
                ws.Cell(currentRow++, 1).Value = "Filtros aplicados:";
                ws.Cell(currentRow - 1, 1).Style.Font.Bold = true;

                foreach (var filtro in filtrosTexto)
                {
                    ws.Cell(currentRow++, 1).Value = filtro;
                }

                currentRow++; // Línea en blanco antes del encabezado
            }

            // 🧾 Escribir encabezados
            for (int colIndex = 0; colIndex < columns.Count; colIndex++)
            {
                var col = columns[colIndex];
                var cell = ws.Cell(currentRow, colIndex + 1);
                cell.Value = col.Etiqueta ?? col.Columna;
                cell.Style.Font.Bold = true;
                cell.Style.Fill.BackgroundColor = XLColor.LightGray;
                cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            }

            // 🧮 Escribir datos
            foreach (var row in data)
            {
                currentRow++;
                for (int colIndex = 0; colIndex < columns.Count; colIndex++)
                {
                    var col = columns[colIndex];
                    var raw = row.TryGetValue(col.Columna, out var val) ? val?.ToString() : null;

                    string texto = col.Tipo switch
                    {
                        "bit" => (raw == "True" || raw == "1") ? "Sí" : "No",
                        "select" => row.TryGetValue(col.Columna + "__texto", out var valTexto) ? valTexto?.ToString() : raw,
                        _ => raw
                    };

                    ws.Cell(currentRow, colIndex + 1).Value = texto ?? "";
                }
            }

            // 📐 Ajustes visuales
            ws.Columns().AdjustToContents();
            ws.RangeUsed().Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            ws.RangeUsed().Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

            // 📦 Guardar archivo
            using var ms = new MemoryStream();
            wb.SaveAs(ms);
            var fileName = $"{sheetName}_{DateTime.UtcNow:yyyyMMdd_HHmm}.xlsx";

            return new FileContentResult(ms.ToArray(),
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                FileDownloadName = fileName
            };
        }

    }
}