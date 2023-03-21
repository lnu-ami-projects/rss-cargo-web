using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RSSCargo.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RSSCargoCreateDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cargos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    name = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("cargos_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    username = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    password = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pk", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "cargo_feeds",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    cargo_id = table.Column<int>(type: "integer", nullable: false),
                    rss_feed = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("cargo_feeds_pk", x => x.id);
                    table.ForeignKey(
                        name: "cargo_feeds_cargo_id_fk",
                        column: x => x.cargo_id,
                        principalTable: "cargos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_cargos",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    cargo_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_cargos_pk", x => x.id);
                    table.ForeignKey(
                        name: "user_cargos_cargo_id_fk",
                        column: x => x.cargo_id,
                        principalTable: "cargos",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "user_cargos_user_id_fk",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_feeds",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    rss_feed = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_feeds_pk", x => x.id);
                    table.ForeignKey(
                        name: "user_cargos_user_id_fk",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "cargo_feeds_rss_feed_index",
                table: "cargo_feeds",
                columns: new[] { "cargo_id", "rss_feed" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "cargos_name_index",
                table: "cargos",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_cargos_cargo_id",
                table: "user_cargos",
                column: "cargo_id");

            migrationBuilder.CreateIndex(
                name: "user_cargos_cargo_id_index",
                table: "user_cargos",
                columns: new[] { "user_id", "cargo_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "user_feeds_rss_feed_index",
                table: "user_feeds",
                columns: new[] { "user_id", "rss_feed" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "users_email_index",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cargo_feeds");

            migrationBuilder.DropTable(
                name: "user_cargos");

            migrationBuilder.DropTable(
                name: "user_feeds");

            migrationBuilder.DropTable(
                name: "cargos");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
