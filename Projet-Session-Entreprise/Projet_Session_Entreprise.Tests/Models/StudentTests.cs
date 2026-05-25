using Projet_Session_Entreprise.Core.Models;
using Xunit;

namespace Projet_Session_Entreprise.Tests.Models
{
    public class StudentTests
    {
        [Fact]
        public void Student_AssignationProprietes_ValeursConservees()
        {
            var student = new Student
            {
                DA = "2000100",
                Nom = "Doe",
                Prenom = "John",
                Role = "Étudiant"
            };

            Assert.Equal("2000100", student.DA);
            Assert.Equal("Doe", student.Nom);
            Assert.Equal("John", student.Prenom);
            Assert.Equal("Étudiant", student.Role);
        }

        [Fact]
        public void Student_Initialisation_DoitAvoirUnDAVideParDefaut()
        {
            var student = new Student();
            Assert.True(string.IsNullOrEmpty(student.DA));
        }
    }
}