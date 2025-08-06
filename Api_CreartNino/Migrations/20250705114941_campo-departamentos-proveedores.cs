using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api_CreartNino.Migrations
{
    /// <inheritdoc />
    public partial class campodepartamentosproveedores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Departamento",
                table: "Proveedores",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Departamento",
                table: "Proveedores");
        }
    }
}
