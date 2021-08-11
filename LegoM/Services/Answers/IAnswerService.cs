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

        void ChangeVisibility(int id);
    }
}
