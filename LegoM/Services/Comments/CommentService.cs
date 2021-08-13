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
         => this.data.Comments.Where(x => x.ReviewId == reviewId && x.IsPublic)
            .OrderBy(x => x.PublishedOn)
            .ProjectTo<CommentServiceModel>(mapper)
            .ToList();

        public IEnumerable<CommentServiceModel> All()
        => this.data.Comments
           .OrderByDescending(x => x.PublishedOn)
           .ProjectTo<CommentServiceModel>(mapper)
           .ToList();

        public void Create(int reviewId, string userId, string content)
        {
            var comment = new Comment
            {
                ReviewId = reviewId,
                UserId = userId,
                Content = content,
                PublishedOn=DateTime.UtcNow,
                IsPublic=false
            };

            this.data.Comments.Add(comment);

            this.data.SaveChanges();
        }

       public void ChangeVisibility(int id)
        {
            var comment = this.data.Comments.Find(id);

            if (comment == null)
            {
                return;
            }

            comment.IsPublic = !comment.IsPublic;

            this.data.SaveChanges();
        }

        public bool Delete(int id)
        {
            var comment = this.data.Comments.Find(id);

            if (comment == null)
            {
                return false;
            }

            this.data.Comments.Remove(comment);

            this.data.SaveChanges();

            return true;
        }
    }
}
