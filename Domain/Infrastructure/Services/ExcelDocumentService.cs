using Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Infrastructure
{
    public class ExcelDocumentService : IExcelDocumentService
    {
        public async void ReadFileAsync()
        {
            //var dtContent = await FileProcessAsync("C:\\Users\\CMNB-63-002\\Desktop\\ทดสอบ กยศ.xlsx");
            //foreach (DataRow dr in dtContent.Rows)
            //{
            //    foreach(DataColumn column in dr.ItemArray)
            //    {
            //        Console.WriteLine(dr[0]);
            //    }              
            //}
        }

        public async Task<DataTable> FileProcessAsync(IFormFile file, bool hasHeader = true, int hasHeaderRowStart = 0, bool hasFooter = true, int hasFooterEnd = 0)
        {
            DataTable tbl = new DataTable();
            //if (File.Exists(path))
            //{
                string extension = Path.GetExtension(file.FileName).ToLower();
                if(extension.Equals(".xlsx"))
                {
                    using (var pck = new OfficeOpenXml.ExcelPackage())
                    {
                        using (var stream = file.OpenReadStream())
                        {
                            pck.Load(stream);
                        }
                        var ws = pck.Workbook.Worksheets.First();

                        foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                        {
                            tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                        }
                        var startRow = hasHeader ? hasHeaderRowStart : 1;
                        var endRow = hasFooter ? (ws.Dimension.End.Row - hasFooterEnd) : ws.Dimension.End.Row;
                        for (int rowNum = startRow; rowNum <= endRow; rowNum++)
                        {
                            var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                            DataRow row = tbl.Rows.Add();
                            foreach (var cell in wsRow)
                            {
                                row[cell.Start.Column - 1] = cell.Text;
                            }
                        }
                    }
                } 
            //}
            return tbl;
        }

        public byte[] FileExcelExport(IEnumerable<dynamic> datas, bool hasHeader = true)
        {
            byte[] fileContents;

            using (var pkg = new OfficeOpenXml.ExcelPackage())
            {
                var worksheet = pkg.Workbook.Worksheets.Add("Sheet1");

                int rowDetail = 1;
                if (hasHeader)
                {
                    int headColumn = 1;
                    foreach (KeyValuePair<string, object> entry in datas.FirstOrDefault())
                    {
                        worksheet.Cells[1, headColumn].Value = entry.Key;
                        headColumn++;
                    }
                    rowDetail++;
                }

                foreach (var detail in datas)
                {
                    int colDetail = 1;
                    foreach (KeyValuePair<string, object> entry in detail)
                    {
                        worksheet.Cells[rowDetail, colDetail].Value = entry.Value;
                        colDetail++;
                    }
                    rowDetail++;
                }
                fileContents = pkg.GetAsByteArray();
            }

            return fileContents;
        }

        public string CheckSum()
        {
            using (var md5 = MD5.Create())
            {
                using (var stream = File.OpenRead(""))
                {
                    var hash = md5.ComputeHash(stream);
                    return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
                }
            }
        }
    }
}
