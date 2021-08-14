namespace LegoM.Infrastructure.Profiles
{
    using AutoMapper;
    using LegoM.Data.Models;
    using LegoM.Services.Questions.Models;
    using System.Linq;

    using static ProfileConstants;

    public class QuestionProfile:Profile
    {
        public QuestionProfile()
        {          
            this.CreateMap<Question, QuestionListingServiceModel>()
                .ForMember(x => x.AnswersCount, cfg => cfg.MapFrom(x => x.Answers.Count(c => c.IsPublic)))
                .ForMember(x => x.ProductCondition, cfg => cfg.MapFrom(x => (int)x.Product.ProductCondition))
                .ForMember(x => x.PublishedOn, cfg => cfg.MapFrom(x => x.PublishedOn.ToString(DateTimeFormat)))
                .ForMember(x => x.ProductTitle, cfg => cfg.MapFrom(x => x.Product.Title))
                .ForMember(x => x.ProductImage, cfg => cfg.MapFrom(x => x.Product.Images.Select(x => x.ImageUrl).FirstOrDefault()));

            this.CreateMap<Question, QuestionDetailsServiceModel>()
                .ForMember(x => x.AnswersCount, cfg => cfg.MapFrom(x => x.Answers.Count))
                .ForMember(x => x.ProductCondition, cfg => cfg.MapFrom(x => (int)x.Product.ProductCondition))
                .ForMember(x => x.ProductPrice, cfg => cfg.MapFrom(x => x.Product.Price.ToString(PriceFormat)))
                .ForMember(x => x.AnswersCount, cfg => cfg.MapFrom(x => x.Answers.Count))
                .ForMember(x => x.UserName, cfg => cfg.MapFrom(x => x.User.FullName))
                .ForMember(x => x.PublishedOn, cfg => cfg.MapFrom(x => x.PublishedOn.ToString(DateTimeFormat)))
                .ForMember(x => x.ProductTitle, cfg => cfg.MapFrom(x => x.Product.Title))
                .ForMember(x => x.ProductImage, cfg => cfg.MapFrom(x => x.Product.Images.Select(x => x.ImageUrl).FirstOrDefault()));


            this.CreateMap<Question, QuestionServiceModel>()
                .ForMember(x => x.AnswersCount, cfg => cfg.MapFrom(x => x.Answers.Count(c=>c.IsPublic)))
                .ForMember(x => x.ProductCondition, cfg => cfg.MapFrom(x => (int)x.Product.ProductCondition))
                .ForMember(x => x.UserName, cfg => cfg.MapFrom(x => x.User.FullName))
                .ForMember(x => x.PublishedOn, cfg => cfg.MapFrom(x => x.PublishedOn.ToString(DateTimeFormat)));

        }
    }
}
