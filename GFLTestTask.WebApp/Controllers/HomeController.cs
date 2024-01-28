using GFLTestTask.Bll.DTO;
using GFLTestTask.Bll.Interfaces;
using GFLTestTask.WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Mime;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.Json;
using System.Threading.Tasks;
using System.Reflection.PortableExecutable;
using Newtonsoft.Json.Linq;

namespace GFLTestTask.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ITaskServices _taskService;



        public HomeController(ILogger<HomeController> logger, ITaskServices taskServices)
        {
            _logger = logger;
            _taskService = taskServices;
        }

        public IActionResult Index()
        {
            var result = _taskService.GetDataFromDB().ConfigureAwait(false).GetAwaiter().GetResult();
            if (result == null)
            {
                return BadRequest(string.Empty);
            }


            return View(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            ViewBag.id = id;

            var result = _taskService.GetDataFromDB().ConfigureAwait(false).GetAwaiter().GetResult();
            if (result == null)
            {
                return BadRequest(string.Empty);
            }
            return View(result);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult UploadFile()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadFileAsync(IFormFile fileToUpload)
        {
            try
            {
                if (fileToUpload != null && fileToUpload.Length > 0)
                {
                    var content = string.Empty;

                    using (var reader = new StreamReader(fileToUpload.OpenReadStream()))
                    {

                        if (fileToUpload.ContentType == "text/plain")
                        {
                            string line;
                            var list = new List<string>();
                            while ((line = reader.ReadLine()) != null)
                            {
                                list.Add(line);
                            }

                            string[] result = list.ToArray();
                            await _taskService.ReadInputTxtFile(result);
                        }
                        else
                        {
                            content = reader.ReadToEnd();
                            await _taskService.ReadInputJsonFile(content);
                        }
                    }

                    

                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Index");
                }
            }
            catch(Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

    }
}