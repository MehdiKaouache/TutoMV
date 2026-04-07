namespace Projet_Session_Entreprise
{
    public class Review
    {
        public int Id { get; set; }
        public string Commentaire { get; set; } = "";
        public int Note { get; set; }

        public int TutorId { get; set; }
        public Tutor Tutor { get; set; }
    }
}