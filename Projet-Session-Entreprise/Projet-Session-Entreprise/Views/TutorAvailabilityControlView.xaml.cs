using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Projet_Session_Entreprise.Models;
using Projet_Session_Entreprise.Services;

namespace Projet_Session_Entreprise.Views
{
    public partial class TutorAvailabilityControlView : UserControl
    {
        private Tutor? _currentTutor;
        private Dictionary<string, DayOfWeek> _dayMap = new Dictionary<string, DayOfWeek> {
            { "Lundi", DayOfWeek.Monday }, 
            { "Mardi", DayOfWeek.Tuesday }, 
            { "Mercredi", DayOfWeek.Wednesday },
            { "Jeudi", DayOfWeek.Thursday }, 
            { "Vendredi", DayOfWeek.Friday }
        };

        public TutorAvailabilityControlView()
        {
            InitializeComponent();
        }

        public void Initialize(Tutor tutor)
        {
            _currentTutor = tutor;
            LoadTutorSlots();
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
            AjouterSlotArea.Visibility = Visibility.Collapsed;
            btnShowAdd.Visibility = Visibility.Visible;
        }

        private void BtnSaveNewSlot_Click(object sender, RoutedEventArgs e)
        {
            if (_currentTutor == null) return;
            try
            {
                string dayStr = (cmbDay.SelectedItem as ComboBoxItem)?.Content.ToString()!;
                string startStr = (cmbTime.SelectedItem as ComboBoxItem)?.Content.ToString()!;
                string endStr = (cmbEndTime.SelectedItem as ComboBoxItem)?.Content.ToString()!;

                var day = _dayMap[dayStr];
                var start = TimeSpan.Parse(startStr);
                var end = TimeSpan.Parse(endStr);

                if (end <= start)
                {
                    lblSlotError.Text = "L'heure de fin doit être après le début.";
                    lblSlotError.Visibility = Visibility.Visible;
                    return;
                }

                using (var db = new AppDbContext())
                {
                    db.TutorSlots.Add(new TutorSlot { TutorId = _currentTutor.Id, Day = day, StartTime = start, EndTime = end, IsBooked = false });
                    db.SaveChanges();
                }
                AjouterSlotArea.Visibility = Visibility.Collapsed;
                btnShowAdd.Visibility = Visibility.Visible;
                LoadTutorSlots();
            }
            catch (Exception ex) { MessageBox.Show("Erreur : " + ex.Message); }
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