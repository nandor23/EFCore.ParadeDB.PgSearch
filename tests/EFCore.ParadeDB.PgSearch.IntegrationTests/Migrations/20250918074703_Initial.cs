using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace EFCore.ParadeDB.PgSearch.IntegrationTests.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase().Annotation("Npgsql:PostgresExtension:pg_search", ",,");

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table
                        .Column<long>(type: "bigint", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_products", x => x.id);
                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "products");
        }
    }
}
