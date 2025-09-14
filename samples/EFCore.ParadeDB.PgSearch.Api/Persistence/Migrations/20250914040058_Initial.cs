using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EFCore.ParadeDB.PgSearch.Api.Persistence.Migrations
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

            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "id", "description", "name" },
                values: new object[,]
                {
                    { 1L, "Lightweight shoes designed for everyday running", "Running Shoes" },
                    {
                        2L,
                        "Durable keyboard with tactile switches and RGB lighting",
                        "Mechanical Keyboard",
                    },
                    {
                        3L,
                        "Over-ear headphones with active noise cancellation",
                        "Noise Cancelling Headphones",
                    },
                    { 4L, "Fitness-focused smartwatch with heart rate monitor", "Smart Watch" },
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
