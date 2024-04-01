using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
  public interface IExcelDocumentService
  {
    void ReadFileAsync();
    Task<DataTable> FileProcessAsync(IFormFile file, bool hasHeader = true, int hasHeaderRowStart = 0, bool hasFooter = true, int hasFooterEnd = 0);
    byte[] FileExcelExport(IEnumerable<dynamic> datas, bool hasHeader);
  }
}
