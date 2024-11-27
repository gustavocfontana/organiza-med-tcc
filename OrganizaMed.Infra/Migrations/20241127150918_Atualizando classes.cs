using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrganizaMed.Infra.Migrations
{
    /// <inheritdoc />
    public partial class Atualizandoclasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "Atividades");

            migrationBuilder.AddColumn<int>(
                name: "TipoAtividade",
                table: "Atividades",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoAtividade",
                table: "Atividades");

            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                table: "Atividades",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
