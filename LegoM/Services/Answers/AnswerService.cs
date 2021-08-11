namespace LegoM.Services.Answers
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using LegoM.Data;
    using LegoM.Data.Models;
    using LegoM.Services.Answers.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AnswerService : IAnswerService
    {
        private readonly LegoMDbContext data;

        private readonly IConfigurationProvider mapper;

        public AnswerService(LegoMDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }

        public void Create(int questionId, string userId, string content)
        {

            var answer = new Answer
            {
                QuestionId = questionId,
                UserId = userId,
                Content = content,
                PublishedOn=DateTime.UtcNow,
                IsPublic=false
            };

            this.data.Answers.Add(answer);

            this.data.SaveChanges();

        }

        public IEnumerable<AnswerServiceModel> AnswersOfQuestion(int questionId)
        => this.data.Answers.Where(x => x.QuestionId == questionId&&x.IsPublic)
            .OrderBy(x => x.PublishedOn)
            .ProjectTo<AnswerServiceModel>(mapper)
            .ToList();

       public void ChangeVisibility(int id)
        {
            var answer = this.data.Answers.Find(id);

            if (answer == null)
            {
                return;
            }

            answer.IsPublic = !answer.IsPublic;

            this.data.SaveChanges();
        }
    }
}
