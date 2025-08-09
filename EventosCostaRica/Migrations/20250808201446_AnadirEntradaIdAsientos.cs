using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventosCostaRica.Migrations
{
    /// <inheritdoc />
    public partial class AnadirEntradaIdAsientos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fila",
                table: "Entradas");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCompra",
                table: "Entradas",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "EntradaId",
                table: "Asientos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Asientos_EntradaId",
                table: "Asientos",
                column: "EntradaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Asientos_Entradas_EntradaId",
                table: "Asientos",
                column: "EntradaId",
                principalTable: "Entradas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Asientos_Entradas_EntradaId",
                table: "Asientos");

            migrationBuilder.DropIndex(
                name: "IX_Asientos_EntradaId",
                table: "Asientos");

            migrationBuilder.DropColumn(
                name: "FechaCompra",
                table: "Entradas");

            migrationBuilder.DropColumn(
                name: "EntradaId",
                table: "Asientos");

            migrationBuilder.AddColumn<string>(
                name: "Fila",
                table: "Entradas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
