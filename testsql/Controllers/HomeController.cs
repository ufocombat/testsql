using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using testsql.Models;
using System.Data.SQLite;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace testsql.Controllers
{
    public class HomeController : Controller
    {
        private SQLiteConnection connection;
        private IHostingEnvironment _env;
        public List<String> items = new List<String>();

        public HomeController(IHostingEnvironment env)
        {
            _env = env;
        }

        public IActionResult Index()
        {
            var webRoot = _env.WebRootPath;
            var file = System.IO.Path.Combine(webRoot, "testsql.csproj.nuget.g.db");
           //var file2 = System.IO.Path.Combine(webRoot + "/images", "mydoc.jpg");

            connection = new SQLiteConnection($"Data Source={file}; Version=3");
            connection.Open();

            SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM ITEMS", connection);
            SQLiteDataReader reader = cmd.ExecuteReader();

            items.Clear();

            while (reader.Read()) {
                items.Add($"{reader[0]} - {reader[1]}");
            }

            ViewData["items"] = items;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
