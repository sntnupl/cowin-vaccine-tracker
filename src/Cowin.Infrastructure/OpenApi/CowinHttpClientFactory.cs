using Cowin.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Cowin.Infrastructure.OpenApi
{
    public class CowinHttpClientFactory
    {
        //public static ICowinHttpClient Create(string serverBaseAddress)
        //{
        //    if (string.IsNullOrEmpty(serverBaseAddress)) throw new ArgumentNullException(serverBaseAddress);

        //    var httpClient = new HttpClient();
        //    httpClient.BaseAddress = new Uri(serverBaseAddress);
        //    httpClient.Timeout = new TimeSpan(0, 0, 60);
        //    httpClient.DefaultRequestHeaders.Clear();

            
        //    return new CowinHttpClient(httpClient);
        //}
    }
}
