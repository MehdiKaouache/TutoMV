namespace Projet_Session_Entreprise.Core.Models
{
    public class TutorSlot
    {
        public int Id { get; set; }
        public int TutorId { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsBooked { get; set; } = false;
        public string JourFrancais => Day switch
        {
            DayOfWeek.Monday => "Lundi",
            DayOfWeek.Tuesday => "Mardi",
            DayOfWeek.Wednesday => "Mercredi",
            DayOfWeek.Thursday => "Jeudi",
            DayOfWeek.Friday => "Vendredi",
            _ => Day.ToString()
        };
    }
}