using AutoMapper;
using Kuchenne_rewolucje.Dtos;
using Kuchenne_rewolucje.Models;

namespace Kuchenne_rewolucje.Helpers
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Article, ArticleDto>()
                .ForMember(
                    dest => dest.Rating,
                    opt => opt.MapFrom(src => CalculateAverageRating(src.Ratings)));
            CreateMap<ArticleDto, Article>()
                .ForMember(
                    dest => dest.Ratings,
                    opt => opt.Ignore());
            CreateMap<Comment, CommentDto>().ReverseMap();
        }

        private double? CalculateAverageRating(List<Rating>? ratings)
        {
            if (ratings == null || ratings.Count == 0)
            {
                return 0;
            } else if(ratings.Count == 1)
            {
                return ratings[0].Value;
            }

            double totalRating = 0;
            foreach (var rating in ratings)
            {
                totalRating += rating.Value;
            }
            return totalRating / ratings.Count;
        }
    }
}
