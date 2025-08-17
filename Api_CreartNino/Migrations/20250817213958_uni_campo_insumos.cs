using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api_CreartNino.Migrations
{
    /// <inheritdoc />
    public partial class uni_campo_insumos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("EXEC sp_rename 'Insumos.Descripcion', 'UnidadesMedidas', 'COLUMN';");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("EXEC sp_rename 'Insumos.UnidadesMedidas', 'Descripcion', 'COLUMN';");
        }
    }
}
