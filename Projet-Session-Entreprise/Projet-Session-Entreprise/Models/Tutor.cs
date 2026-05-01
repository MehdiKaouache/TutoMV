using System;
using System.Collections.Generic;

namespace Projet_Session_Entreprise.Models
{
    public class Tutor
    {
        public int Id { get; set; }
        public string Nom { get; set; } = "";
        public string Prenom { get; set; } = "";
        public string DA { get; set; } = "";
        public string Password { get; set; } = "";
        public string Subject { get; set; } = "";
        public string Availability { get; set; } = "";
        public double AverageGrade { get; set; } = 85.0;
        public bool IsValidated { get; set; } = false;
        public string Role { get; set; } = "Enseignant";
        public List<Review> Reviews { get; set; } = new List<Review>();
        public int NumberOfRatings { get; set; } = 0;
        public int TotalRatings { get; set; } = 0;

        public double AverageRating
        {
            get
            {
                if (NumberOfRatings == 0) return 0.0;
                return Math.Round((double)TotalRatings / NumberOfRatings, 1);
            }
        }
    }
}