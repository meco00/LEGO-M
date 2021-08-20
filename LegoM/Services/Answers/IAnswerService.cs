using LegoM.Services.Answers.Models;
using System.Collections.Generic;

namespace LegoM.Services.Answers
{
    public interface IAnswerService
    {
        void Create(
            int questionId,
            string userId,
            string content,
            bool IsPublic = false
            );

        IEnumerable<AnswerServiceModel> AnswersOfQuestion(int questionId);

        AnswerQueryModel All(
           string searchTerm = null,
           int currentPage = 1,
           int answersPerPage = int.MaxValue,
           bool IsPublicOnly = true);

        void ChangeVisibility(int id);

        bool Delete(int id);
    }
}
