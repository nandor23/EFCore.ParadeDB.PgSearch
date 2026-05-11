using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ParadeDB.EFCore.IntegrationTests.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:pg_search", ",,");

            migrationBuilder.CreateTable(
                name: "items",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_items", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    details = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_products", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_items_id_description_name",
                table: "items",
                columns: new[] { "id", "description", "name" })
                .Annotation("Npgsql:IndexMethod", "bm25")
                .Annotation("Npgsql:StorageParameter:key_field", "id")
                .Annotation("ParadeDb:ColumnTokenizers", new[] { null, "pdb.simple('alias=description_simple')", null });

            migrationBuilder.CreateIndex(
                name: "ix_products_id_description_name",
                table: "products",
                columns: new[] { "id", "description", "name" })
                .Annotation("Npgsql:IndexMethod", "bm25")
                .Annotation("Npgsql:StorageParameter:key_field", "id")
                .Annotation("ParadeDb:AdditionalIndexExpressions", new[] { "((details->>'Summary')::pdb.literal('alias=details_summary'))", "((details->>'Summary')::pdb.icu('alias=details_summary_icu'))", "((details->>'Tags')::pdb.icu('alias=details_tags'))" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "items");

            migrationBuilder.DropTable(
                name: "products");
        }
    }
}
