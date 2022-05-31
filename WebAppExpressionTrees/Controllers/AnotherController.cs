namespace WebAppExpressionTrees.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using WebAppExpressionTrees.Infrastructure;

    public class AnotherController : Controller
    {
        // With expression trees we can have compile time checking

        public IActionResult About()
        {
            var id = 5;
            var query = "Test query";

            return this.RedirectTo<HomeController>(c => c.Index(id, query));

            //return this.RedirectToAction("Index", "Home", new {id = 5, query = "Test query"});
        }
    }
}
