using Microsoft.Data.SqlClient;
using TI_Net2025_DemoCleanAsp.DL.Entities;

namespace TI_Net2025_DemoCleanAsp.DAL.Repositories
{
    public class ProductRepository
    {

        private readonly string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=DemoAspClean.Db;Trusted_Connection=True;";

        public List<Product> GetAll()
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT * 
                                        FROM [Product]";

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    List<Product> products = [];

                    while (reader.Read())
                    {
                        products.Add(MapProduct(reader));
                    }

                    return products;
                }
            }
        }

        public List<Product> GetPageWithCategory(int page, string? name, int? minPrice, int? maxPrice, int? categoryId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT 
                                            p.Id as Id,
                                            p.Name as Name,
                                            p.Description as Description,
                                            p.Price as Price,
                                            p.CategoryId as CategoryId,
                                            c.Name as CategoryName
                                        FROM [Product] p
                                        JOIN [Category] c 
                                            ON p.CategoryId = c.Id 
                                        WHERE   (p.Name LIKE @name OR @name is null) 
                                            AND (p.Price >= @minPrice OR @minPrice is null)
                                            AND (p.Price <= @maxPrice OR @maxPrice is null)
                                            AND (p.CategoryId = @categoryId OR @categoryId is null)
                                        ORDER BY p.Name
                                        OFFSET @offset ROWS
                                        FETCH NEXT 5 ROWS ONLY";

                command.Parameters.AddWithValue("@name", name is null ? DBNull.Value : $"%{name}%");
                command.Parameters.AddWithValue("@minPrice", minPrice is null ? DBNull.Value : minPrice);
                command.Parameters.AddWithValue("@maxPrice", maxPrice is null ? DBNull.Value : maxPrice);
                command.Parameters.AddWithValue("@categoryId", categoryId is null ? DBNull.Value : categoryId);
                command.Parameters.AddWithValue("@offset", page * 5);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    List<Product> products = [];

                    while (reader.Read())
                    {
                        products.Add(MapProductWithCategory(reader));
                    }

                    return products;
                }
            }
        }

        public Product? GetById(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT * 
                                        FROM [Product] 
                                        WHERE Id = @id";

                command.Parameters.AddWithValue("@id", id);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    if (!reader.Read())
                    {
                        return null;
                    }

                    return MapProduct(reader);
                }
            }
        }

        public Product? GetByIdWithCategory(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT 
                                            p.Id as Id,
                                            p.Name as Name,
                                            p.Description as Description,
                                            p.Price as Price,
                                            p.CategoryId as CategoryId,
                                            c.Name as CategoryName
                                        FROM [Product] p
                                        JOIN [Category] c 
                                            ON p.CategoryId = c.Id
                                        WHERE Id = @id";

                command.Parameters.AddWithValue("@id", id);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    if (!reader.Read())
                    {
                        return null;
                    }

                    return MapProductWithCategory(reader);
                }
            }
        }

        public void Add(Product product)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @$"INSERT INTO [Product](Name,Description,Price,CategoryId)
                                        VALUES(@name,@description,@price,@categoryId)";

                command.Parameters.AddWithValue("@name", product.Name);
                command.Parameters.AddWithValue("@description", product.Description is null ? DBNull.Value : product.Description);
                command.Parameters.AddWithValue("@price", product.Price);
                command.Parameters.AddWithValue("@categoryId", product.CategoryId);

                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void Update(int id, Product product)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @$"UPDATE [Product] 
                                         SET Name = @name,
                                             Description = @description,
                                             Price = @price,
                                             CategoryId = @categoryId 
                                         WHERE Id = @id";

                command.Parameters.AddWithValue("@name", product.Name);
                command.Parameters.AddWithValue("@description", product.Description is null ? DBNull.Value : product.Description);
                command.Parameters.AddWithValue("@price", product.Price);
                command.Parameters.AddWithValue("@categoryId", product.CategoryId);
                command.Parameters.AddWithValue("@id", id);

                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void Delete(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @$"DELETE [Product] 
                                         WHERE Id = @id";

                command.Parameters.AddWithValue("@id", id);

                connection.Open();

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public int Count(string? name, int? minPrice, int? maxPrice, int? categoryId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT COUNT(*) 
                                        FROM [Product] p
                                            WHERE   (p.Name LIKE @name OR @name is null) 
                                                AND (p.Price >= @minPrice OR @minPrice is null)
                                                AND (p.Price <= @maxPrice OR @maxPrice is null)
                                                AND (p.CategoryId = @categoryId OR @categoryId is null)";

                command.Parameters.AddWithValue("@name", name is null ? DBNull.Value : $"%{name}%");
                command.Parameters.AddWithValue("@minPrice", minPrice is null ? DBNull.Value : minPrice);
                command.Parameters.AddWithValue("@maxPrice", maxPrice is null ? DBNull.Value : maxPrice);
                command.Parameters.AddWithValue("@categoryId", categoryId is null ? DBNull.Value : categoryId);

                connection.Open();

                return (int) command.ExecuteScalar();
            }
        }

        private Product MapProduct(SqlDataReader reader)
        {
            return new Product()
            {
                Id = (int) reader["Id"],
                Name = (string) reader["Name"],
                Description = reader["Description"] == DBNull.Value ? null : (string) reader["Description"],
                Price = (int)reader["Price"],
                CategoryId = (int)reader["CategoryId"],
            };
        }

        private Product MapProductWithCategory(SqlDataReader reader)
        {
            Product product = MapProduct(reader);
            product.Category = new Category()
            {
                Id = (int) reader["CategoryId"],
                Name = (string) reader["CategoryName"],
            };

            return product;
        }
    }
}
