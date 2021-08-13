﻿namespace LegoM.Test.Pipeline.Admin
{
    using FluentAssertions;
    using LegoM.Areas.Admin;
    using LegoM.Areas.Admin.Controllers;
    using LegoM.Data.Models;
    using LegoM.Services.Comments.Models;
    using MyTested.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    using static Data.Comments;

    public class CommentsControllerTest
    {
        [Fact]
        public void AllShouldReturnCorrectViewWithModel()
         => MyPipeline
              .Configuration()
              .ShouldMap(request => request.WithPath("/Admin/Comments/All")
              .WithUser(new[] { AdminConstants.AdministratorRoleName })
              .WithAntiForgeryToken())
              .To<CommentsController>(c => c.All())
               .Which(controller => controller
                  .WithData(TenComments()))
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
                  .WithData(GetComment()))
                 .ShouldHave()
                  .Data(data => data
                       .WithSet<Comment>(set => set
                            .Any(x => x.Id == 1 && !x.IsPublic)))
                   .AndAlso()
                   .ShouldReturn()
                   .Redirect(redirect => redirect
                      .To<CommentsController>(c => c.All()));

    }
}
