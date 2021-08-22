namespace LegoM.Infrastructure.Profiles
{
    using AutoMapper;
    using LegoM.Data.Models;
    using LegoM.Services.Reports.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using static ProfileConstants;

    public class ReportProfile:Profile
    {
        public ReportProfile()
        {
            this.CreateMap<Report, ReportServiceModel>()
              .ForMember(x => x.ReportType, cfg => cfg.MapFrom(x => x.ReportType.ToString()))
              .ForMember(x => x.UserName, cfg => cfg.MapFrom(x => x.User.FullName))
              .ForMember(x => x.PublishedOn, cfg => cfg.MapFrom(x => x.PublishedOn.ToString(DateTimeFormat)));

        }
    }
}
