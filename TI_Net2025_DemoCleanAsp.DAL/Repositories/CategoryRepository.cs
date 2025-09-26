using Microsoft.Data.SqlClient;
using TI_Net2025_DemoCleanAsp.DL.Entities;

namespace TI_Net2025_DemoCleanAsp.DAL.Repositories
{
    public class CategoryRepository
    {
        private readonly string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=DemoAspClean.Db;Trusted_Connection=True;";

        public List<Category> GetAll()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT * 
                                        FROM [Category]";

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    List<Category> categories = [];

                    while (reader.Read())
                    {
                        categories.Add(MapCategory(reader));
                    }

                    return categories;
                }
            }
        }

        public bool ExistById(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @$"SELECT 
                                         cast(CASE WHEN EXISTS (
                                            SELECT 1 FROM Product WHERE Id = @id) 
                                             THEN 1 
                                             ELSE 0 
                                         END as bit) AS isExisting;";

                command.Parameters.AddWithValue("@id", id);

                connection.Open();

                return (bool)command.ExecuteScalar();
            }
        }

        private Category MapCategory(SqlDataReader reader)
        {
            return new Category()
            {
                Id = (int) reader["Id"],
                Name = (string) reader["Name"],
            };
        }
    }
}
