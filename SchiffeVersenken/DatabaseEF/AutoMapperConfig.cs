using AutoMapper;
using SchiffeVersenken.DatabaseEF.Models;
using SchiffeVersenken.DatabaseEF.Database;

public class AutoMapperConfig
{
    public static Mapper InitializeAutomapper()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<UserEF, User>()
            .ReverseMap();

            cfg.CreateMap<HighScoreEF, Highscore>()
            .ForMember(dest => dest.User_Id, act => act.MapFrom(src => src.UserId))
            .ReverseMap();

            cfg.CreateMap<HighScoreEF, UserScoreView>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.Name))
            .ForMember(dest => dest.Score, opt => opt.MapFrom(src => src.Score))
            .ForMember(dest => dest.Opponent, opt => opt.MapFrom(src => src.Opponent))
            .ForMember(dest => dest.Won, opt => opt.MapFrom(src => src.Won));

        });

        //Create an Instance of Mapper and return that Instance
        var mapper = new Mapper(config);
        return mapper;
    }
}
