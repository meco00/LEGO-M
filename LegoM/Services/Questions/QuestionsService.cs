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

       public QuestionQueryModel All(
          string searchTerm = null,
         int currentPage = 1,
         int questionsPerPage = int.MaxValue,
         bool IsPublicOnly=true,
         string productId = null
         )
        {

            var questionsQuery = this.data.Questions
                 .Where(x => !IsPublicOnly || x.IsPublic)
                 .AsQueryable();


            if (!string.IsNullOrEmpty(productId))
            {

                questionsQuery = questionsQuery.Where(x => x.ProductId == productId);

            }


            if (!string.IsNullOrEmpty(searchTerm))
            {

                questionsQuery = questionsQuery
                                         .Where(x =>
                                         x.Content.ToLower().Contains(searchTerm.ToLower()));
                                       
            }


            var totalQuestions = questionsQuery.Count();

            var questions = questionsQuery
                  .Skip((currentPage - 1) * questionsPerPage)
                    .Take(questionsPerPage)
                    .OrderByDescending(x=>x.PublishedOn)
                    .ProjectTo<QuestionServiceModel>(mapper)
                    .ToList();

            return new QuestionQueryModel
            {
                Questions = questions,
                CurrentPage = currentPage,
                TotalQuestions = totalQuestions,
                QuestionsPerPage = questionsPerPage,
            };

        }

        public void Create(
            string productId,
            string userId,
            string content,
            bool IsPublic = false)
        {
            var question = new Question
            {
                Content = content,
                ProductId = productId,
                UserId = userId,
                PublishedOn = DateTime.UtcNow,
                IsPublic = IsPublic
                
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

        public QuestionServiceModel QuestionById(int id)
         => this.data.Questions.Where(x => x.Id == id)
            .ProjectTo<QuestionServiceModel>(mapper)
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
