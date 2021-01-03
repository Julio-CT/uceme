namespace Uceme.UI.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddBlogColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.AddColumn<string>("slug", "Blog", nullable: true, schema: "dbo", type: "nvarchar(500)");
            migrationBuilder.AddColumn<string>("seoTitle", "Blog", nullable: true, schema: "dbo", type: "nvarchar(500)");
            migrationBuilder.AddColumn<string>("metaDescription", "Blog", nullable: true, schema: "dbo", type: "nvarchar(500)");

            migrationBuilder.Sql("update [UCEMEDb].[dbo].[Blog] set slug = LOWER(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(titulo, 'ñ', 'n'), '-', ''), '/', ''), ',', ''), ')', ''), '(', ''), ':', ''), 'ú', 'u'), 'ó', 'o'), 'é', 'e'), 'í', 'i'), 'á', 'a'), '?', ''), '¿', ''), ' ', ''))");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.DropColumn("slug", "dbo.Blog");
        }
    }
}
