namespace LegoM.Services.Questions
{
  using LegoM.Services.Questions.Models;
    using System.Collections;
    using System.Collections.Generic;

    public interface IQuestionsService
    {
        void Create(
            string productId,
            string userId,
            string content
            );

        QuestionDetailsServiceModel Details(int id);

        QuestionByUserServiceModel QuestionByUser(string productId, string userId);

        IEnumerable<QuestionServiceModel> AllOfProduct(string productId);
    }
}
