namespace LegoM.Services.Answers.Models
{
    public class AnswerServiceModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string UserName { get; set; }

        public string QuestionId { get; set; }

        public string PublishedOn { get; set; }

        public bool IsPublic { get; set; }
    }
}
