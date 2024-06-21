using AutoMapper;
using Forum.Entities;
using Forum.Models;
using Forum.Models.Identity;

namespace Forum.Service
{
    public class MappingInitializer
    {
        public MappingInitializer() { }
        public IMapper Initialize()
        {
            MapperConfiguration configuration = new(config =>
            {
                config.CreateMap<Topic, TopicCreatingDto>().ReverseMap();
                config.CreateMap<Topic, TopicUpdatingDto>().ReverseMap();
                config.CreateMap<Topic, TopicGettingDto>()
                .ForMember(destination => destination.CommentCount, options => options.MapFrom(source => source.Comments != null ? source.Comments.Count() : 0))
                .ReverseMap();

                config.CreateMap<Comment, CommentCreatingDto>().ReverseMap();
                config.CreateMap<Comment, CommentUpdatingDto>().ReverseMap();
                config.CreateMap<Comment, CommentsInTopicDto>()
                .ForMember(destination => destination.Name, options => options.MapFrom(source => source.User.Name))
                .ForMember(destination => destination.CreatedTime, options => options.MapFrom(source => source.CreatedTime))
                .ForMember(destination => destination.Context, options => options.MapFrom(source => source.Content))
                .ReverseMap();

                config.CreateMap<Comment, CommentGettingDto>()
                .ForMember(destination => destination.Content, options => options.MapFrom(source => source.Content))
                .ForMember(destination => destination.CreatedTime, options => options.MapFrom(source => source.CreatedTime))
                .ForMember(destination => destination.TopicTitle, options => options.MapFrom(source => source.Topic.Title))
                .ForMember(destination => destination.UserName, options => options.MapFrom(source => source.User.Name))
                .ReverseMap();

                config.CreateMap<Topic, StateUpdatingDto>()
                .ForMember(destination => destination.State, options => options.MapFrom(source => source.State)).ReverseMap();

                config.CreateMap<Users, UserDto>().ReverseMap();
                config.CreateMap<Users, UserGettingDto>().ReverseMap();
                config.CreateMap<Users, LoginRequestDto>().ReverseMap();
                config.CreateMap<Users, RegistrationRequestDto>().ReverseMap();

                config.CreateMap<RegistrationRequestDto, Users>()
                .ForMember(destination => destination.UserName, options => options.MapFrom(source => source.Email))
                .ForMember(destination => destination.NormalizedUserName, options => options.MapFrom(source => source.Email.ToUpper()))
                .ForMember(destination => destination.Email, options => options.MapFrom(source => source.Email))
                .ForMember(destination => destination.NormalizedEmail, options => options.MapFrom(source => source.Email.ToUpper()))
                .ForMember(destination => destination.PhoneNumber, options => options.MapFrom(source => source.PhoneNumber));

            });

            return configuration.CreateMapper();
        }
    }
}
