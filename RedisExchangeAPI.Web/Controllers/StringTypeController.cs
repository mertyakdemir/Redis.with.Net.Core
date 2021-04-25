using Microsoft.AspNetCore.Mvc;
using RedisExchangeAPI.Web.Services;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisExchangeAPI.Web.Controllers
{
    public class StringTypeController : Controller
    {
        private readonly RedisService _redisService;
        private readonly IDatabase db;

        public StringTypeController(RedisService redisService)
        {
            _redisService = redisService;
            db = _redisService.GetDb(0);
        }

        public IActionResult Index()
        {
            var db = _redisService.GetDb(0);
            db.StringSet("name", "Jack");
            db.StringSet("visitors", 1000);
            return View();
        }

        public IActionResult Show()
        {
            var value = db.StringGet("name");

            //db.StringIncrement("visitors", 10);

            var count = db.StringDecrementAsync("visitors", 1).Result;

            if (value.HasValue)
            {
                ViewBag.Name = value.ToString();
            }

            return View();
        }
    }
}
