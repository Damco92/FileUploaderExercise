using AutoMapper;
using FileUploader.Domain.Models;
using FileUploader.Web.ViewModels;

namespace FileUploader.Services.Helpers.Mapper
{
    public class FileMapper : Profile
    {
        public FileMapper()
        {
            CreateMap<File, FileViewModel>()
                    .ForMember(dest => dest.FileId, act => act.MapFrom(src => src.FileId))
                    .ForMember(dest => dest.FileName, act => act.MapFrom(src => src.FileName))
                    .ForMember(dest => dest.Created, act => act.MapFrom(src => src.Created))
                    .ForMember(dest => dest.OriginalData, act => act.MapFrom(src => src.FileData))
                    .ForMember(dest => dest.FileData, act => act.Ignore())
                    .ReverseMap();
        }
    }
}
