using Projet_Session_Entreprise.Models;
using Xunit;

namespace Projet_Session_Entreprise.Tests.Models
{
    public class TutorTests
    {
        [Fact]
        public void Tutor_AssignationProprietes_ValeursConservees()
        {
            var tutor = new Tutor
            {
                DA = "1234567",
                Nom = "John",
                Prenom = "Tuteur",
                Subject = "Mathématiques",
                IsValidated = true
            };

            Assert.Equal("1234567", tutor.DA);
            Assert.Equal("Mathématiques", tutor.Subject);
            Assert.True(tutor.IsValidated);
        }
    }
}