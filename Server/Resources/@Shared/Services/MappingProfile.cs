using AutoMapper;
using Server.Resources.Application.DTOs;
using Server.Resources.Application.Models;

namespace Server.Resources.Shared.Services
{
    public partial class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to API DTO
            CreateMap<Category, CategoryDTO>();
            CreateMap<Solution, SolutionDTO>();
            CreateMap<Category, CategoryResultDTO>();
            CreateMap<Solution, SolutionResultDTO>();

            // API DTO to Domain
            CreateMap<CategoryDTO, Category>();
            CreateMap<SolutionDTO, Solution>();
            CreateMap<SolutionResultDTO, Solution>();
            CreateMap<CategoryResultDTO, Category>();
        }
    }
}