using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Payment.Api.HttpService
{
    public class InvoiceHttpClient
    {
        private readonly HttpClient httpClient;

        public InvoiceHttpClient(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task GetInvoiceByIdAsync(Guid invoiceId)
        {
            await httpClient.GetAsync("");
        }

    }
}
