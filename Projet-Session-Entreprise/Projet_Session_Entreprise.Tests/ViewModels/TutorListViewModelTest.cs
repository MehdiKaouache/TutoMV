using Moq;
using Projet_Session_Entreprise.Core.Models;
using Projet_Session_Entreprise.Core.Interfaces;
using Projet_Session_Entreprise.UI.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Projet_Session_Entreprise.Tests.ViewModels
{
    public class TutorListViewModelTests
    {
        [Fact]
        public async Task ApplyFilters_TexteRecherche_AppelleRepositoryEtRemplitResultats()
        {
            var mockRepo = new Mock<ITutorRepository>();
            var tutors = new List<Tutor> { new Tutor { Nom = "John", Prenom = "Doe", Subject = "Maths" } };
            mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(tutors);

            var vm = new TutorListViewModel(null, mockRepo.Object);

            await Task.Delay(100);

            vm.SearchText = "John";
            vm.SelectedSubject = "Toutes les matières";

            vm.ApplyFilters();

            Assert.Single(vm.Results);
            Assert.Equal("John", vm.Results[0].Nom);
        }
    }
}