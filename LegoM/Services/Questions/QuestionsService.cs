namespace LegoM.Services.Questions
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
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
        private readonly IConfigurationProvider mapper;

        public QuestionsService(LegoMDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }

        public IEnumerable<QuestionServiceModel> AllOfProduct(string productId)
        => this.data.Questions
            .Where(x => x.ProductId == productId&&x.IsPublic)
            .ProjectTo<QuestionServiceModel>(mapper)
            .ToList() ;

        public void Create(string productId, string userId, string content)
        {
            var question = new Question
            {
                Content = content,
                ProductId = productId,
                UserId = userId,
                PublishedOn = DateTime.UtcNow,
                IsPublic = false
                
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
        => this.data.Questions
            .Where(x=>x.Id==id)
            .ProjectTo<QuestionDetailsServiceModel>(mapper)
            .FirstOrDefault();

        public IEnumerable<QuestionListingServiceModel> Mine(string userId)
        => this.data.Questions
            .Where(x => x.UserId == userId)
            .ProjectTo<QuestionListingServiceModel>(mapper)
            .ToList();

        public QuestionByUserServiceModel QuestionById(int id)
         => this.data.Questions.Where(x => x.Id == id)
            .ProjectTo<QuestionByUserServiceModel>(mapper)
            .FirstOrDefault();

        public QuestionByUserServiceModel QuestionByUser(string productId, string userId)
        => this.data.Questions
            .Where(x => x.ProductId == productId && x.UserId == userId)
            .ProjectTo<QuestionByUserServiceModel>(mapper)
            .FirstOrDefault();

        public bool QuestionExists(int id)
        => this.data.Questions.Any(x => x.Id == id);

        public bool QuestionIsByUser(int id, string userId)
        => this.data.Questions.Any(x => x.Id == id && x.UserId == userId);

        public void ChangeVisibility(int id)
        {
            var question = this.data.Questions.Find(id);

            if (question == null)
            {
                return;
            }

            question.IsPublic = !question.IsPublic;

            this.data.SaveChanges();
        }
    }
}
