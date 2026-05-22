using Microsoft.EntityFrameworkCore;
using Projet_Session_Entreprise.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.Data
{
    static class DbInitialSetup
    {
        public static void SetupDonnees(AppDbContext context)
        {
            if (context.Tutors.Any()) return; //si l'app à déjà des tuteurs, va pas en ajouter
            var tuteursInit = Enumerable.Range(1, 1000).Select(i => new Tutor
            {
                Nom = $"Tuteur{i}",
                Prenom = $"John {i}",
                DA = $"{1230560 + i}",
                Password = "password123",
                //va check si i à un reste de 1-3, si le reste = 0 -> Maths, = 1 -> Info, = 2 -> Francais
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
