using FileUploader.Web.ViewModels;
using System;
using System.Collections.Generic;

namespace FileUploader.Services.Services.Interfaces
{
    public interface IFileServices
    {
        FileViewModel GetFileById(int fileId);
        List<FileViewModel> GetAllFiles();
        void UploadFile(FileViewModel fileDTO);
        List<FileRow> GetLastFileRows();
    }
}
