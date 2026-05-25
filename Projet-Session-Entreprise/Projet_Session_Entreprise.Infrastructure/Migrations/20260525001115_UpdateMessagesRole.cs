using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projet_Session_Entreprise.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMessagesRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SenderRole",
                table: "Messages",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SenderRole",
                table: "Messages");
        }
    }
}
