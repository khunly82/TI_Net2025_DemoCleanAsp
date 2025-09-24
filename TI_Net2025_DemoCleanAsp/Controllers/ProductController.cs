using Microsoft.AspNetCore.Mvc;
using TI_Net2025_DemoCleanAsp.BLL.Services;
using TI_Net2025_DemoCleanAsp.Mappers;
using TI_Net2025_DemoCleanAsp.Models.Product;

namespace TI_Net2025_DemoCleanAsp.Controllers
{
    public class ProductController : Controller
    {

        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            List<ProductIndexDto> products = [.. _productService.GetAllWithCategory().Select(p => p.ToProductIndex())];

            return View(products);
        }
    }
}
