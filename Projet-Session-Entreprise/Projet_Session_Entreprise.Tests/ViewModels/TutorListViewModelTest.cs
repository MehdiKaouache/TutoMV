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
        public async Task RechercherAsync_TexteRecherche_AppelleRepositoryEtRemplitResultats()
        {
            var mockRepo = new Mock<ITutorRepository>();
            var tutors = new List<Tutor> { new Tutor { Nom = "John" } };
            mockRepo.Setup(r => r.SearchTutorsAsync("John")).ReturnsAsync(tutors);
            var vm = new TutorListViewModel(null, mockRepo.Object);
            vm.SearchText = "John";
            vm.SelectedFilter = "Nom";

            await vm.RechercherAsync();

            Assert.Single(vm.Results);
            Assert.Equal("John", vm.Results[0].Nom);
            mockRepo.Verify(r => r.SearchTutorsAsync("John"), Times.Once);
        }
    }
}