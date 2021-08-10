namespace LegoM.Infrastructure.Profiles
{
    using AutoMapper;
    using LegoM.Data.Models;
    using LegoM.Services.Comments.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using static ProfileConstants;

    public class CommentProfile:Profile
    {
        public CommentProfile()
        {
            this.CreateMap<Comment, CommentServiceModel>()
                .ForMember(x => x.UserName, cfg => cfg.MapFrom(x => x.User.FullName))
                .ForMember(x => x.PublishedOn, cfg => cfg.MapFrom(x => x.PublishedOn.ToString(DateTimeFormat)));

        }

    }
}
