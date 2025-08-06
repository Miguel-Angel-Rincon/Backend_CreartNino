using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api_CreartNino.Migrations
{
    /// <inheritdoc />
    public partial class EliminarTablaMarca : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Compras__IdMarca__6D0D32F4",
                table: "Compras");

            migrationBuilder.DropForeignKey(
                name: "FK__Insumos__IdMarca__656C112C",
                table: "Insumos");

            migrationBuilder.DropTable(
                name: "Marca");

            migrationBuilder.DropIndex(
                name: "IX_Insumos_IdMarca",
                table: "Insumos");

            migrationBuilder.DropIndex(
                name: "IX_Compras_IdMarca",
                table: "Compras");

            migrationBuilder.DropColumn(
                name: "IdMarca",
                table: "Insumos");

            migrationBuilder.DropColumn(
                name: "IdMarca",
                table: "Compras");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdMarca",
                table: "Insumos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "IdMarca",
                table: "Compras",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Marca",
                columns: table => new
                {
                    IdMarca = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreMarca = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Marca__4076A8873CD5B467", x => x.IdMarca);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Insumos_IdMarca",
                table: "Insumos",
                column: "IdMarca");

            migrationBuilder.CreateIndex(
                name: "IX_Compras_IdMarca",
                table: "Compras",
                column: "IdMarca");

            migrationBuilder.AddForeignKey(
                name: "FK__Compras__IdMarca__6D0D32F4",
                table: "Compras",
                column: "IdMarca",
                principalTable: "Marca",
                principalColumn: "IdMarca");

            migrationBuilder.AddForeignKey(
                name: "FK__Insumos__IdMarca__656C112C",
                table: "Insumos",
                column: "IdMarca",
                principalTable: "Marca",
                principalColumn: "IdMarca");
        }
    }
}
