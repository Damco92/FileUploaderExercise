using Microsoft.AspNetCore.Http;
using System;

namespace FileUploader.Web.ViewModels
{
    public class FileViewModel
    {
        public int FileId { get; set; }
        public string FileName { get; set; }
        public byte[] OriginalData { get; set; }
        public IFormFile FileData { get; set; }
        public string Created { get; set; }
    }
}
