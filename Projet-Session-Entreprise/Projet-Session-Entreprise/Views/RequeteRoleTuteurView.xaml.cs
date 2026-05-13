using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Projet_Session_Entreprise.Models;
using Projet_Session_Entreprise.Services;
using Projet_Session_Entreprise.Data;

namespace Projet_Session_Entreprise.Views
{
    public partial class RequeteRoleTuteurView : UserControl
    {
        private Tutor _tutor;
        private List<SlotDisplay> _tempSlots = new List<SlotDisplay>();
        private Dictionary<string, DayOfWeek> _dayMap = new Dictionary<string, DayOfWeek> {
            { "Lundi", DayOfWeek.Monday }, { "Mardi", DayOfWeek.Tuesday }, { "Mercredi", DayOfWeek.Wednesday },
            { "Jeudi", DayOfWeek.Thursday }, { "Vendredi", DayOfWeek.Friday }
        };

        public class SlotDisplay
        {
            public DayOfWeek Day { get; set; }
            public string DayDisplay { get; set; } = string.Empty;
            public TimeSpan StartTime { get; set; }
        }

        public RequeteRoleTuteurView(Tutor tutor)
        {
            InitializeComponent();
            _tutor = tutor;
        }

        private void BtnAddSlot_Click(object sender, RoutedEventArgs e)
        {
            string? dayStr = (cmbDay.SelectedItem as ComboBoxItem)?.Content?.ToString();
            string? timeStr = (cmbTime.SelectedItem as ComboBoxItem)?.Content?.ToString();

            if (dayStr != null && timeStr != null)
            {
                if (!_tempSlots.Any(s => s.DayDisplay == dayStr && s.StartTime == TimeSpan.Parse(timeStr)))
                {
                    _tempSlots.Add(new SlotDisplay
                    {
                        Day = _dayMap[dayStr],
                        DayDisplay = dayStr,
                        StartTime = TimeSpan.Parse(timeStr)
                    });
                    lstAddedSlots.ItemsSource = null;
                    lstAddedSlots.ItemsSource = _tempSlots;
                }
            }
        }

        private void BtnFinalize_Click(object sender, RoutedEventArgs e)
        {
            string course = txtCourseTargeted.Text.Trim();
            string gradeStr = txtGradeCourse.Text.Trim();
            var subjects = lstSubjects.SelectedItems.Cast<ListBoxItem>().Select(i => i.Content.ToString() ?? "").ToList();

            if (string.IsNullOrEmpty(course) || !double.TryParse(gradeStr, out double grade) || subjects.Count == 0 || _tempSlots.Count == 0)
            {
                MessageBox.Show("Veuillez remplir tous les champs et ajouter au moins une disponibilité.");
                return;
            }

            if (grade < 80)
            {
                MessageBox.Show("Une note de 80% est requise pour ce cours.");
                return;
            }

            try
            {
                using (var db = new AppDbContext())
                {
                    var tInDb = db.Tutors.FirstOrDefault(t => t.Id == _tutor.Id);
                    if (tInDb != null)
                    {
                        tInDb.Subject = string.Join(", ", subjects) + " (" + course + ")";
                        tInDb.IsValidated = true;

                        foreach (var s in _tempSlots)
                        {
                            db.TutorSlots.Add(new TutorSlot { TutorId = tInDb.Id, Day = s.Day, StartTime = s.StartTime, IsBooked = false });
                        }
                        db.SaveChanges();
                        CurrentSessionService.CurrentUser = tInDb;
                    }
                }
                MainView.Instance.NavigateTo(new HomeView());
            }
            catch (Exception ex) { MessageBox.Show("Erreur : " + ex.Message); }
        }
    }
}