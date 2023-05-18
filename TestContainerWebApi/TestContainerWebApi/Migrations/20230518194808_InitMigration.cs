using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestContainerWebApi.Migrations
{
    /// <inheritdoc />
    public partial class InitMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sql_command = Path.Combine("Migrations/20230518194808_InitMigration.sql");
            migrationBuilder.Sql(File.ReadAllText(sql_command));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sql_command = Path.Combine("Migrations/20230518194808_InitMigration_Down.sql");
            migrationBuilder.Sql(File.ReadAllText(sql_command));
        }
    }
}
