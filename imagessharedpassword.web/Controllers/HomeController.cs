using imagessharedpassword.web.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using imagessharedpassword.Data;
using System.IO;

namespace imagessharedpassword.web.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString =
           @"Data Source=.\sqlexpress;Initial Catalog=ImagesDemo;Integrated Security=true;";

        private readonly IWebHostEnvironment _environment;
        public IActionResult Index()
        {
            return View();
        }
        public HomeController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpPost]
        public IActionResult AddImage(IFormFile myImage, string password)
        {
            Guid guid = Guid.NewGuid();
            string fileName = $"{guid}-{myImage.FileName}";
            string finalFileName = Path.Combine(_environment.WebRootPath, "uploads", fileName);
            using var fs = new FileStream(finalFileName, FileMode.CreateNew);
            myImage.CopyTo(fs);
            var db = new ShareImagesDB(_connectionString);
            Image image = new()
            {
                ImageName = fileName,
                Password = password
            };
            db.AddImage(image);
            ImageViewModel vm = new()
            {
                ImageId = image.Id,
                Password = password
            };
            return View(vm);
        }
        public IActionResult View(int id)
        {
            var db = new ShareImagesDB(_connectionString);
            var ids = HttpContext.Session.Get<List<int>>("Ids");
            if (ids == null)
            {
                ids = new List<int>();
            }
            var vm = new ShowImageViewModel()
            {
                Id = id,
                Ids = ids
            };
            if (ids.Contains(id))
            {
                Image image = db.GetImage(id);
                vm.Image = image;
                vm.ShowImage = true;
                db.UpdateViews(image);
            }
            return View(vm);
        }
        [HttpPost]
        public IActionResult View(int id, string password)
        {
            var db = new ShareImagesDB(_connectionString);
            Image image = db.GetImage(id);
            var ids = HttpContext.Session.Get<List<int>>("Ids");
            if (ids == null)
            {
                ids = new List<int>();
            }
            if (image.Password == password)
            {
                ids.Add(id);
                HttpContext.Session.Set("Ids", ids);
                
            }
            else
            {
                TempData["message"] = "Invalid password";
            }
            return Redirect($"/home/View?id={id}");
        }
    }
}
