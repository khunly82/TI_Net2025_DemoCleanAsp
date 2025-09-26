using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TI_Net2025_DemoCleanAsp.BLL.Services;
using TI_Net2025_DemoCleanAsp.DL.Entities;
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
            int? minPrice = filters?.MinPrice is decimal min 
                ? (int)Math.Round(min * 100m)
                : null;

            int? maxPrice = filters?.MaxPrice is decimal max
                ? (int)Math.Round(max * 100m)
                : null;

            List<ProductIndexDto> products = [.. _productService.GetPageWithCategory(page, filters?.Name, minPrice, maxPrice, filters?.CategoryId).Select(p => p.ToProductIndex())];

            int totalItems = _productService.Count(filters?.Name, minPrice, maxPrice, filters?.CategoryId);

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

            return View((pageIndex, categories, filters));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult Create()
        {
            List<CategoryDto> categories = [.. _categoryService.GetAll().Select(c => c.ToCategoryDto())];
            ViewBag.categories = categories;
            return View(new ProductFormDto());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create([FromForm] ProductFormDto form)
        {
            if (!ModelState.IsValid)
            {
                List<CategoryDto> categories = [.. _categoryService.GetAll().Select(c => c.ToCategoryDto())];
                ViewBag.categories = categories;
                return View(form);
            }

            _productService.Add(form.ToProduct());

            return RedirectToAction("Index","Product");
        }


        [Authorize(Roles = "Admin")]
        [HttpGet("product/update/{id}")]
        public IActionResult Update([FromRoute] int id)
        {
            Product? product = _productService.GetById(id);

            if(product == null)
            {
                return RedirectToAction("Index","Product");
            }

            List<CategoryDto> categories = [.. _categoryService.GetAll().Select(c => c.ToCategoryDto())];
            ViewBag.categories = categories;
            ViewBag.id = id;


            return View(product.ToProductForm());
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("product/update/{id}")]
        public IActionResult Update([FromRoute] int id, [FromForm] ProductFormDto form)
        {
            if (!ModelState.IsValid)
            {
                List<CategoryDto> categories = [.. _categoryService.GetAll().Select(c => c.ToCategoryDto())];
                ViewBag.categories = categories;
                ViewBag.id = id;
                return View(form);
            }

            _productService.Edit(id, form.ToProduct());

            return RedirectToAction("Index", "Product");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Delete([FromRoute] int id, [FromQuery] ProductFilterFormDto? filter = null)
        {
            _productService.Delete(id);
            return RedirectToAction("Index", "Product", filter);
        }
    }
}
