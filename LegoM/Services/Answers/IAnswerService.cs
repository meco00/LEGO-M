using LegoM.Services.Answers.Models;
using System.Collections.Generic;

namespace LegoM.Services.Answers
{
    public interface IAnswerService
    {
        void Create(
            int questionId,
            string userId,
            string content
            );

        IEnumerable<AnswerServiceModel> AnswersOfQuestion(int questionId);

        IEnumerable<AnswerServiceModel> All();

        void ChangeVisibility(int id);

        bool Delete(int id);
    }
}
