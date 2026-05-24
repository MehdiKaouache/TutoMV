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

        [Fact]
        public async Task UpdateAppointmentStatusCommand_AccepteRDV_MetAJourStatut()
        {
            var mockRepo = new Mock<IAppointmentRepository>();
            var mockTutorRepo = new Mock<ITutorRepository>();
            var mockReviewRepo = new Mock<IReviewRepository>();

            var tutor = new Tutor { Id = 1, Nom = "John" };
            var appointment = new Appointment { Id = 10, TutorId = tutor.Id, Status = "En attente" };

            mockRepo.Setup(r => r.GetByTutorIdAsync(tutor.Id)).ReturnsAsync(new List<Appointment>());
            mockReviewRepo.Setup(r => r.GetByTutorIdAsync(tutor.Id)).ReturnsAsync(new List<Review>());

            var vm = new ProfileViewModel(tutor, mockRepo.Object, mockTutorRepo.Object, mockReviewRepo.Object);

            await vm.UpdateAppointmentStatusCommand.ExecuteAsync(new AppointmentStatusChange(appointment, "Accepté"));

            Assert.Equal("Accepté", appointment.Status);
            mockRepo.Verify(r => r.Update(appointment), Times.Once);
            mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAppointmentStatusCommand_RefuseRDV_MetAJourStatut()
        {
            var mockRepo = new Mock<IAppointmentRepository>();
            var mockTutorRepo = new Mock<ITutorRepository>();
            var mockReviewRepo = new Mock<IReviewRepository>();

            var tutor = new Tutor { Id = 1, Nom = "John" };
            var appointment = new Appointment { Id = 10, TutorId = tutor.Id, Status = "En attente" };

            mockRepo.Setup(r => r.GetByTutorIdAsync(tutor.Id)).ReturnsAsync(new List<Appointment>());
            mockReviewRepo.Setup(r => r.GetByTutorIdAsync(tutor.Id)).ReturnsAsync(new List<Review>());

            var vm = new ProfileViewModel(tutor, mockRepo.Object, mockTutorRepo.Object, mockReviewRepo.Object);

            await vm.UpdateAppointmentStatusCommand.ExecuteAsync(new AppointmentStatusChange(appointment, "Refusé"));

            Assert.Equal("Refusé", appointment.Status);
            mockRepo.Verify(r => r.Update(appointment), Times.Once);
            mockRepo.Verify(r => r.SaveChangesAsync(), Times.Once);
        }
    }
}
