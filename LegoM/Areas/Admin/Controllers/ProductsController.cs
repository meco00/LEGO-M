namespace LegoM.Areas.Admin.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

   
    public class ProductsController:AdminController
    {
        public IActionResult Index() => View();
    }
}
