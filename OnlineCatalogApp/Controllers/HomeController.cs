using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OnlineCatalogApp.Models;
using Microsoft.AspNetCore.Http;
using ProductCatalog.Models;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using X.PagedList.Mvc.Core;
using X.PagedList.Mvc.Common;
using X.PagedList;
using AutoMapper;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using Microsoft.AspNetCore.Hosting;
using OnlineCatalogApp.Helper;
using System.Data;

namespace OnlineCatalogApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        public HomeController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<IActionResult> Index(string searchField, string search, int? page)
        {
            List<Product> product = new List<Product>();

            HttpResponseMessage serviceResponse = await ClientHelper.CallApi(Common.BaseUrl, null, "", HTTPMODE.GETALL);

            //Checking the response is successful or not which is sent using HttpClient  
            if (serviceResponse.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api   
                var productResponse = serviceResponse.Content.ReadAsStringAsync().Result;
                product = JsonConvert.DeserializeObject<List<Product>>(productResponse);
            }

            if (searchField == "Name")
            {
                return View(product.Where(x => (search != null && x.Name.ToLower().Contains(search.ToLower())) || search == null)
                    .ToList().ToPagedList(page ?? 1, 5));
            }
            else
            {
                return View(product.Where(x => (search != null && x.Code.ToLower().Contains(search.ToLower())) || search == null)
                                   .ToList().ToPagedList(page ?? 1, 5));
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            Product product = new Product();

            HttpResponseMessage serviceResponse = await ClientHelper.CallApi(Common.BaseUrl, null, "", HTTPMODE.GET, id);

            //Checking the response is successful or not which is sent using HttpClient  
            if (serviceResponse.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api   
                var productResponse = serviceResponse.Content.ReadAsStringAsync().Result;
                product = JsonConvert.DeserializeObject<Product>(productResponse);
            }

            return View(product);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        //GET: ProductCatalog/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductCatalog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([FromForm] CatalogItem catalogItem)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    Product product = await MapProductObject(catalogItem);
                    HttpResponseMessage serviceResponse = await ClientHelper.CallApi(Common.BaseUrl, product, "Product", HTTPMODE.POST);
                    if (!serviceResponse.IsSuccessStatusCode)
                    {
                        ModelState.AddModelError("InsertError", "Unable to save changes. Please check all data entries and ensure product code is not dupicate.");
                        return View(product);
                    }
                }
            }
            catch
            {
                return View();
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: ProductCatalog/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            Product product = new Product();

            HttpResponseMessage serviceResponse = await ClientHelper.CallApi(Common.BaseUrl, null, "", HTTPMODE.GET, id);

            //Checking the response is successful or not which is sent using HttpClient  
            if (serviceResponse.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api   
                var productResponse = serviceResponse.Content.ReadAsStringAsync().Result;
                product = JsonConvert.DeserializeObject<Product>(productResponse);
            }

            return View(product);
        }

        // POST: ProductCatalog/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, [Bind("Id", "Name", "Price", "Code", "Photo", "RowVersion")] CatalogItem catalogitem)
        {
            Product product = await MapProductObject(catalogitem);

            HttpResponseMessage serviceResponse = await ClientHelper.CallApi(Common.BaseUrl, product, "Product", HTTPMODE.PUT, id);

            //Checking the response is successful or not which is sent using HttpClient  
            if (serviceResponse.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api   
                var apiResponse = serviceResponse.Content.ReadAsStringAsync().Result;
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("UpdateError", "Unable to save changes. This may be due to concurrent data updates. Please go back to the list and try again.");
                return View(product);
            }
        }

        private static async Task<Product> MapProductObject(CatalogItem catalogitem)
        {
            Product product;
            byte[] photo = Array.Empty<byte>();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Product, CatalogItem>();
            });

            IMapper iMapper = config.CreateMapper();
            var source = new CatalogItem();
            product = iMapper.Map<CatalogItem, Product>(catalogitem);
            product.LastUpdated = DateTime.UtcNow;

            if (product.Photo != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    await catalogitem.Photo.CopyToAsync(memoryStream);
                    photo = memoryStream.ToArray();
                }
                product.Photo = photo;
            }

            return product;
        }

        //// GET: ProductCatalog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            HttpResponseMessage serviceResponse = await ClientHelper.CallApi(Common.BaseUrl, null, "", HTTPMODE.DELETE, id);
            //Checking the response is successful or not which is sent using HttpClient  
            if (serviceResponse.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api   
                var productResponse = serviceResponse.Content.ReadAsStringAsync().Result;
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            Product product = new Product();
            HttpResponseMessage serviceResponse = await ClientHelper.CallApi(Common.BaseUrl, null, "", HTTPMODE.GET, id);

            //Checking the response is successful or not which is sent using HttpClient  
            if (serviceResponse.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api   
                var productResponse = serviceResponse.Content.ReadAsStringAsync().Result;
                product = JsonConvert.DeserializeObject<Product>(productResponse);

            }
            return View(product);
        }

        public async Task<IActionResult> FileReport()
        {
            List<Product> product = new List<Product>();
            HttpResponseMessage serviceResponse = await ClientHelper.CallApi(Common.BaseUrl, null, "", HTTPMODE.GETALL);

            //Checking the response is successful or not which is sent using HttpClient  
            if (serviceResponse.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api   
                var productResponse = serviceResponse.Content.ReadAsStringAsync().Result;
                product = JsonConvert.DeserializeObject<List<Product>>(productResponse);
            }

            using (var package = ExcelHelper.CreateExcelPackage<Product>(product))
            {
                package.SaveAs(new FileInfo(Path.Combine(_hostingEnvironment.WebRootPath, Common.ExportFolder, Common.FileDownLoadName)));
            }

            return File($"~/{Common.ExportFolder}/{Common.FileDownLoadName}", Common.XlsxContentType, Common.FileDownLoadName);

        }
    }
}
