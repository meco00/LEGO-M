namespace LegoM.Infrastructure.Profiles
{
    using AutoMapper;
    using LegoM.Data.Models;
    using LegoM.Services.Answers.Models;

    using static ProfileConstants;

    public class AnswerProfile:Profile
    {
        public AnswerProfile()
        {
            this.CreateMap<Answer, AnswerServiceModel>()
                .ForMember(x => x.UserName, cfg => cfg.MapFrom(x => x.User.FullName))
                .ForMember(x => x.PublishedOn, cfg => cfg.MapFrom(x => x.PublishedOn.ToString(DateTimeFormat)));

        }


    }
}
