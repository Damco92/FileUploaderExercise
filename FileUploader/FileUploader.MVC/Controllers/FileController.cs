using FileUploader.Services.Services.Interfaces;
using FileUploader.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace FileUploader.MVC.Controllers
{
    public class FileController : Controller
    {
        private readonly ILogger<FileController> _logger;
        private readonly IFileServices _fileServices;
        public FileController(ILogger<FileController> logger, IFileServices fileServices)
        {
            _logger = logger;
            _fileServices = fileServices;
        }

        public IActionResult Index()
        {
            var getAllFiles = _fileServices.GetAllFiles();
            var getLastFileRows = _fileServices.GetLastFileRows();
            return View("Index", new Tuple<List<FileViewModel>, List<FileRow>>(getAllFiles, getLastFileRows));
        }

        public IActionResult UploadFile()
        {
            return View("InsertFileForm");
        }

        [HttpPost]
        public IActionResult UploadFile(FileViewModel fileToBeUploaded)
        {
            fileToBeUploaded.Created = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            fileToBeUploaded.FileName = fileToBeUploaded.FileData.FileName;
            _fileServices.UploadFile(fileToBeUploaded);
            return RedirectToAction("Index");
        }
    }
}
