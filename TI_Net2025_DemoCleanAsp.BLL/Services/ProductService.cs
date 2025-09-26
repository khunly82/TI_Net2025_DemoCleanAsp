using Azure;
using TI_Net2025_DemoCleanAsp.DAL.Repositories;
using TI_Net2025_DemoCleanAsp.DL.Entities;

namespace TI_Net2025_DemoCleanAsp.BLL.Services
{
    public class ProductService
    {
        private readonly ProductRepository _productRepository;
        private readonly CategoryRepository _categoryRepository;

        public ProductService(ProductRepository productRepository, CategoryRepository categoryRepository)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
        }

        public List<Product> GetPageWithCategory(int page, string? name, int? minPrice, int? maxPrice, int? categoryId)
        {
            return _productRepository.GetPageWithCategory(page,name, minPrice, maxPrice, categoryId);
        }

        public void Add(Product p)
        {
            if (!_categoryRepository.ExistById(p.CategoryId))
            {
                throw new Exception($"Failed to create product. Category with Id : {p.CategoryId} doesn't exist");
            }
            _productRepository.Add(p);
        }

        public void Edit(int id, Product p)
        {
            Product? existing = _productRepository.GetById(id);

            if (existing == null)
            {
                throw new Exception($"Product with Id : {id} doesn't exist.");
            }

            if (!_categoryRepository.ExistById(p.CategoryId))
            {
                throw new Exception($"Failed to update product. Category with Id : {p.CategoryId} doesn't exist");
            }

            _productRepository.Update(id, p);
        }

        public void Delete(int id)
        {
            Product? existing = _productRepository.GetById(id);

            if (existing == null)
            {
                throw new Exception($"Product with Id : {id} doesn't exist.");
            }

            _productRepository.Delete(id);
        }

        public int Count(string? name, int? minPrice, int? maxPrice, int? categoryId)
        {
            return _productRepository.Count(name, minPrice, maxPrice, categoryId);
        }

        public Product? GetById(int id)
        {
            return _productRepository.GetById(id);
        }
    }
}
