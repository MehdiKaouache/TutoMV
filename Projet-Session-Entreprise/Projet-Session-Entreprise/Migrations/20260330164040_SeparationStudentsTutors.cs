using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Projet_Session_Entreprise.Data;

#nullable disable

namespace Projet_Session_Entreprise.Migrations
{
    public partial class SeparationStudentsTutors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Teachers");
            migrationBuilder.DropTable(name: "Users");

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false).Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DA = table.Column<string>(type: "longtext", nullable: false),
                    Nom = table.Column<string>(type: "longtext", nullable: false),
                    Prenom = table.Column<string>(type: "longtext", nullable: false),
                    Password = table.Column<string>(type: "longtext", nullable: false),
                    AverageGrade = table.Column<double>(type: "double", nullable: false),
                    Role = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Students", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "Tutors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false).Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nom = table.Column<string>(type: "longtext", nullable: false),
                    Prenom = table.Column<string>(type: "longtext", nullable: false),
                    DA = table.Column<string>(type: "longtext", nullable: false),
                    Password = table.Column<string>(type: "longtext", nullable: false),
                    Subject = table.Column<string>(type: "longtext", nullable: false),
                    Availability = table.Column<string>(type: "longtext", nullable: false),
                    AverageGrade = table.Column<double>(type: "double", nullable: false),
                    IsValidated = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Role = table.Column<string>(type: "longtext", nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Tutors", x => x.Id); });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Students");
            migrationBuilder.DropTable(name: "Tutors");
        }
    }
}