namespace LegoM.Services.Questions.Models
{
    public interface IQuestionModel
    {
         int ProductCondition { get; }

         string Content { get; }

         string PublishedOn { get; }
    }
}
