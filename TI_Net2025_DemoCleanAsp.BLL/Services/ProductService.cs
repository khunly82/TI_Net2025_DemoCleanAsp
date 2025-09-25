using Azure;
using TI_Net2025_DemoCleanAsp.DAL.Repositories;
using TI_Net2025_DemoCleanAsp.DL.Entities;

namespace TI_Net2025_DemoCleanAsp.BLL.Services
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;

        public ProductService(ProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public List<Product> GetPageWithCategory(int page, string? name, int? minPrice, int? maxPrice, int? categoryId)
        {
            return _productRepository.GetAllWithCategory(page,name, minPrice, maxPrice, categoryId);
        }

        public int Count(string? name, int? minPrice, int? maxPrice, int? categoryId)
        {
            return _productRepository.Count(name, minPrice, maxPrice, categoryId);
        }
    }
}
