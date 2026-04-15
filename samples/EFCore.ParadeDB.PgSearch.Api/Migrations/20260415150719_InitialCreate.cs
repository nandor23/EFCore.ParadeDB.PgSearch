using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EFCore.ParadeDB.PgSearch.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
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
                    description = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_products", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "items",
                columns: new[] { "id", "description", "name" },
                values: new object[,]
                {
                    { 1L, "A vintage leather journal featuring hand-stitched binding, aged parchment pages, and a brass clasp closure.\nEach page has a subtle texture that makes writing a tactile pleasure, and the cover develops a rich patina over time.\nPerfect for travelers, writers, and anyone who appreciates the art of analog note-taking.", "Vintage Leather Journal" },
                    { 2L, "A premium bamboo cutting board with a deep juice groove along the perimeter to catch liquids during food prep.\nFeatures a built-in herb stripper, a hanging hole for easy storage, and a non-slip base to keep it firmly in place.\nNaturally antimicrobial and gentle on knife edges, making it an essential kitchen companion.", "Bamboo Cutting Board" },
                    { 3L, "A handcrafted ceramic pour-over coffee dripper paired with a solid walnut stand and a reusable stainless mesh filter.\nThe carefully angled interior ribs slow the flow rate for optimal extraction, producing a clean and nuanced cup.\nDesigned for coffee enthusiasts who treat their morning brew as a ritual rather than a routine.", "Ceramic Pour-Over Coffee Dripper" },
                    { 4L, "A ultra-soft Merino wool travel blanket that folds and compresses neatly into its own integrated carry pouch.\nNaturally temperature-regulating and odor-resistant, it keeps you warm on cold flights and cool in mild conditions.\nLightweight enough to slip into a carry-on, yet substantial enough to replace a full-size throw at home.", "Merino Wool Travel Blanket" },
                    { 5L, "A solar-powered desk lamp with a flexible gooseneck arm, adjustable color temperature ranging from warm amber to cool daylight,\nand a built-in USB-A charging port to keep your devices powered throughout the day.\nThe wide solar panel charges efficiently even in indirect light, making it ideal for eco-conscious home and office setups.", "Solar-Powered Desk Lamp" }
                });

            migrationBuilder.InsertData(
                table: "products",
                columns: new[] { "id", "description", "name" },
                values: new object[,]
                {
                    { 1L, "Experience unparalleled comfort with the UltraComfort Memory Foam Running Shoes.\nDesigned for athletes and casual joggers alike, these shoes feature a breathable mesh upper,\na cushioned memory foam insole that molds to your foot's unique shape, and a durable rubber outsole.\nWhether you're hitting the pavement or enjoying a leisurely walk, these shoes provide the support\nand comfort you need.", "UltraComfort Memory Foam Running Shoes" },
                    { 2L, "Elevate your running experience with the ProStride Jogging Sneakers.\nCrafted with a lightweight, breathable mesh upper, these sneakers offer optimal ventilation\nto keep your feet cool. The responsive cushioning system absorbs impact, reducing strain on your joints,\nwhile the slip-resistant rubber outsole ensures stability on various surfaces.\nPerfect for both seasoned runners and beginners.", "ProStride Jogging Sneakers" },
                    { 3L, "Immerse yourself in high-quality sound with the NoiseCancel Wireless Headphones.\nFeaturing advanced noise-cancelling technology, these headphones block out ambient noise,\nallowing you to focus on your music or calls. The ergonomic design ensures a comfortable fit\nfor extended wear, and the long-lasting battery provides hours of uninterrupted listening.\nIdeal for travel, work, or leisure.", "NoiseCancel Wireless Headphones" },
                    { 4L, "Discover exceptional audio clarity with the StudioSound Over-Ear Headphones.\nEquipped with premium drivers, these headphones deliver rich bass and crisp highs,\nproviding an immersive listening experience. The plush ear cups and adjustable headband\noffer a personalized fit, while the foldable design makes them easy to store and transport.\nSuitable for audiophiles and casual listeners alike.", "StudioSound Over-Ear Headphones" },
                    { 5L, "Brew your favorite coffee with the EcoBrew French Press Coffee Maker.\nMade from high-quality borosilicate glass and stainless steel, this French press ensures durability\nand heat retention. The fine mesh filter allows essential oils and fine particles to pass through,\ndelivering a rich and full-bodied cup of coffee. Its sleek design and easy-to-use mechanism\nmake it a must-have for coffee enthusiasts.", "EcoBrew French Press Coffee Maker" }
                });
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
