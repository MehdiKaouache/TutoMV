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
                DA = "2387924",
                Nom = "Kaouache",
                Prenom = "Mehdi",
                Role = "Étudiant"
            };

            Assert.Equal("2387924", student.DA);
            Assert.Equal("Kaouache", student.Nom);
            Assert.Equal("Mehdi", student.Prenom);
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