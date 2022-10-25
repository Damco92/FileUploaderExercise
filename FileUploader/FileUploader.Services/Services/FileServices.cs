using AutoMapper;
using FileUploader.Domain.Repositories.Interfaces;
using FileUploader.Services.Services.Interfaces;
using FileUploader.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FileUploader.Services.Services
{
    public class FileServices : IFileServices
    {
        private IFilesRepository _fileRepository;
        private readonly IMapper _mapper;
        public FileServices(IFilesRepository fileRepository, IMapper mapper)
        {
            _fileRepository = fileRepository;
            _mapper = mapper;
        }
        public FileViewModel GetFileById(int fileId)
        {
            var file = _fileRepository.GetFileById(fileId);

            if(file == null)
            {
                throw new KeyNotFoundException("File is not found");
            }

            return _mapper.Map<FileViewModel>(file);
        }

        public List<FileViewModel> GetAllFiles()
        {
            var files = _fileRepository.GetAllFiles().OrderByDescending(x => x.Created).ToList();
            return _mapper.Map<List<FileViewModel>>(files);
        }

        public void UploadFile(FileViewModel fileVM)
        {
            byte[] bytes = new byte[4064];
            Domain.Models.File file = new Domain.Models.File();
            file.Created = DateTime.Now;
            file.FileName = fileVM.FileName;
            if (fileVM.FileData.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    fileVM.FileData.CopyTo(ms);
                    var fileText = Encoding.Default.GetString(ms.ToArray());
                    string[] lines = fileText.Split(new string[] { "\r\n", "\r","\n" },
                                        StringSplitOptions.None);
                    var linesThatWillBeAddedToFile = new List<string>();
                    var regex = new Regex(@"^(\d*),\s*([a-zA-Z]+),\s*([a-zA-Z]+)$");
                    foreach (var line in lines)
                    {
                        var match = regex.Match(line);
                        if (match.Success)
                        {
                            var groups = match.Groups;
                            var colors = Enum.GetNames(typeof(Domain.Models.Enums.Color));
                            var cities = Enum.GetNames(typeof(Domain.Models.Enums.City));
                            var isFirstGroupNumber = int.TryParse(groups[1].Value,out int result);
                            if(isFirstGroupNumber && colors.Any(x => x == groups[2].Value.ToLower()) && cities.Any(x => x == groups[3].Value.ToLower()))
                            {
                                linesThatWillBeAddedToFile.Add(line);
                                linesThatWillBeAddedToFile.Add(Environment.NewLine);
                            }
                        }
                    }
                    var filteredString = String.Join(String.Empty, linesThatWillBeAddedToFile);
                    var byteArrayFilteredText = Encoding.ASCII.GetBytes(filteredString);
                    bytes = byteArrayFilteredText;
                    ms.Close();
                }
            }
            file.FileData = Compress(bytes);
            _fileRepository.InsertFileToDatabase(file);
        }

        public List<FileRow> GetLastFileRows()
        {
            var file = _fileRepository.GetLastCreatedFile();

            if(file == null)
            {
                return null;
            }
            var decompressedFileData = Decompress(file.FileData);
            var fileText = Encoding.UTF8.GetString(decompressedFileData);
            List<string> lines = fileText.Split(new string[] { "\r\n", "\r", "\n" },
                                StringSplitOptions.None).ToList();
            var fileRows = new List<FileRow>();
            foreach (var line in lines)
            {
                if (!string.IsNullOrEmpty(line))
                {
                    string[] words = line.Split(',');
                    var fileRow = new FileRow
                    {
                        Id = Convert.ToInt32(words[0]),
                        Color = words[1],
                        City = words[2]
                    };
                    fileRows.Add(fileRow);
                }
            }
            return fileRows;
        }

        private byte[] Compress(byte[] data)
        {
            using (var compressedStream = new MemoryStream())
            {
                using (var zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
                {
                    zipStream.Write(data, 0, data.Length);
                    zipStream.Close();
                    return compressedStream.ToArray();
                }
            }
        }

        byte[] Decompress(byte[] data)
        {
            using (var compressedStream = new MemoryStream(data))
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            using (var resultStream = new MemoryStream())
            {
                zipStream.CopyTo(resultStream);
                return resultStream.ToArray();
            }
        }
    }
}
