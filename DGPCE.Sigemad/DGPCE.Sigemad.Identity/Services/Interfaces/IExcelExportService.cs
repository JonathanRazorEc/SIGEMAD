using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DGPCE.Sigemad.Application.Record;
namespace DGPCE.Sigemad.Identity.Services.Interfaces
{
    public interface IExcelExportService
    {
        Task<FileContentResult> ExportAsync(string title, List<ColumnInfo> columns, List<Dictionary<string, object>> data, List<string>? filtrosTexto = null);
    }
}
