using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Projet_Session_Entreprise.Data;

#nullable disable

namespace Projet_Session_Entreprise.Migrations
{
    public partial class AjoutLogiqueTuteurFinal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DA = table.Column<string>(type: "longtext", nullable: false),
                    Password = table.Column<string>(type: "longtext", nullable: false),
                    AverageGrade = table.Column<double>(type: "double", nullable: false),
                    HasSignedEngagement = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsTutor = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Users", x => x.Id); });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Users");
        }
    }
}