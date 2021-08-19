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

        void ChangeVisibility(int id);

        bool QuestionExists(int id);

        bool QuestionIsByUser(int id, string userId);

        bool Delete(int id);

        QuestionDetailsServiceModel Details(int id);

        QuestionServiceModel QuestionById(int id);

        IEnumerable<QuestionListingServiceModel> Mine(string userId);

        IEnumerable<QuestionServiceModel> AllOfProduct(string productId);

        QuestionQueryModel All(
             string searchTerm = null,
            int currentPage = 1,
            int questionsPerPage = int.MaxValue,
            bool IsPublicOnly = true
            );

        
    }
}
