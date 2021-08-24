namespace LegoM.Services.Comments.Models
{
    public class CommentServiceModel
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public string UserName { get; set; }

        public bool IsPublic  { get; set; }

        public string PublishedOn { get; set; }
    }
}
