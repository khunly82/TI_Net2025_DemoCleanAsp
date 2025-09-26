using TI_Net2025_DemoCleanAsp.DL.Entities;
using TI_Net2025_DemoCleanAsp.Models.Product;

namespace TI_Net2025_DemoCleanAsp.Mappers
{
    public static class ProductMappers
    {
        public static ProductIndexDto ToProductIndex(this Product p)
        {
            return new ProductIndexDto()
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Category = p.Category!.Name
            };
        }

        public static ProductFormDto ToProductForm(this Product p)
        {
            return new ProductFormDto()
            {
                Name = p.Name,
                Description = p.Description,
                Price = Math.Round(p.Price / 100m, 2),
                CategoryId = p.CategoryId,
            };
        }

        public static Product ToProduct(this ProductFormDto p)
        {
            return new Product()
            {
                Name = p.Name,
                Description = p.Description,
                Price = (int) Math.Round(p.Price * 100m),
                CategoryId = p.CategoryId,
            };
        }
    }
}
