namespace Uceme.UI.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddTechniqueName : Migration
    {
#pragma warning disable CA1062 // Validate arguments of public methods
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "nombre",
                table: "Tecnica",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "nombre",
                table: "Tecnica");
        }
#pragma warning restore CA1062 // Validate arguments of public methods
    }
}
