using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TestContainerWebApi.Models;

namespace TestContainerWebApi.db
{
    public class AdoDbContext : DbContext
    {
        private readonly string _con_str;

        public AdoDbContext(DbContextOptions<AdoDbContext> options) : base(options)
        {
            _con_str = Database.GetConnectionString();
        }

        #region User

        public async Task<List<User>> GetAllUsers()
        {
            List<User> users = new List<User>();
            string sql_statements = """
                SELECT * FROM users
                """;

            using (SqlConnection connection = new SqlConnection(_con_str))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(sql_statements, connection);
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (!reader.HasRows)
                    {
                        return null;
                    }

                    while (await reader.ReadAsync())
                    {
                        users.Add(new User(
                            id: Convert.ToInt32(reader["id"]),
                            created_at: (DateTimeOffset)reader["created_at"],
                            updated_at: (DateTimeOffset)reader["updated_at"]
                            ));
                    }
                }
            }
            return users;
        }

        public async Task<User> GetUser(int id)
        {
            string sql_statements = """
                SELECT * FROM users
                WHERE id = @id
                """;

            using (SqlConnection connection = new SqlConnection(_con_str))
            {
                User result = null;

                SqlCommand command = new SqlCommand(sql_statements, connection);
                command.Parameters.AddWithValue("@id", id);

                await connection.OpenAsync();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (!reader.HasRows)
                    {
                        return null;
                    }

                    while (await reader.ReadAsync())
                    {
                        result = new User(
                            id: Convert.ToInt32(reader["id"]),
                            created_at: (DateTimeOffset)reader["created_at"],
                            updated_at: (DateTimeOffset)reader["updated_at"]
                            );
                    }
                }
                return result;
            }
        }

        public async Task<int> CreateUser()
        {
            int result;
            DateTimeOffset created_at = new DateTimeOffset(DateTime.Now);
            DateTimeOffset updated_at = new DateTimeOffset(DateTime.Now);
            string sql_statements = """
                INSERT INTO users (created_at, updated_at)
                VALUES (@created_at, @updated_at);
                SET @id=SCOPE_IDENTITY();
                """;

            using (SqlConnection connection = new SqlConnection(_con_str))
            {
                using (SqlCommand cmd = new SqlCommand(sql_statements, connection))
                {
                    cmd.Parameters.Add("@created_at", SqlDbType.DateTimeOffset).Value = created_at;
                    cmd.Parameters.Add("@updated_at", SqlDbType.DateTimeOffset).Value = updated_at;
                    SqlParameter id_returning = new SqlParameter
                    {
                        ParameterName = "@id",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(id_returning);
                    
                    await connection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    result = Convert.ToInt32(id_returning.Value);
                }
            }
            return result;
        }

        public async Task<User> UpdateUser(int id)
        {
            User result = null;
            DateTimeOffset updated_at = new DateTimeOffset(DateTime.Now);
            string sql_statements = """
                UPDATE users
                SET updated_at = @updated_at
                OUTPUT INSERTED.*
                WHERE id = @id
                """;

            using (SqlConnection connection = new SqlConnection(_con_str))
            {
                using (SqlCommand cmd = new SqlCommand(sql_statements, connection))
                {
                    cmd.Parameters.AddWithValue("@updated_at", updated_at);
                    cmd.Parameters.AddWithValue("@id", id);

                    await connection.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (!reader.HasRows)
                        {
                            return null;
                        }
                        
                        while (await reader.ReadAsync())
                        {
                            result = new User(
                            id: Convert.ToInt32(reader["id"]),
                            created_at: (DateTimeOffset)reader["created_at"],
                            updated_at: (DateTimeOffset)reader["updated_at"]
                            );
                        }
                    }
                }
            }
            return result;
        }

        #endregion User

        #region Url

        public async Task<List<Url>> GetAllUrl()
        {
            List<Url> url = new List<Url>();
            string sql_statements = """
                SELECT * FROM urls
                """;

            using (SqlConnection connection = new SqlConnection(_con_str))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(sql_statements, connection);
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (!reader.HasRows)
                    {
                        return null;
                    }

                    while (await reader.ReadAsync())
                    {
                        url.Add(new Url(
                            id: Convert.ToInt32(reader["id"]),
                            original_url: Convert.ToString(reader["original_url"]),
                            short_url: Convert.ToString(reader["short_url"]),
                            secret_access_token: Guid.Parse(reader["secret_access_token"].ToString()),
                            created_at: (DateTimeOffset)reader["created_at"],
                            creator_id: Convert.ToInt32(reader["created_by"]),
                            updated_at: reader["updated_at"] == DBNull.Value ? null : (DateTimeOffset)reader["updated_at"],
                            deleted_at: reader["deleted_at"] == DBNull.Value ? null : (DateTimeOffset)reader["deleted_at"]
                            ));
                    }
                }
            }
            return url;
        }

        public async Task<Url> GetUrl(int id)
        {
            string sql_statements = """
                SELECT * FROM urls
                WHERE id = @id
                """;

            using (SqlConnection connection = new SqlConnection(_con_str))
            {
                Url result = null;

                SqlCommand command = new SqlCommand(sql_statements, connection);
                command.Parameters.AddWithValue("@id", id);

                await connection.OpenAsync();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (!reader.HasRows)
                    {
                        return null;
                    }

                    while (await reader.ReadAsync())
                    {
                        result = new Url(
                            id: Convert.ToInt32(reader["id"]),
                            original_url: Convert.ToString(reader["original_url"]),
                            short_url: Convert.ToString(reader["short_url"]),
                            secret_access_token: Guid.Parse(reader["secret_access_token"].ToString()),
                            created_at: (DateTimeOffset)reader["created_at"],
                            creator_id: Convert.ToInt32(reader["created_by"]),
                            updated_at: reader["updated_at"] == DBNull.Value ? null : (DateTimeOffset)reader["updated_at"],
                            deleted_at: reader["deleted_at"] == DBNull.Value ? null : (DateTimeOffset)reader["deleted_at"]
                            );
                    }
                }
                return result;
            }
        }

        public async Task<Url> GetUrlByShort(string short_url)
        {
            string sql_statements = """
                SELECT * FROM urls
                WHERE short_url = @short_url
                """;

            using (SqlConnection connection = new SqlConnection(_con_str))
            {
                Url result = null;

                SqlCommand command = new SqlCommand(sql_statements, connection);
                command.Parameters.AddWithValue("@short_url", short_url);

                await connection.OpenAsync();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (!reader.HasRows)
                    {
                        return null;
                    }

                    while (await reader.ReadAsync())
                    {
                        result = new Url(
                            id: Convert.ToInt32(reader["id"]),
                            original_url: Convert.ToString(reader["original_url"]),
                            short_url: Convert.ToString(reader["short_url"]),
                            secret_access_token: Guid.Parse(reader["secret_access_token"].ToString()),
                            created_at: (DateTimeOffset)reader["created_at"],
                            creator_id: Convert.ToInt32(reader["created_by"]),
                            updated_at: reader["updated_at"] == DBNull.Value ? null : (DateTimeOffset)reader["updated_at"],
                            deleted_at: reader["deleted_at"] == DBNull.Value ? null : (DateTimeOffset)reader["deleted_at"]
                            );
                    }
                }
                return result;
            }
        }

        public async Task<int> CreateUrl(string original_url, string short_url, Guid secret_access_token, int creator_id)
        {
            int result;
            DateTimeOffset created_at = new DateTimeOffset(DateTime.Now);
            
            string sql_statements = """
                INSERT INTO urls (original_url, short_url, secret_access_token, created_by, created_at)
                VALUES (@original_url, @short_url, @secret_access_token, @created_by, @created_at);
                SET @id=SCOPE_IDENTITY();
                """;

            using (SqlConnection connection = new SqlConnection(_con_str))
            {
                using (SqlCommand cmd = new SqlCommand(sql_statements, connection))
                {
                    cmd.Parameters.AddWithValue("@original_url", original_url);
                    cmd.Parameters.AddWithValue("@short_url", short_url);
                    cmd.Parameters.AddWithValue("@secret_access_token", secret_access_token);
                    cmd.Parameters.AddWithValue("@created_by", creator_id);
                    cmd.Parameters.AddWithValue("@created_at", created_at);

                    SqlParameter id_returning = new SqlParameter
                    {
                        ParameterName = "@id",
                        SqlDbType = SqlDbType.Int,
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(id_returning);

                    await connection.OpenAsync();
                    await cmd.ExecuteNonQueryAsync();

                    result = Convert.ToInt32(id_returning.Value);
                }
            }
            return result;
        }
        #endregion Url
    }
}
