namespace LegoM.Services.Questions
{
    using LegoM.Data;
    using LegoM.Data.Models;
    using LegoM.Services.Questions.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
   

    public class QuestionsService : IQuestionsService
    {
        private readonly LegoMDbContext data;

        public QuestionsService(LegoMDbContext data)
        {
            this.data = data;
        }

        public IEnumerable<QuestionServiceModel> AllOfProduct(string productId)
        => this.data.Questions.Where(x => x.ProductId == productId)
            .Select(x => new QuestionServiceModel
            {
                Id = x.Id,
                Content = x.Content,
                UserName = x.User.FullName,
                PublishedOn = x.PublishedOn.ToString("MM MMM yyy"),
                ProductPublishedOn=x.Product.PublishedOn.ToString("MM MMM yyy"),
                ProductCondition=(int)x.Product.ProductCondition
            })
            .ToList() ;

        public void Create(string productId, string userId, string content)
        {
            var question = new Question
            {
                Content = content,
                ProductId = productId,
                UserId = userId,
                PublishedOn=DateTime.UtcNow
                
            };

            this.data.Questions.Add(question);

            this.data.SaveChanges();

        }

        public bool Delete(int id)
        {
            var question = this.data.Questions.FirstOrDefault(x => x.Id == id);

            if (question==null)
            {
                return false;
            }

            this.data.Questions.Remove(question);

            this.data.SaveChanges();

            return true;
        }

        public QuestionDetailsServiceModel Details(int id)
        => this.data.Questions.Where(x=>x.Id==id).Select(x => new QuestionDetailsServiceModel
        {
            Content=x.Content,
            UserName=x.User.FullName,
            PublishedOn=x.PublishedOn.ToString("MM MMM yyy"),
            ProductId=x.ProductId,
            ProductTitle=x.Product.Title,
            ProductPrice=x.Product.Price.ToString("F2"),
            ProductCondition=(int)x.Product.ProductCondition,
            ProductPublishedOn=x.Product.PublishedOn.ToString("MM MMM yyy"),
            ProductImage= x.Product.Images.Select(x => x.ImageUrl).FirstOrDefault()


        }).FirstOrDefault();

        public IEnumerable<QuestionListingServiceModel> Mine(string userId)
        => this.data.Questions.Where(x => x.UserId == userId).Select(x => new QuestionListingServiceModel
        {
            Id = x.Id,
            Content = x.Content,
            PublishedOn = x.PublishedOn.ToString("MM MMM yyy"),
            ProductId = x.ProductId,
            ProductTitle = x.Product.Title,
            ProductCondition = (int)x.Product.ProductCondition,
            ProductPublishedOn = x.Product.PublishedOn.ToString("MM MMM yyy"),
            ProductImage = x.Product.Images.Select(x => x.ImageUrl).FirstOrDefault()

        }).ToList();

        public QuestionByUserServiceModel QuestionByUser(string productId, string userId)
        => this.data.Questions.Where(x => x.ProductId == productId && x.UserId == userId)
            .Select(x => new QuestionByUserServiceModel
            {
                Id = x.Id,
                Information = String.Concat(((int)(x.Product.ProductCondition)).ToString() + "-" + x.Product.PublishedOn.ToString("MM MMM yyy") + "-" + x.PublishedOn.ToString("MM MMM yyy"))

            }).FirstOrDefault();



        public bool QuestionIsByUser(int id, string userId)
        => this.data.Questions.Any(x => x.Id == id && x.UserId == userId);
    }
}
