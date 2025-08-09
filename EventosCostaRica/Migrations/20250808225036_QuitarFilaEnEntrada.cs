using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventosCostaRica.Migrations
{
    /// <inheritdoc />
    public partial class QuitarFilaEnEntrada : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Numero",
                table: "Entradas");

            migrationBuilder.AlterColumn<string>(
                name: "Apellidos",
                table: "Usuarios",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Apellidos",
                table: "Usuarios",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "Numero",
                table: "Entradas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
