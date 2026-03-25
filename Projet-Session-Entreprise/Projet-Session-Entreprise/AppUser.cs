namespace Projet_Session_Entreprise
{
    public class AppUser
    {
        public int Id { get; set; }

        public string DA { get; set; } = "";

        public string Password { get; set; } = "";

        public double AverageGrade { get; set; }
        
        public bool HasSignedEngagement { get; set; }
        
        public bool IsTutor { get; set; }
    }
}