using Microsoft.EntityFrameworkCore;
using Projet_Session_Entreprise.Infrastructure.Data;
using Projet_Session_Entreprise.Core.Models;
using Projet_Session_Entreprise.Infrastructure.Repositories;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Projet_Session_Entreprise.Tests.Repositories
{
    public class TutorRepositoryTests
    {
        private AppDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task SearchTutorsAsync_Resultats_RetourneListe()
        {
            using var context = CreateContext();
            var repo = new TutorRepository(context);
            context.Tutors.Add(new Tutor { Nom = "John", DA = "1", Password = "1", Availability = "", Role = "T" });
            await context.SaveChangesAsync();

            var result = await repo.SearchTutorsAsync("Momo");
            Assert.Single(result);
        }
    }
}