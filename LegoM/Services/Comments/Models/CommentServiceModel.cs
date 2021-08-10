namespace LegoM.Services.Comments.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CommentServiceModel
    {
        public string Content { get; set; }

        public string UserName { get; set; }

        public string PublishedOn { get; set; }
    }
}
