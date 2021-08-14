namespace LegoM.Test.Pipeline.Admin
{
    using FluentAssertions;
    using LegoM.Areas.Admin;
    using LegoM.Data.Models;
    using LegoM.Services.Comments.Models;
    using MyTested.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    using static Data.Comments;

    using CommentsController = LegoM.Areas.Admin.Controllers.CommentsController;


    public class CommentsPipelineTest
    {
        [Fact]
        public void AllShouldReturnCorrectViewWithModel()
         => MyPipeline
              .Configuration()
              .ShouldMap(request => request.WithPath("/Admin/Comments/All")
              .WithUser(new[] { AdminConstants.AdministratorRoleName })
              .WithAntiForgeryToken())
              .To<CommentsController>(c => c.All())
               .Which((System.Action<MyTested.AspNetCore.Mvc.Builders.Contracts.Pipeline.IWhichControllerInstanceBuilder<CommentsController>>)(controller => controller
                  .WithData(Data.Comments.GetComments())))
                .ShouldReturn()
                .View(view => view
                .WithModelOfType<List<CommentServiceModel>>()
                .Passing(model => model.Should().NotBeEmpty()));

        [Fact]
        public void ChangeVisibilityShouldChangeAnswerAndRedirectToAll()
          => MyPipeline
                .Configuration()
                 .ShouldMap(request => request
                  .WithPath($"/Admin/Comments/ChangeVisibility/{1}")
                   .WithUser(new[] { AdminConstants.AdministratorRoleName })
                   .WithAntiForgeryToken())
                 .To<CommentsController>(c => c.ChangeVisibility(1))
                 .Which(controller => controller
                  .WithData(GetCommentsBeta(1)))
                 .ShouldHave()
                  .Data(data => data
                       .WithSet<Comment>(set => set
                            .Any(x => x.Id == 1 && !x.IsPublic)))
                   .AndAlso()
                   .ShouldReturn()
                   .Redirect(redirect => redirect
                      .To<CommentsController>(c => c.All()));


        [Fact]
        public void DeleteShouldDeleteCommentAndRedirectToAll()
          => MyPipeline
             .Configuration()
              .ShouldMap(request => request
                  .WithPath($"/Admin/Comments/Delete/{1}")
                   .WithUser(new[] { AdminConstants.AdministratorRoleName })
                   .WithAntiForgeryToken())
                     .To<CommentsController>(c => c.Delete(1))
                   .Which(controller => controller
                         .WithData(GetCommentsBeta(1)))
                  .ShouldHave()
                  .TempData(tempData => tempData
                               .ContainingEntryWithKey(WebConstants.GlobalMessageKey))
                  .Data(data => data.WithSet<Comment>(set =>
                  {
                      set.FirstOrDefault(x=>x.Id==1).Should().BeNull();
                      set.Should().BeEmpty();

                  }))
                   .AndAlso()
                   .ShouldReturn()
                   .Redirect(redirect => redirect
                      .To<CommentsController>(c => c.All()));


    }
}
