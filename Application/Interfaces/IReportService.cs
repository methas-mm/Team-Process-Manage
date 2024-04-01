using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IReportService
    {
        Task<T> GetAsync<T>(string requestUrl);
        Task<byte[]> GetAsByteArrayAsync(string requestUrl);
        Task<T> PostAsync<T>(string requestUrl, object bodyContent);
        Task<byte[]> PostAsByteArrayAsync(string requestUrl, object bodyContent);

        Task<dynamic> PostAsAsync(string requestUrl, object bodyContent);
        Task<string> GetAsyncReturnString(string requestUrl);
    }
}
