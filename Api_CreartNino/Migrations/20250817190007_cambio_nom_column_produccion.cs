using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api_CreartNino.Migrations
{
    /// <inheritdoc />
    public partial class cambio_nom_column_produccion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("EXEC sp_rename 'Produccion.FechaRegistro', 'FechaInicio', 'COLUMN';");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("EXEC sp_rename 'Produccion.FechaInicio', 'FechaRegistro', 'COLUMN';");
        }
    }
}
