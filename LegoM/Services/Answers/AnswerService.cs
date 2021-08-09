namespace LegoM.Services.Answers
{
    using LegoM.Data;
    using LegoM.Data.Models;
    using LegoM.Services.Answers.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AnswerService : IAnswerService
    {
        private LegoMDbContext data { get; set; }

        public AnswerService(LegoMDbContext data)
        {
            this.data = data;
        }

        public void Create(int questionId, string userId, string content)
        {

            var answer = new Answer
            {
                QuestionId = questionId,
                UserId = userId,
                Content = content,
                PublishedOn=DateTime.UtcNow
            };

            this.data.Answers.Add(answer);

            this.data.SaveChanges();

        }

        public IEnumerable<AnswerServiceModel> AnswersOfQuestion(int questionId)
        => this.data.Answers.Where(x => x.QuestionId == questionId).OrderByDescending(x => x.PublishedOn)
            .Select(x => new AnswerServiceModel
            {
                Content = x.Content,
                UserName = x.User.FullName,
                PublishedOn = x.PublishedOn.ToString("MM MMM yyy")
            })
            .ToList();
    }
}
