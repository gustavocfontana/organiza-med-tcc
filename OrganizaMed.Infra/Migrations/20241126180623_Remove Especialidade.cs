using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrganizaMed.Infra.Migrations
{
    /// <inheritdoc />
    public partial class RemoveEspecialidade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TBMedicos_Atividades_AtividadeId",
                table: "TBMedicos");

            migrationBuilder.DropIndex(
                name: "IX_TBMedicos_AtividadeId",
                table: "TBMedicos");

            migrationBuilder.DropColumn(
                name: "AtividadeId",
                table: "TBMedicos");

            migrationBuilder.CreateTable(
                name: "AtividadeMedico",
                columns: table => new
                {
                    AtividadesId = table.Column<int>(type: "int", nullable: false),
                    MedicosEnvolvidosId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AtividadeMedico", x => new { x.AtividadesId, x.MedicosEnvolvidosId });
                    table.ForeignKey(
                        name: "FK_AtividadeMedico_Atividades_AtividadesId",
                        column: x => x.AtividadesId,
                        principalTable: "Atividades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AtividadeMedico_TBMedicos_MedicosEnvolvidosId",
                        column: x => x.MedicosEnvolvidosId,
                        principalTable: "TBMedicos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AtividadeMedico_MedicosEnvolvidosId",
                table: "AtividadeMedico",
                column: "MedicosEnvolvidosId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AtividadeMedico");

            migrationBuilder.AddColumn<int>(
                name: "AtividadeId",
                table: "TBMedicos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TBMedicos_AtividadeId",
                table: "TBMedicos",
                column: "AtividadeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TBMedicos_Atividades_AtividadeId",
                table: "TBMedicos",
                column: "AtividadeId",
                principalTable: "Atividades",
                principalColumn: "Id");
        }
    }
}
