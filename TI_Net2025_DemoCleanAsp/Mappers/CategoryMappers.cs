using TI_Net2025_DemoCleanAsp.DL.Entities;
using TI_Net2025_DemoCleanAsp.Models.Category;

namespace TI_Net2025_DemoCleanAsp.Mappers
{
    public static class CategoryMappers
    {
        public static CategoryDto ToCategoryDto(this Category c)
        {
            return new CategoryDto()
            {
                Id = c.Id,
                Name = c.Name,
            };
        }
    }
}
