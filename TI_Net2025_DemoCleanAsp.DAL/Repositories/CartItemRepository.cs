using Microsoft.Data.SqlClient;
using System.Data;
using TI_Net2025_DemoCleanAsp.DL.Entities;

namespace TI_Net2025_DemoCleanAsp.DAL.Repositories
{
    public class CartItemRepository
    {
        private readonly string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=DemoAspClean.Db;Trusted_Connection=True;";

        public void AddAll(List<CartItem> items)
        {
            DataTable table = new DataTable();
            table.Columns.Add("CartId", typeof(int));
            table.Columns.Add("ProductId", typeof(int));
            table.Columns.Add("Quantity", typeof(int));

            foreach (var item in items)
            {
                table.Rows.Add(item.CartId, item.ProductId, item.Quantity);
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
                    bulkCopy.DestinationTableName = "CartItem";
                    bulkCopy.ColumnMappings.Add("CartId", "CartId");
                    bulkCopy.ColumnMappings.Add("ProductId", "ProductId");
                    bulkCopy.ColumnMappings.Add("Quantity", "Quantity");

                    bulkCopy.WriteToServer(table);
                }
            }
        }

        public void AddItem(int cartId, int productId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"MERGE CartItem AS target
                                        USING (SELECT @CartId AS CartId, @ProductId AS ProductId) AS source
                                        ON (target.CartId = source.CartId AND target.ProductId = source.ProductId)
                                        WHEN MATCHED THEN 
                                            UPDATE SET Quantity = Quantity + 1
                                        WHEN NOT MATCHED THEN
                                            INSERT (CartId, ProductId, Quantity)
                                            VALUES (source.CartId, source.ProductId, 1);";

                    cmd.Parameters.AddWithValue("@CartId", cartId);
                    cmd.Parameters.AddWithValue("@ProductId", productId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id, int userId)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = connection.CreateCommand();
            command.CommandText = @"
                DELETE FROM CartItem
                WHERE ProductId = @id AND CartId = (
                    SELECT Id FROM Cart WHERE UserId = @userId
                )
            ";
            command.Parameters.AddWithValue("id", id);
            command.Parameters.AddWithValue("userId", userId);
            connection.Open();
            command.ExecuteNonQuery();
        }
    }
}
