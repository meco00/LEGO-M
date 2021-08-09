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
                
            };

            this.data.Questions.Add(question);

            this.data.SaveChanges();

        }

        public QuestionDetailsServiceModel Details(int id)
        {
            throw new NotImplementedException();
        }

        public QuestionByUserServiceModel QuestionByUser(string productId, string userId)
        => this.data.Questions.Where(x => x.ProductId == productId && x.UserId == userId)
            .Select(x => new QuestionByUserServiceModel
            {
                Id = x.Id,
                Information = String.Concat(((int)(x.Product.ProductCondition)).ToString() + "-" + x.Product.PublishedOn.ToString("MM MMM yyy") + "-" + x.PublishedOn.ToString("MM MMM yyy"))

            }).FirstOrDefault();
    }
}
