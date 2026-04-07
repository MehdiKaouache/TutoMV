namespace Projet_Session_Entreprise
{
    public class Student
    {
        public int Id { get; set; }
        public string DA { get; set; } = "";
        public string Nom { get; set; } = "";
        public string Prenom { get; set; } = "";
        public string Password { get; set; } = "";
        public double AverageGrade { get; set; }
        public string Role { get; set; } = "Étudiant";
    }
}