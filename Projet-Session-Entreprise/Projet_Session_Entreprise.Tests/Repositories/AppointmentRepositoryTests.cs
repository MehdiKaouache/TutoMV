using Microsoft.EntityFrameworkCore;
using Projet_Session_Entreprise.Data;
using Projet_Session_Entreprise.Models;
using Projet_Session_Entreprise.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Projet_Session_Entreprise.Tests.Repositories
{
    public class AppointmentRepositoryTests
    {
        private AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task GetByStudentIdAsync_FiltreParEtudiant_RetourneListe()
        {
            using var context = CreateContext();
            var repo = new AppointmentRepository(context);
            context.Appointments.Add(new Appointment { StudentId = 1, TutorId = 2, DateRDV = DateTime.Now });
            context.Appointments.Add(new Appointment { StudentId = 99, TutorId = 2, DateRDV = DateTime.Now });
            await context.SaveChangesAsync();

            var results = await repo.GetByStudentIdAsync(1);

            Assert.Single(results);
            Assert.Equal(1, results.First().StudentId);
        }
    }
}