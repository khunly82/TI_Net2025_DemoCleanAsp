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
    }
}
