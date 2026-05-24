using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Projet_Session_Entreprise.Core.Models;
using Projet_Session_Entreprise.Infrastructure.Services;
using Projet_Session_Entreprise.Infrastructure.Data;

namespace Projet_Session_Entreprise.UI.Views
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
            public TimeSpan EndTime { get; set; }
        }

        public RequeteRoleTuteurView(Tutor tutor)
        {
            InitializeComponent();
            _tutor = tutor;
        }

        private void ShowError(string message)
        {
            txtError.Text = message;
            txtError.Visibility = Visibility.Visible;
        }

        private void BtnShowAdd_Click(object sender, RoutedEventArgs e)
        {
            txtError.Visibility = Visibility.Collapsed;
            lblSlotError.Visibility = Visibility.Collapsed;
            AjouterSlotArea.Visibility = Visibility.Visible;
            btnShowAdd.Visibility = Visibility.Collapsed;
        }

        private void BtnCancelAdd_Click(object sender, RoutedEventArgs e)
        {
            txtError.Visibility = Visibility.Collapsed;
            lblSlotError.Visibility = Visibility.Collapsed;
            AjouterSlotArea.Visibility = Visibility.Collapsed;
            btnShowAdd.Visibility = Visibility.Visible;
        }

        private void BtnAddSlot_Click(object sender, RoutedEventArgs e)
        {
            txtError.Visibility = Visibility.Collapsed;
            lblSlotError.Visibility = Visibility.Collapsed;

            string? dayStr = (cmbDay.SelectedItem as ComboBoxItem)?.Content?.ToString();
            string? startStr = (cmbTime.SelectedItem as ComboBoxItem)?.Content?.ToString();
            string? endStr = (cmbEndTime.SelectedItem as ComboBoxItem)?.Content?.ToString();

            if (dayStr != null && startStr != null && endStr != null)
            {
                var start = TimeSpan.Parse(startStr);
                var end = TimeSpan.Parse(endStr);

                if (end <= start)
                {
                    lblSlotError.Text = "L'heure de fin doit être après le début.";
                    lblSlotError.Visibility = Visibility.Visible;
                    return;
                }

                if (!_tempSlots.Any(s => s.DayDisplay == dayStr && s.StartTime == start && s.EndTime == end))
                {
                    _tempSlots.Add(new SlotDisplay
                    {
                        Day = _dayMap[dayStr],
                        DayDisplay = dayStr,
                        StartTime = start,
                        EndTime = end
                    });

                    lstAddedSlots.ItemsSource = null;
                    lstAddedSlots.ItemsSource = _tempSlots;

                    AjouterSlotArea.Visibility = Visibility.Collapsed;
                    btnShowAdd.Visibility = Visibility.Visible;
                }
            }
        }

        private void BtnDeleteSlot_Click(object sender, RoutedEventArgs e)
        {
            var slot = (sender as Button)?.DataContext as SlotDisplay;
            if (slot != null)
            {
                _tempSlots.Remove(slot);
                lstAddedSlots.ItemsSource = null;
                lstAddedSlots.ItemsSource = _tempSlots;
            }
        }

        private void BtnFinalize_Click(object sender, RoutedEventArgs e)
        {
            txtError.Visibility = Visibility.Collapsed;
            string course = txtCourseTargeted.Text.Trim();
            string gradeStr = txtGradeCourse.Text.Trim();
            var subjects = lstSubjects.SelectedItems.Cast<ListBoxItem>().Select(i => i.Content.ToString() ?? "").ToList();

            if (string.IsNullOrEmpty(course) || !double.TryParse(gradeStr, out double grade) || subjects.Count == 0 || _tempSlots.Count == 0)
            {
                ShowError("Veuillez remplir tous les champs et ajouter au moins une disponibilité.");
                return;
            }

            if (grade < 80)
            {
                ShowError("Une note de 80% minimum est requise pour ce cours.");
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
                            db.TutorSlots.Add(new TutorSlot { TutorId = tInDb.Id, Day = s.Day, StartTime = s.StartTime, EndTime = s.EndTime, IsBooked = false });
                        }
                        db.SaveChanges();
                        CurrentSessionService.CurrentUser = tInDb;
                    }
                }
                MainView.Instance.NavigateTo(new HomeView());
            }
            catch (Exception ex)
            {
                ShowError("Erreur lors de l'enregistrement : " + ex.Message);
            }
        }
    }
}