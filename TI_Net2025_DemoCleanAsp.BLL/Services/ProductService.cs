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

        public List<Product> GetAllWithCategory()
        {
            return _productRepository.GetAllWithCategory();
        }
    }
}
