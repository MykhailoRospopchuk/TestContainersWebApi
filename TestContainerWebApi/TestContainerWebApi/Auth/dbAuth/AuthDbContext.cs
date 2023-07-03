using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TestContainerWebApi.Auth.AuthModel;
using TestContainerWebApi.Models.ModelDto;

namespace TestContainerWebApi.Auth.dbAuth
{
    public class AuthDbContext : DbContext
    {
        private readonly string _conStr;

        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
            _conStr = Database.GetConnectionString();
        }

        #region UserAuth

        public async Task<List<UserAuth>> GetAllUsersAuth()
        {
            List<UserAuth> users = new List<UserAuth>();
            string sql_statements = """
                SELECT users.id, users.email, users.password, users.roleId, roleAuth.role FROM users
                LEFT JOIN roleAuth ON users.roleId = roleAuth.id;
                """;

            using (SqlConnection connection = new SqlConnection(_conStr))
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
                        users.Add(new UserAuth
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Email = Convert.ToString(reader["email"]),
                            Password = Convert.ToString(reader["password"]),
                            RoleId = Convert.ToInt32(reader["roleId"]),
                            Role = new RoleAuth { Id = Convert.ToInt32(reader["roleId"]), Name = Convert.ToString(reader["role"]) }
                        });
                    }
                }
            }
            return users;
        }

        public async Task<UserAuth> GetUserByIdAuth(int id)
        {
            string sql_statements = """
                SELECT users.id, users.email, users.password, users.roleId, roleAuth.role FROM users
                LEFT JOIN roleAuth ON users.roleId = roleAuth.id;
                WHERE users.id = @id
                """;

            using (SqlConnection connection = new SqlConnection(_conStr))
            {
                UserAuth result = null;

                SqlCommand command = new SqlCommand(sql_statements, connection);
                command.Parameters.AddWithValue("@id", id);

                await connection.OpenAsync();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        result = new UserAuth
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Email = Convert.ToString(reader["email"]),
                            Password = Convert.ToString(reader["password"]),
                            RoleId = Convert.ToInt32(reader["roleId"]),
                            Role = new RoleAuth { Id = Convert.ToInt32(reader["roleId"]), Name = Convert.ToString(reader["role"]) }
                        };
                    }
                }
                return result;
            }
        }

        public async Task<UserAuth> GetUserByEmailAuth(string email)
        {
            string sql_statements = """
                SELECT users.id, users.email, users.password, users.roleId, roleAuth.role FROM users
                LEFT JOIN roleAuth ON users.roleId = roleAuth.id
                WHERE users.email = @email
                """;

            using (SqlConnection connection = new SqlConnection(_conStr))
            {
                UserAuth result = null;

                SqlCommand command = new SqlCommand(sql_statements, connection);
                command.Parameters.AddWithValue("@email", email);

                await connection.OpenAsync();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        result = new UserAuth
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Email = Convert.ToString(reader["email"]),
                            Password = Convert.ToString(reader["password"]),
                            RoleId = Convert.ToInt32(reader["roleId"]),
                            Role = new RoleAuth { Id = Convert.ToInt32(reader["roleId"]), Name = Convert.ToString(reader["role"]) }
                        };
                    }
                }
                return result;
            }
        }

        public async Task<int> CreateUserAuth(UserAuth incomeUser)
        {
            int result;
            DateTimeOffset created_at = new DateTimeOffset(DateTime.Now);
            DateTimeOffset updated_at = new DateTimeOffset(DateTime.Now);
            string sql_statements = """
                INSERT INTO users (email, password, roleId, created_at, updated_at)
                VALUES (@email, @password, @roleId, @created_at, @updated_at);
                SET @id=SCOPE_IDENTITY();
                """;

            using (SqlConnection connection = new SqlConnection(_conStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql_statements, connection))
                {
                    cmd.Parameters.Add("@email", SqlDbType.VarChar).Value = incomeUser.Email;
                    cmd.Parameters.Add("@password", SqlDbType.VarChar).Value = incomeUser.Password;
                    cmd.Parameters.Add("@roleId", SqlDbType.Int).Value = incomeUser.RoleId;
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

        #endregion UserAuth

        #region RoleAuth

        public async Task<RoleAuth> GetRoleByNameAuth(string name)
        {
            string sql_statements = """
                SELECT * FROM roleAuth
                WHERE role = @name
                """;

            using (SqlConnection connection = new SqlConnection(_conStr))
            {
                RoleAuth result = null;

                SqlCommand command = new SqlCommand(sql_statements, connection);
                command.Parameters.AddWithValue("@name", name);

                await connection.OpenAsync();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        result = new RoleAuth
                        {
                            Id = reader.GetInt32("Id"),
                            Name = reader.GetString("role")
                        };
                    }
                }
                return result;
            }
        }

        #endregion RoleAuth

    }
}
