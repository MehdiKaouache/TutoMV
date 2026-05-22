using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Projet_Session_Entreprise.Data;

#nullable disable

namespace Projet_Session_Entreprise.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Projet_Session_Entreprise.Models.Student", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                b.Property<double>("AverageGrade").HasColumnType("double");
                b.Property<string>("DA").IsRequired().HasColumnType("longtext");
                b.Property<string>("Nom").IsRequired().HasColumnType("longtext");
                b.Property<string>("Password").IsRequired().HasColumnType("longtext");
                b.Property<string>("Prenom").IsRequired().HasColumnType("longtext");
                b.Property<string>("Role").IsRequired().HasColumnType("longtext");

                b.HasKey("Id");
                b.ToTable("Students");
            });

            modelBuilder.Entity("Projet_Session_Entreprise.Models.Tutor", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                b.Property<string>("Availability").IsRequired().HasColumnType("longtext");
                b.Property<double>("AverageGrade").HasColumnType("double");
                b.Property<string>("DA").IsRequired().HasColumnType("longtext");
                b.Property<bool>("IsValidated").HasColumnType("tinyint(1)");
                b.Property<string>("Nom").IsRequired().HasColumnType("longtext");
                b.Property<int>("NumberOfRatings").HasColumnType("int");
                b.Property<string>("Password").IsRequired().HasColumnType("longtext");
                b.Property<string>("Prenom").IsRequired().HasColumnType("longtext");
                b.Property<string>("Role").IsRequired().HasColumnType("longtext");
                b.Property<string>("Subject").IsRequired().HasColumnType("longtext");
                b.Property<int>("TotalRatings").HasColumnType("int");

                b.HasKey("Id");
                b.ToTable("Tutors");
            });

            modelBuilder.Entity("Projet_Session_Entreprise.Models.TutorSlot", b =>
            {
                b.Property<int>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("int");

                MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("Id"));

                b.Property<int>("Day").HasColumnType("int");
                b.Property<TimeSpan>("EndTime").HasColumnType("time(6)");
                b.Property<bool>("IsBooked").HasColumnType("tinyint(1)");
                b.Property<TimeSpan>("StartTime").HasColumnType("time(6)");
                b.Property<int>("TutorId").HasColumnType("int");

                b.HasKey("Id");
                b.HasIndex("TutorId");
                b.ToTable("TutorSlots");
            });

            modelBuilder.Entity("Projet_Session_Entreprise.Models.TutorSlot", b =>
            {
                b.HasOne("Projet_Session_Entreprise.Models.Tutor", null)
                    .WithMany()
                    .HasForeignKey("TutorId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });
#pragma warning restore 612, 618
        }
    }
}