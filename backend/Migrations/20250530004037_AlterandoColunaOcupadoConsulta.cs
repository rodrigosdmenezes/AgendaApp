using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgendaApp.Migrations
{
    /// <inheritdoc />
    public partial class AlterandoColunaOcupadoConsulta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ocupado",
                table: "Consultas");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Consultas",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Consultas");

            migrationBuilder.AddColumn<bool>(
                name: "Ocupado",
                table: "Consultas",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
