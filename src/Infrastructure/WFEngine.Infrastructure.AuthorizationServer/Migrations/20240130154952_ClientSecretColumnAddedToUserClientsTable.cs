using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WFEngine.Infrastructure.AuthorizationServer.Migrations
{
    /// <inheritdoc />
    public partial class ClientSecretColumnAddedToUserClientsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientSecret",
                table: "UserClients",
                type: "varchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientSecret",
                table: "UserClients");
        }
    }
}
