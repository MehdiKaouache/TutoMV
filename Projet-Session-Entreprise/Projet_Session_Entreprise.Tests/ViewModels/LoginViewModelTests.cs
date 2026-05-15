using Moq;
using Projet_Session_Entreprise.Models;
using Projet_Session_Entreprise.Services;
using Projet_Session_Entreprise.Services.Interfaces;
using Projet_Session_Entreprise.ViewModels;
using System.Threading.Tasks;
using Xunit;

namespace Projet_Session_Entreprise.Tests.ViewModels
{
    public class LoginViewModelTests
    {
        [Fact]
        public async Task SendRequestAsync_ChampsVides_MessageErreur()
        {
            var mockAuth = new Mock<IAuthService>();
            var vm = new LoginViewModel(mockAuth.Object);
            await vm.SendRequestAsync();
            Assert.Equal("Veuillez remplir tous les champs.", vm.StatusMessage);
        }
    }
}