using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ProductCatalog
{
    public static class StatusResponse
    {
        public static HttpStatusCode StatusCode {get;set;}
        public static string Message { get; set; }
    }
}
