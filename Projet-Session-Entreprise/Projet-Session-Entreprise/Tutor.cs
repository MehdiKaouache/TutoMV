namespace Projet_Session_Entreprise
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
    }
}