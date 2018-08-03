using ProductCatalog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace OnlineCatalogApp.Helper
{
    public static class ClientHelper
    {
        public static async Task<HttpResponseMessage> CallApi(string baseUrl, Object dataObject, string dataObjectType, HTTPMODE mode, int id = 0)
        {
            HttpResponseMessage response;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                switch (mode)
                {
                    case HTTPMODE.POST:
                        response = await client.PostAsJsonAsync(dataObjectType, dataObject);
                        break;
                    case HTTPMODE.GET:
                        response = await client.GetAsync(baseUrl + "/" + id);
                        break;
                    case HTTPMODE.PUT:
                        response = await client.PutAsJsonAsync(baseUrl + "/" + id, dataObject);
                        break;
                    case HTTPMODE.DELETE:
                        response = await client.DeleteAsync(baseUrl + "/" + id);
                        break;
                    default:
                        response = await client.GetAsync(baseUrl);
                        break;
                }

                return response;
            }
        }
        public static async Task<HttpResponseMessage> CallApi(string baseUrl, int pageNumber, int pageSize)
        {
            HttpResponseMessage response;
            UriBuilder builder = new UriBuilder(baseUrl);
            builder.Query = "pageNumber=" + pageNumber + "&pageSize=" + pageSize;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                response = await client.GetAsync(builder.Uri);
            }

           return response;
        }
    }
}

