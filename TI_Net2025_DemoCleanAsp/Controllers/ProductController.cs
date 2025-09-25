using Microsoft.AspNetCore.Mvc;
using TI_Net2025_DemoCleanAsp.BLL.Services;
using TI_Net2025_DemoCleanAsp.Mappers;
using TI_Net2025_DemoCleanAsp.Models;
using TI_Net2025_DemoCleanAsp.Models.Category;
using TI_Net2025_DemoCleanAsp.Models.Product;

namespace TI_Net2025_DemoCleanAsp.Controllers
{
    public class ProductController : Controller
    {

        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;

        public ProductController(ProductService productService, CategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult Index([FromQuery] int page = 0, [FromQuery] ProductFilterFormDto? filters = null)
        {
            List<ProductIndexDto> products = [.. _productService.GetPageWithCategory(page,filters?.Name,filters?.MinPrice,filters?.MaxPrice,filters?.CategoryId).Select(p => p.ToProductIndex())];

            int totalItems = _productService.Count(filters?.Name, filters?.MinPrice, filters?.MaxPrice, filters?.CategoryId);

            PageIndex<ProductIndexDto> pageIndex = new PageIndex<ProductIndexDto>()
            {
                Items = products,
                Meta = new PageIndexMeta()
                {
                    ItemsCount = products.Count,
                    Page = page,
                    TotalItems = totalItems,
                    TotalPages = (int)Math.Ceiling(totalItems / 5f) - 1,
                }
            };

            List<CategoryDto> categories = [.. _categoryService.GetAll().Select(c => c.ToCategoryDto())];

            return View((pageIndex,categories,filters));
        }
    }
}
