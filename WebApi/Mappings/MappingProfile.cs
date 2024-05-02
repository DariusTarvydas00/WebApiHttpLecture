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
                .ForMember(dest => dest.ISBN, opt => opt.MapFrom(src => src.ISBN))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author))
                .ForMember(dest => dest.PublicationYear, opt => opt.MapFrom(src => src.PublicationYear))
                .ForMember(dest => dest.AverageRating, opt => opt.Ignore())
                .ForMember(dest => dest.ReviewCount, opt => opt.Ignore())
                .ForMember(dest => dest.Publisher, opt => opt.Ignore())
                .ForMember(dest => dest.Image_URL_S, opt => opt.Ignore())
                .ForMember(dest => dest.Image_URL_M, opt => opt.Ignore())
                .ForMember(dest => dest.Image_URL_L, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<Book, RequestBookDto>()
                .ForMember(dest => dest.ISBN, opt => opt.MapFrom(src => src.ISBN))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Author))
                .ForMember(dest => dest.PublicationYear, opt => opt.MapFrom(src => src.PublicationYear))
                .ReverseMap();

            CreateMap<Review, ReviewDto>()
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Username))
               .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title))
               .ForMember(dest => dest.ReviewText, opt => opt.MapFrom(src => src.ReviewText))
               .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))
               .ForMember(dest => dest.ISBN, opt => opt.MapFrom(src => src.BookISBN))
               .ReverseMap();
        }
    }
}
