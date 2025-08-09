using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventosCostaRica.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCampoFilaAsientos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Fila",
                table: "Asientos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fila",
                table: "Asientos");
        }
    }
}
