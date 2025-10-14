using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TDRevision.Migrations
{
    /// <inheritdoc />
    public partial class CreationDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "T_E_UTILISATEUR_UTI",
                schema: "public",
                columns: table => new
                {
                    UTI_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UTI_NOM = table.Column<string>(type: "text", nullable: true),
                    UTI_PRENOM = table.Column<string>(type: "text", nullable: true),
                    UTI_NUMERORUE = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    UTI_RUE = table.Column<string>(type: "text", nullable: true),
                    UTI_CODEPOSTAL = table.Column<int>(type: "integer", maxLength: 5, nullable: false),
                    UTI_VILLE = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("UTI_ID", x => x.UTI_ID);
                });

            migrationBuilder.CreateTable(
                name: "T_E_COMMANDE_COM",
                schema: "public",
                columns: table => new
                {
                    COM_ID = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    COM_NOMARTICLE = table.Column<string>(type: "text", nullable: true),
                    UTI_ID = table.Column<int>(type: "integer", nullable: false),
                    COM_MONTANT = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("COM_ID", x => x.COM_ID);
                    table.ForeignKey(
                        name: "UTI_ID",
                        column: x => x.UTI_ID,
                        principalSchema: "public",
                        principalTable: "T_E_UTILISATEUR_UTI",
                        principalColumn: "UTI_ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_T_E_COMMANDE_COM_UTI_ID",
                schema: "public",
                table: "T_E_COMMANDE_COM",
                column: "UTI_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "T_E_COMMANDE_COM",
                schema: "public");

            migrationBuilder.DropTable(
                name: "T_E_UTILISATEUR_UTI",
                schema: "public");
        }
    }
}
