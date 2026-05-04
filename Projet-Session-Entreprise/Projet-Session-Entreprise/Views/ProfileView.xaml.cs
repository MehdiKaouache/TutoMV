using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Projet_Session_Entreprise.Models;
using Projet_Session_Entreprise.Services;

namespace Projet_Session_Entreprise.Views
{
    public partial class ProfileView : UserControl
    {
        private Tutor? _currentTutor;
        private Student? _currentStudent;
        private Dictionary<string, DayOfWeek> _dayMap = new Dictionary<string, DayOfWeek> {
            { "Lundi", DayOfWeek.Monday }, { "Mardi", DayOfWeek.Tuesday }, { "Mercredi", DayOfWeek.Wednesday },
            { "Jeudi", DayOfWeek.Thursday }, { "Vendredi", DayOfWeek.Friday }
        };

        public ProfileView(object user)
        {
            InitializeComponent();

            if (user is Tutor t)
            {
                _currentTutor = t;
                this.DataContext = t;
                sectionTuteur.Visibility = Visibility.Visible;
                colDispos.Width = new GridLength(350);
                LoadAppointments(t.Id, true);
                LoadTutorSlots();
            }
            else if (user is Student s)
            {
                _currentStudent = s;
                this.DataContext = s;
                sectionTuteur.Visibility = Visibility.Collapsed;
                colDispos.Width = new GridLength(0);
                LoadAppointments(s.Id, false);
            }
        }

        private void LoadAppointments(int userId, bool isTutor)
        {
            using (var db = new AppDbContext())
            {
                if (isTutor)
                    dgAppointments.ItemsSource = db.Appointments.Where(a => a.TutorId == userId).OrderByDescending(a => a.DateRDV).ToList();
                else
                    dgAppointments.ItemsSource = db.Appointments.Where(a => a.StudentId == userId).OrderByDescending(a => a.DateRDV).ToList();
            }
        }

        private void LoadTutorSlots()
        {
            if (_currentTutor == null) return;
            using (var db = new AppDbContext())
            {
                lstCurrentSlots.ItemsSource = db.TutorSlots.Where(s => s.TutorId == _currentTutor.Id).ToList();
            }
        }

        private void BtnShowAdd_Click(object sender, RoutedEventArgs e)
        {
            lblSlotError.Visibility = Visibility.Collapsed;
            AjouterSlotArea.Visibility = Visibility.Visible;
            btnShowAdd.Visibility = Visibility.Collapsed;
        }

        private void BtnCancelAdd_Click(object sender, RoutedEventArgs e)
        {
            lblSlotError.Visibility = Visibility.Collapsed;
            AjouterSlotArea.Visibility = Visibility.Collapsed;
            btnShowAdd.Visibility = Visibility.Visible;
        }

        private void BtnSaveNewSlot_Click(object sender, RoutedEventArgs e)
        {
            if (_currentTutor == null) return;

            string dayStr = (cmbDay.SelectedItem as ComboBoxItem)?.Content.ToString()!;
            string startStr = (cmbTime.SelectedItem as ComboBoxItem)?.Content.ToString()!;
            string endStr = (cmbEndTime.SelectedItem as ComboBoxItem)?.Content.ToString()!;

            var day = _dayMap[dayStr];
            var start = TimeSpan.Parse(startStr);
            var end = TimeSpan.Parse(endStr);

            if (end <= start)
            {
                lblSlotError.Text = "L'heure de fin doit être après l'heure de début.";
                lblSlotError.Visibility = Visibility.Visible;
                return;
            }

            using (var db = new AppDbContext())
            {
                bool existe = db.TutorSlots.Any(s => s.TutorId == _currentTutor.Id && s.Day == day && s.StartTime == start);
                if (existe)
                {
                    lblSlotError.Text = "Ce créneau existe déjà.";
                    lblSlotError.Visibility = Visibility.Visible;
                    return;
                }

                db.TutorSlots.Add(new TutorSlot
                {
                    TutorId = _currentTutor.Id,
                    Day = day,
                    StartTime = start,
                    EndTime = end,
                    IsBooked = false
                });
                db.SaveChanges();
            }

            lblSlotError.Visibility = Visibility.Collapsed;
            AjouterSlotArea.Visibility = Visibility.Collapsed;
            btnShowAdd.Visibility = Visibility.Visible;
            LoadTutorSlots();
        }

        private void BtnDeleteSlot_Click(object sender, RoutedEventArgs e)
        {
            var slot = (sender as Button)?.DataContext as TutorSlot;
            if (slot == null) return;
            using (var db = new AppDbContext())
            {
                var s = db.TutorSlots.Find(slot.Id);
                if (s != null) { db.TutorSlots.Remove(s); db.SaveChanges(); }
            }
            LoadTutorSlots();
        }
    }
}