namespace Projet_Session_Entreprise
{
    public class Avis
    {
        public int Id { get; set; }
        public string NomEleve { get; set; }
        public int Note { get; set; }
        public string Commentaire { get; set; }
        public string EtoilesString => new string('★', Note) + new string('☆', 5 - Note);
    }
}
