using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Projet_Session_Entreprise.Data;

#nullable disable

namespace Projet_Session_Entreprise.Migrations
{
    public partial class AjoutTutorSlots : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(name: "NumberOfRatings", table: "Tutors", type: "int", nullable: false, defaultValue: 0);
            migrationBuilder.AddColumn<int>(name: "TotalRatings", table: "Tutors", type: "int", nullable: false, defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "TutorSlots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false).Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TutorId = table.Column<int>(type: "int", nullable: false),
                    Day = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    IsBooked = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TutorSlots", x => x.Id);
                    table.ForeignKey(name: "FK_TutorSlots_Tutors_TutorId", column: x => x.TutorId, principalTable: "Tutors", principalColumn: "Id", onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(name: "IX_TutorSlots_TutorId", table: "TutorSlots", column: "TutorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "TutorSlots");
        }
    }
}