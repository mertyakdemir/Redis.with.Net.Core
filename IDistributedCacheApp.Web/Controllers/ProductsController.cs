using IDistributedCacheApp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IDistributedCacheApp.Web.Controllers
{
    public class ProductsController : Controller
    {
        private IDistributedCache _distributedCache;

        public ProductsController(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task<IActionResult> Index()
        {
            DistributedCacheEntryOptions cacheEntryOptions = new DistributedCacheEntryOptions();

            cacheEntryOptions.AbsoluteExpiration = DateTime.Now.AddMinutes(1);

            //_distributedCache.SetString("name", "John", cacheEntryOptions);

            Product product = new Product { Id = 1, Name = "Computer", Price = 1000 };

            string jsonProduct = JsonConvert.SerializeObject(product);

            await _distributedCache.SetStringAsync("product:1", jsonProduct, cacheEntryOptions);

            return View();
        }

        public IActionResult Show()
        {
            //string name = _distributedCache.GetString("name");

            string jsonProduct = _distributedCache.GetString("product:1");
            Product product = JsonConvert.DeserializeObject<Product>(jsonProduct);

            ViewBag.Product = product;
            return View();
        }

        public IActionResult Remove()
        {
            _distributedCache.Remove("product:1");

            return View();
        }

        public IActionResult ImageCache()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/1.jpg");

            byte[] imageByte = System.IO.File.ReadAllBytes(path);

            _distributedCache.Set("image", imageByte);

            return View();
        }

        public IActionResult ImageUrl()
        {
            byte[] imgByte = _distributedCache.Get("image");

            return File(imgByte, "image/jpg");
        }

    }
}
