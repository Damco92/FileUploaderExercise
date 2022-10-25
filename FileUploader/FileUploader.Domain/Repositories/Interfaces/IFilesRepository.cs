using FileUploader.Domain.Models;
using System;
using System.Collections.Generic;

namespace FileUploader.Domain.Repositories.Interfaces
{
    public interface IFilesRepository
    {
        File GetFileById(int fileId);
        IEnumerable<File> GetAllFiles();
        File GetLastCreatedFile();
        void InsertFileToDatabase(File file);
    }
}
