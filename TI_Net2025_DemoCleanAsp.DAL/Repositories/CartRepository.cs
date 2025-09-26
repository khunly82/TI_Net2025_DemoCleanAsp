using Microsoft.Data.SqlClient;
using System.Data;
using TI_Net2025_DemoCleanAsp.DL.Entities;

namespace TI_Net2025_DemoCleanAsp.DAL.Repositories
{
    public class CartRepository
    {
        private readonly string _connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=DemoAspClean.Db;Trusted_Connection=True;";

        public Cart? GetByUserId(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT
                                            *
                                        FROM Cart
                                        WHERE UserId = @id";

                command.Parameters.AddWithValue("@id", id);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if(!reader.Read())
                    {
                        return null;
                    }

                    return MapCart(reader);
                }
            }
        }

        public Cart? GetWithCartLineByUserId(int id)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @"SELECT
                                            c.Id AS Id, c.UserId,
                                            ci.Id AS ItemId, ci.ProductId, ci.Quantity
                                        FROM Cart c
                                        LEFT JOIN CartItem ci ON ci.CartId = c.Id
                                        WHERE c.UserId = @id";

                command.Parameters.AddWithValue("@id", id);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Cart? cart = null;

                    while (reader.Read())
                    {
                        if (cart == null)
                        {
                            cart = MapCart(reader);
                        }

                        if (reader["ItemId"] != DBNull.Value)
                        {
                            cart.Items.Add(MapCartItem(reader));
                        }
                    }

                    return cart;
                }
            }
        }

        public Cart Add(int userId)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            using (SqlCommand command = connection.CreateCommand())
            {
                command.CommandText = @$"INSERT INTO [Cart](UserId)
                                         OUTPUT INSERTED.*
                                         VALUES(@userId)";

                command.Parameters.AddWithValue("@userId", userId);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    Cart? cart = null;

                    if (!reader.Read())
                    {
                        throw new Exception($"Error when creating cart for user : {userId}");
                    }

                    cart = MapCart(reader);

                    return cart;
                }
            }
        }

        public void UpdateBatch(Cart cart)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        DataTable tvp = new DataTable();
                        tvp.Columns.Add("ProductId", typeof(int));
                        tvp.Columns.Add("Quantity", typeof(int));

                        foreach (var item in cart.Items)
                        {
                            tvp.Rows.Add(item.ProductId, item.Quantity);
                        }

                        using (SqlCommand cmd = new SqlCommand(@"
                            MERGE CartItem AS target
                            USING (SELECT @CartId AS CartId, src.ProductId, src.Quantity
                                   FROM @CartItems AS src) AS source
                            ON (target.CartId = source.CartId AND target.ProductId = source.ProductId)
                            WHEN MATCHED THEN 
                                UPDATE SET Quantity = source.Quantity
                            WHEN NOT MATCHED THEN
                                INSERT (CartId, ProductId, Quantity)
                                VALUES (source.CartId, source.ProductId, source.Quantity);
                            ", connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@CartId", cart.Id);

                            SqlParameter tvpParam = cmd.Parameters.AddWithValue("@CartItems", tvp);
                            tvpParam.SqlDbType = SqlDbType.Structured;
                            tvpParam.TypeName = "CartItemTableType";

                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private static Cart MapCart(SqlDataReader reader)
        {
            return new Cart
            {
                Id = (int)reader["Id"],
                UserId = (int)reader["UserId"],
            };
        }

        private static CartItem MapCartItem(SqlDataReader reader)
        {
            return new CartItem
            {
                Id = (int)reader["ItemId"],
                ProductId = (int)reader["ProductId"],
                Quantity = (int)reader["Quantity"]
            };
        }
    }
}
