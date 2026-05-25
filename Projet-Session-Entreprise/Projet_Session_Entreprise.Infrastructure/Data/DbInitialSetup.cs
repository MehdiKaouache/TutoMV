using Microsoft.EntityFrameworkCore;
using Projet_Session_Entreprise.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.Infrastructure.Data
{
    static class DbInitialSetup
    {
        public static void SetupDonnees(AppDbContext context)
        {
            if (context.Tutors.Any()) return; 
            var tuteursInit = Enumerable.Range(1, 1000).Select(i => new Tutor
            {
                Nom = $"Tuteur{i}",
                Prenom = $"John {i}",
                DA = $"{1230560 + i}",
                Password = "password123",
                Subject = i % 3 == 0 ? "Mathématiques" : i % 3 == 1 ? "Informatique" : "Francais",
                Availability = "Lundi 9h-12h",
                AverageGrade = 85.0 + (i % 15),
                IsValidated = true,
                Role = "Tuteur",
                NumberOfRatings = 0,
                TotalRatings = 0
            }).ToList();
            context.Tutors.AddRange(tuteursInit);
            context.SaveChanges();
        }
    }
}
