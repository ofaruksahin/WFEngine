using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WFEngine.Infrastructure.AuthorizationServer.Migrations.AuthorizationPersistedGrantDb
{
    /// <inheritdoc />
    public partial class IsMasterColumnAddToTenantUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMaster",
                table: "TenantUsers",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMaster",
                table: "TenantUsers");
        }
    }
}
