namespace LegoM.Services.Comments
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using LegoM.Data;
    using LegoM.Data.Models;
    using LegoM.Services.Answers.Models;
    using LegoM.Services.Comments.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class CommentService : ICommentService
    {
        private readonly LegoMDbContext data;

        private readonly IConfigurationProvider mapper;

        public CommentService(LegoMDbContext data, IMapper mapper)
        {
            this.data = data;
            this.mapper = mapper.ConfigurationProvider;
        }

        public IEnumerable<CommentServiceModel> CommentsOfReview(int reviewId)
         => this.data.Comments.Where(x => x.ReviewId == reviewId)
            .OrderBy(x => x.PublishedOn)
            .ProjectTo<CommentServiceModel>(mapper)
            .ToList();

        public void Create(int reviewId, string userId, string content)
        {
            var comment = new Comment
            {
                ReviewId = reviewId,
                UserId = userId,
                Content = content,
                PublishedOn=DateTime.UtcNow
            };

            this.data.Comments.Add(comment);

            this.data.SaveChanges();
        }
    }
}
