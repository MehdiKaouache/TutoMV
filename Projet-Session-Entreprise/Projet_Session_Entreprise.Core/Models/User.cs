namespace Projet_Session_Entreprise.Core.Models
{
    // Classe de base pour Student et Tutor
    public abstract class User
    {
        public int Id { get; set; }
        public string DA { get; set; } = string.Empty;
        public string Nom { get; set; } = string.Empty;
        public string Prenom { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}