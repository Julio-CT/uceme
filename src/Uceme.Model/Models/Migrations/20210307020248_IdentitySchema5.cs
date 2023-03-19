namespace Uceme.UI.Data.Migrations
{
    using System;
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class IdentitySchema5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.AddColumn<string>("SessionId", "DeviceCodes", nullable: true, schema: "dbo", type: "nvarchar(200)");
            migrationBuilder.AddColumn<string>("Description", "DeviceCodes", nullable: true, schema: "dbo", type: "nvarchar(200)");

            migrationBuilder.AddColumn<string>("SessionId", "PersistedGrants", nullable: true, schema: "dbo", type: "nvarchar(200)");
            migrationBuilder.AddColumn<string>("Description", "PersistedGrants", nullable: true, schema: "dbo", type: "nvarchar(200)");
            migrationBuilder.AddColumn<string>("ConsumedTime", "PersistedGrants", nullable: true, schema: "dbo", type: "datetime2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            if (migrationBuilder is null)
            {
                throw new ArgumentNullException(nameof(migrationBuilder));
            }

            migrationBuilder.DropColumn("SessionId", "dbo.DeviceCodes");
            migrationBuilder.DropColumn("Description", "dbo.DeviceCodes");

            migrationBuilder.DropColumn("SessionId", "dbo.PersistedGrants");
            migrationBuilder.DropColumn("Description", "dbo.PersistedGrants");
            migrationBuilder.DropColumn("ConsumedTime", "dbo.PersistedGrants");
        }
    }
}
