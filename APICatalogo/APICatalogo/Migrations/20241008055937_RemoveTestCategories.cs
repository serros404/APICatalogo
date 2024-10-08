using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APICatalogo.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTestCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder mb)
        {
            mb.Sql("DELETE FROM Categorias WHERE Nome NOT IN ('Lanches', 'Sobremesas', 'Bebidas')");
        }

        protected override void Down(MigrationBuilder mb)
        {
            // Opcionalmente, você pode reverter a remoção aqui
        }

    }
}
