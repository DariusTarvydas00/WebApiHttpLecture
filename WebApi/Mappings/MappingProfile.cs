using WebApi.DataAccessLayer.Models;
using WebApi.ServiceLayer.DTOs;
using AutoMapper;

namespace WebApi.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, ResponseBookDto>()
                .ForMember(dest => dest.AverageRating, opt => opt.Ignore())
                .ForMember(dest => dest.ReviewCount, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<Book, RequestBookDto>()
                .ReverseMap();

            CreateMap<Review, ReviewDto>()
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Username))
               .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title))
               .ForMember(dest => dest.ReviewText, opt => opt.MapFrom(src => src.ReviewText))
               .ReverseMap();


        }
    }
}
