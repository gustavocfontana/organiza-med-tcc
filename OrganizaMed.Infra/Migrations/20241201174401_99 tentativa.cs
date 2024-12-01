using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrganizaMed.Infra.Migrations
{
    /// <inheritdoc />
    public partial class _99tentativa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TBAtividades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFim = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TipoAtividade = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBAtividades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TBMedicos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "varchar(100)", nullable: false),
                    Crm = table.Column<string>(type: "varchar(8)", nullable: false),
                    Especialidade = table.Column<string>(type: "varchar(100)", nullable: false),
                    HorasTrabalhadas = table.Column<double>(type: "float", nullable: false),
                    Ranking = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TBMedicos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AtividadeMedicos",
                columns: table => new
                {
                    AtividadesId = table.Column<int>(type: "int", nullable: false),
                    MedicosEnvolvidosId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtividadeMedicos", x => new { x.AtividadesId, x.MedicosEnvolvidosId });
                    table.ForeignKey(
                        name: "FK_AtividadeMedicos_TBAtividades_AtividadesId",
                        column: x => x.AtividadesId,
                        principalTable: "TBAtividades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AtividadeMedicos_TBMedicos_MedicosEnvolvidosId",
                        column: x => x.MedicosEnvolvidosId,
                        principalTable: "TBMedicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AtividadeMedicos_MedicosEnvolvidosId",
                table: "AtividadeMedicos",
                column: "MedicosEnvolvidosId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AtividadeMedicos");

            migrationBuilder.DropTable(
                name: "TBAtividades");

            migrationBuilder.DropTable(
                name: "TBMedicos");
        }
    }
}
