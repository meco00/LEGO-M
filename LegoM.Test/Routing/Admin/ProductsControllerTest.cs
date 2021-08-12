namespace LegoM.Test.Routing.Admin
{
    using LegoM.Areas.Admin.Controllers;
    using MyTested.AspNetCore.Mvc;
    using Xunit;

    public class ProductsControllerTest
    {
        const string TestId = "TestId";

        [Fact]
        public void AllRouteShouldBeMapped()
            => MyRouting
                .Configuration()
            .ShouldMap("/Admin/Products/All")
            .To<ProductsController>(c => c.All());

        [Fact]
        public void DeletedRouteShouldBeMapped()
            => MyRouting
                .Configuration()
            .ShouldMap("/Admin/Products/Deleted")
            .To<ProductsController>(c => c.Deleted());

        [Fact]
        public void ChangeVisibilityShouldBeMapped()
            => MyRouting
                .Configuration()
            .ShouldMap($"/Admin/Products/ChangeVisibility/{TestId}")
            .To<ProductsController>(c => c.ChangeVisibility(TestId));

        [Fact]
        public void ReviveShouldBeMapped()
         => MyRouting
             .Configuration()
         .ShouldMap($"/Admin/Products/Revive/{TestId}")
         .To<ProductsController>(c => c.Revive(TestId));

    }
}
