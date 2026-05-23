using Projet_Session_Entreprise.Models;
using Projet_Session_Entreprise.ViewModels;
using Xunit;

namespace Projet_Session_Entreprise.Tests.ViewModels
{
    public class ProfileViewModelTests
    {
        [Fact]
        public void LoadData_ChargeRDV()
        {
            var student = new Student { Id = 1, Nom = "John" };

            var vm = new ProfileViewModel(student);

            vm.LoadData();

            Assert.NotNull(vm.MyAppointments);
        }
    }
}