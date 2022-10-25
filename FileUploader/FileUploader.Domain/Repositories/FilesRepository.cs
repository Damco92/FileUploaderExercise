using FileUploader.Domain.Context;
using FileUploader.Domain.Models;
using FileUploader.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FileUploader.Domain.Repositories
{
    public class FilesRepository : IFilesRepository
    {
        private FileUploaderDbContext _dbContext;

        public FilesRepository(FileUploaderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public File GetFileById(int fileId)
        {
            return _dbContext.Files.Find(fileId);
        }

        public IEnumerable<File> GetAllFiles()
        {
            return _dbContext.Files;
        }

        public void InsertFile(File file)
        {
            _dbContext.Files.Add(file);
            _dbContext.SaveChanges();
        }

        public File GetLastCreatedFile()
        {
            if (!_dbContext.Files.Any())
                return null;

            return _dbContext.Files.OrderByDescending(x => x.Created).First();
        }

        public void InsertFileToDatabase(File file)
        {
            _dbContext.Files.Add(file);
            _dbContext.SaveChanges();
        }
    }
}
