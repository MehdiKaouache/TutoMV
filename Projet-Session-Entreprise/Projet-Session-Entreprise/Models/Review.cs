namespace Projet_Session_Entreprise.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string NomEleve { get; set; } = "";
        public string Commentaire { get; set; } = "";
        public int Note { get; set; }
        public int TutorId { get; set; }
        public Tutor? Tutor { get; set; }

        public string EtoilesString => new string('★', Note) + new string('☆', 5 - Note);
    }
}