using static System.Runtime.InteropServices.JavaScript.JSType;
using WebApi.DataAccessLayer.Models;
using WebApi.ServiceLayer.DTOs;
using AutoMapper;

namespace WebApi.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BookModel, BookDto>()
                .ForMember(dest => dest.AverageRating, opt => opt.Ignore())
               .ForMember(dest => dest.ReviewCount, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<BookModel, AddBookDto>()
                .ReverseMap();
            CreateMap<BookModel, UpdateBookDto>()
                .ReverseMap();

            CreateMap<ReviewModel, ReviewDto>()
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
               .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title))
               .ForMember(dest => dest.ReviewText, opt => opt.MapFrom(src => src.ReviewText))
               .ReverseMap();


        }
    }
}
