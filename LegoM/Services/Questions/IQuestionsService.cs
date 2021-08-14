﻿namespace LegoM.Services.Questions
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

        QuestionByUserServiceModel QuestionByUser(string productId, string userId);

        QuestionByUserServiceModel QuestionById(int id);

        IEnumerable<QuestionListingServiceModel> Mine(string userId);

        IEnumerable<QuestionServiceModel> AllOfProduct(string productId);

        IEnumerable<QuestionServiceModel> All();

        
    }
}
