using TI_Net2025_DemoCleanAsp.DL.Entities;
using TI_Net2025_DemoCleanAsp.Models.Users;

namespace TI_Net2025_DemoCleanAsp.Mappers
{
    public static class UserMappers
    {
        public static User ToUser(this RegisterFormDto form)
        {
            return new User()
            {
                Email = form.Email,
                Username = form.Username,
                Password = form.Password,
            };
        }
    }
}
