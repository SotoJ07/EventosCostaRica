using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventosCostaRica.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCampoCapacidadAEventos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Capacidad",
                table: "Eventos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Capacidad",
                table: "Eventos");
        }
    }
}
