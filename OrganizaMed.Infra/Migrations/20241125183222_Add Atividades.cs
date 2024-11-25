using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrganizaMed.Infra.Migrations
{
    /// <inheritdoc />
    public partial class AddAtividades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AtividadeId",
                table: "TBMedicos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Atividades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DataFim = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atividades", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TBMedicos_Atividades_AtividadeId",
                table: "TBMedicos");

            migrationBuilder.DropTable(
                name: "Atividades");

            migrationBuilder.DropIndex(
                name: "IX_TBMedicos_AtividadeId",
                table: "TBMedicos");

            migrationBuilder.DropColumn(
                name: "AtividadeId",
                table: "TBMedicos");
        }
    }
}
