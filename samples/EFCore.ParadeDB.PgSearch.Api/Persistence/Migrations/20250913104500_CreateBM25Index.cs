using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCore.ParadeDB.PgSearch.Api.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CreateBM25Index : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"CREATE INDEX search_idx ON products
                  USING bm25 (id, name, description)
                  WITH (key_field='id')"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP INDEX IF EXISTS search_idx");
        }
    }
}
