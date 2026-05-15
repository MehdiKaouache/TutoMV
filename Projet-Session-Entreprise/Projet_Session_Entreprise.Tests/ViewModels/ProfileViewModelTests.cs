using Moq;
using Projet_Session_Entreprise.Models;
using Projet_Session_Entreprise.Repositories.Interfaces;
using Projet_Session_Entreprise.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Projet_Session_Entreprise.Tests.ViewModels
{
    public class ProfileViewModelTests
    {
        [Fact]
        public async Task LoadDataAsync_ChargeRDV()
        {
            var mockRepo = new Mock<IAppointmentRepository>();
            var mockTutorRepo = new Mock<ITutorRepository>();
            var mockReviewRepo = new Mock<IReviewRepository>();

            var student = new Student { Id = 1, Nom = "John" };

            mockRepo.Setup(r => r.GetByStudentIdAsync(1))
                    .ReturnsAsync(new List<Appointment> { new Appointment { Id = 1 } });

            var vm = new ProfileViewModel(student, mockRepo.Object, mockTutorRepo.Object, mockReviewRepo.Object);

            await vm.LoadDataAsync();

            Assert.Single(vm.MyAppointments);
        }
    }
}