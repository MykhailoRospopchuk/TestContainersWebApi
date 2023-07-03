using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using TestContainerWebApi.Models;
using TestContainerWebApi.Models.ModelDto;

namespace TestContainerWebApi.db
{
    public class AdoDbContext : DbContext
    {
        private readonly string _conStr;

        public AdoDbContext(DbContextOptions<AdoDbContext> options) : base(options)
        {
            _conStr = Database.GetConnectionString();
        }

        #region User

        public async Task<List<UserDto>> GetAllUsers()
        {
            List<UserDto> users = new List<UserDto>();
            string sql_statements = """
                SELECT * FROM users
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
                        users.Add(new UserDto(
                            id: Convert.ToInt32(reader["id"]),
                            createdAt: (DateTimeOffset)reader["created_at"],
                            updatedAt: (DateTimeOffset)reader["updated_at"]
                            ));
                    }
                }
            }
            return users;
        }

        public async Task<UserDto> GetUser(int id)
        {
            string sql_statements = """
                SELECT * FROM users
                WHERE id = @id
                """;

            using (SqlConnection connection = new SqlConnection(_conStr))
            {
                UserDto result = null;

                SqlCommand command = new SqlCommand(sql_statements, connection);
                command.Parameters.AddWithValue("@id", id);

                await connection.OpenAsync();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        result = new UserDto(
                            id: Convert.ToInt32(reader["id"]),
                            createdAt: (DateTimeOffset)reader["created_at"],
                            updatedAt: (DateTimeOffset)reader["updated_at"]
                            );
                    }
                }
                return result;
            }
        }

        public async Task<UserDto> UpdateUser(int id)
        {
            UserDto result = null;
            DateTimeOffset updated_at = new DateTimeOffset(DateTime.Now);
            string sql_statements = """
                UPDATE users
                SET updated_at = @updated_at
                OUTPUT INSERTED.*
                WHERE id = @id
                """;

            using (SqlConnection connection = new SqlConnection(_conStr))
            {
                using (SqlCommand cmd = new SqlCommand(sql_statements, connection))
                {
                    cmd.Parameters.AddWithValue("@updated_at", updated_at);
                    cmd.Parameters.AddWithValue("@id", id);

                    await connection.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result = new UserDto(
                            id: Convert.ToInt32(reader["id"]),
                            createdAt: (DateTimeOffset)reader["created_at"],
                            updatedAt: (DateTimeOffset)reader["updated_at"]
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
                        url.Add(new Url(
                            id: Convert.ToInt32(reader["id"]),
                            originalUrl: Convert.ToString(reader["original_url"]),
                            shortUrl: Convert.ToString(reader["short_url"]),
                            secretAccessToken: Guid.Parse(reader["secret_access_token"].ToString()),
                            createdAt: (DateTimeOffset)reader["created_at"],
                            creatorId: Convert.ToInt32(reader["created_by"]),
                            updatedAt: reader["updated_at"] == DBNull.Value ? null : (DateTimeOffset)reader["updated_at"],
                            deletedAt: reader["deleted_at"] == DBNull.Value ? null : (DateTimeOffset)reader["deleted_at"]
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

            using (SqlConnection connection = new SqlConnection(_conStr))
            {
                Url result = null;

                SqlCommand command = new SqlCommand(sql_statements, connection);
                command.Parameters.AddWithValue("@id", id);

                await connection.OpenAsync();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        result = new Url(
                            id: Convert.ToInt32(reader["id"]),
                            originalUrl: Convert.ToString(reader["original_url"]),
                            shortUrl: Convert.ToString(reader["short_url"]),
                            secretAccessToken: Guid.Parse(reader["secret_access_token"].ToString()),
                            createdAt: (DateTimeOffset)reader["created_at"],
                            creatorId: Convert.ToInt32(reader["created_by"]),
                            updatedAt: reader["updated_at"] == DBNull.Value ? null : (DateTimeOffset)reader["updated_at"],
                            deletedAt: reader["deleted_at"] == DBNull.Value ? null : (DateTimeOffset)reader["deleted_at"]
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

            using (SqlConnection connection = new SqlConnection(_conStr))
            {
                Url result = null;

                SqlCommand command = new SqlCommand(sql_statements, connection);
                command.Parameters.AddWithValue("@short_url", short_url);

                await connection.OpenAsync();
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        result = new Url(
                            id: Convert.ToInt32(reader["id"]),
                            originalUrl: Convert.ToString(reader["original_url"]),
                            shortUrl: Convert.ToString(reader["short_url"]),
                            secretAccessToken: Guid.Parse(reader["secret_access_token"].ToString()),
                            createdAt: (DateTimeOffset)reader["created_at"],
                            creatorId: Convert.ToInt32(reader["created_by"]),
                            updatedAt: reader["updated_at"] == DBNull.Value ? null : (DateTimeOffset)reader["updated_at"],
                            deletedAt: reader["deleted_at"] == DBNull.Value ? null : (DateTimeOffset)reader["deleted_at"]
                            );
                    }
                }
                return result;
            }
        }

        public async Task<int> CreateUrl(string originalUrl, string shortUrl, Guid secretAccessToken, int creatorId)
        {
            int result;
            DateTimeOffset createdAt = new DateTimeOffset(DateTime.Now);
            
            string sqlStatements = """
                INSERT INTO urls (original_url, short_url, secret_access_token, created_by, created_at)
                VALUES (@originalUrl, @shortUrl, @secretAccessToken, @createdBy, @createdAt);
                SET @id=SCOPE_IDENTITY();
                """;

            using (SqlConnection connection = new SqlConnection(_conStr))
            {
                using (SqlCommand cmd = new SqlCommand(sqlStatements, connection))
                {
                    cmd.Parameters.AddWithValue("@originalUrl", originalUrl);
                    cmd.Parameters.AddWithValue("@shortUrl", shortUrl);
                    cmd.Parameters.AddWithValue("@secretAccessToken", secretAccessToken);
                    cmd.Parameters.AddWithValue("@createdBy", creatorId);
                    cmd.Parameters.AddWithValue("@createdAt", createdAt);

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

        public async Task<Url> UpdateUrl(int urlId, string newUrl, Guid secretAccessToken, DateTimeOffset? updatedAt)
        {
            Url result = null;

            string sqlStatements = """
                UPDATE urls
                SET original_url = @newUrl,
                    updated_at = @updatedAt
                OUTPUT INSERTED.*
                WHERE id = @urlId
                    AND secret_access_token = @secretAccessToken;
                """;

            using (SqlConnection connection = new SqlConnection(_conStr))
            {
                using (SqlCommand cmd = new SqlCommand(sqlStatements, connection))
                {
                    cmd.Parameters.AddWithValue("@newUrl", newUrl);
                    cmd.Parameters.AddWithValue("@updatedAt", updatedAt);
                    cmd.Parameters.AddWithValue("@urlId", urlId);
                    cmd.Parameters.AddWithValue("@secretAccessToken", secretAccessToken);

                    await connection.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result = new Url(
                                id: Convert.ToInt32(reader["id"]),
                                originalUrl: Convert.ToString(reader["original_url"]),
                                shortUrl: Convert.ToString(reader["short_url"]),
                                secretAccessToken: Guid.Parse(reader["secret_access_token"].ToString()),
                                createdAt: (DateTimeOffset)reader["created_at"],
                                creatorId: Convert.ToInt32(reader["created_by"]),
                                updatedAt: reader["updated_at"] == DBNull.Value ? null : (DateTimeOffset)reader["updated_at"],
                                deletedAt: reader["deleted_at"] == DBNull.Value ? null : (DateTimeOffset)reader["deleted_at"]
                                );
                        }
                    }
                }
            }
            return result;
        }

        public async Task<Url> DeleteUrl(int urlId, Guid secretAccessToken, DateTimeOffset? deletedAt)
        {
            Url result = null;

            string sqlStatements = """
                UPDATE urls
                SET deleted_at = @deletedAt
                OUTPUT INSERTED.*
                WHERE id = @urlId
                    AND secret_access_token = @secretAccessToken;
                """;

            using (SqlConnection connection = new SqlConnection(_conStr))
            {
                using (SqlCommand cmd = new SqlCommand(sqlStatements, connection))
                {
                    cmd.Parameters.AddWithValue("@deletedAt", deletedAt);
                    cmd.Parameters.AddWithValue("@urlId", urlId);
                    cmd.Parameters.AddWithValue("@secretAccessToken", secretAccessToken);

                    await connection.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result = new Url(
                                id: Convert.ToInt32(reader["id"]),
                                originalUrl: Convert.ToString(reader["original_url"]),
                                shortUrl: Convert.ToString(reader["short_url"]),
                                secretAccessToken: Guid.Parse(reader["secret_access_token"].ToString()),
                                createdAt: (DateTimeOffset)reader["created_at"],
                                creatorId: Convert.ToInt32(reader["created_by"]),
                                updatedAt: reader["updated_at"] == DBNull.Value ? null : (DateTimeOffset)reader["updated_at"],
                                deletedAt: reader["deleted_at"] == DBNull.Value ? null : (DateTimeOffset)reader["deleted_at"]
                                );
                        }
                    }
                }
            }
            return result;
        }



        #endregion Url

        #region View

        public async Task<View> CreateView(int urlId)
        {
            View result = null;
            DateTime viewAt = DateTime.Now;

            string sqlStatements = """
                MERGE INTO hour_views AS target
                USING (VALUES (@urlId, DATEADD(HOUR, DATEDIFF(HOUR, 0, @viewAt), 0))) AS source (url_id, hour_time)
                    ON target.url_id = source.url_id AND target.hour_time = source.hour_time
                WHEN MATCHED THEN
                    UPDATE SET target.count = target.count + 1
                WHEN NOT MATCHED THEN
                    INSERT (url_id, hour_time, count)
                    VALUES (source.url_id, source.hour_time, 1)
                OUTPUT inserted.*;
                """;

            using (SqlConnection connection = new SqlConnection(_conStr))
            {
                using (SqlCommand cmd = new SqlCommand(sqlStatements, connection))
                {
                    cmd.Parameters.AddWithValue("@urlId", urlId);
                    cmd.Parameters.AddWithValue("@viewAt", viewAt);

                    await connection.OpenAsync();

                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            result = new View(
                                urlId: Convert.ToInt32(reader["url_id"]),
                                viewAt: (DateTime)reader["hour_time"],
                                count: Convert.ToInt32(reader["count"])
                                );
                        }
                    }
                }
            }
            return result;
        }

        public async Task<List<ViewStat>> GetViewStats(Guid secretAccessToken)
        {
            List<ViewStat> viewStat = new List<ViewStat>();


            string sql_statements = """
                SELECT day_series.days AS [DAY],
                    ISNULL(stats.[count], 0) AS [count]
                FROM (
                    SELECT DATEADD(DAY, DATEDIFF(DAY, 0, dd), 0) AS days
                    FROM (
                        SELECT TOP 36 ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) - 33 AS n
                        FROM sys.columns AS c1
                        CROSS JOIN sys.columns AS c2
                    ) AS nums
                    CROSS APPLY (
                        SELECT DATEADD(DAY, nums.n, GETDATE()) AS dd
                    ) AS dates
                ) AS day_series
                LEFT JOIN (
                    SELECT CAST(DATEADD(DAY, DATEDIFF(DAY, 0, hour_time), 0) AS DATE) AS [DAY],
                        SUM([count]) AS [count]
                    FROM hour_views
                    WHERE url_id = (
                        SELECT id
                        FROM urls
                        WHERE secret_access_token = @token
                    )
                    AND hour_time >= DATEADD(MONTH, -1, GETDATE())
                    GROUP BY CAST(DATEADD(DAY, DATEDIFF(DAY, 0, hour_time), 0) AS DATE)
                ) AS stats ON day_series.days = stats.[DAY]
                ORDER BY day_series.days;
                """;

            using (SqlConnection connection = new SqlConnection(_conStr))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand(sql_statements, connection);
                command.Parameters.AddWithValue("@token", secretAccessToken);
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    if (!reader.HasRows)
                    {
                        return null;
                    }

                    while (await reader.ReadAsync())
                    {
                        viewStat.Add(new ViewStat(
                            day: (DateTime)reader["DAY"],
                            count: Convert.ToInt32(reader["count"])
                            ));
                    }
                }
            }
            return viewStat;
        }

        #endregion View
    }
}
