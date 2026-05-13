using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Projet_Session_Entreprise.Models;
using Projet_Session_Entreprise.Data;

namespace Projet_Session_Entreprise.Views
{
    public partial class BookingView : UserControl
    {
        private Student _student;
        private Tutor _tutor;

        public BookingView(Student student, Tutor tutor)
        {
            InitializeComponent();
            _student = student;
            _tutor = tutor;

            if (tutor != null && txtTutorName != null)
                txtTutorName.Text = $"Réserver avec {tutor.Prenom}";

            LoadAvailableSlots();
        }

        private void LoadAvailableSlots()
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    var slots = db.TutorSlots
                        .Where(s => s.TutorId == _tutor.Id && !s.IsBooked)
                        .ToList();
                    icSlots.ItemsSource = slots;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur BDD : " + ex.Message);
            }
        }

        private void Slot_Click(object sender, RoutedEventArgs e)
        {
            var slot = (sender as Button)?.DataContext as TutorSlot;
            if (slot == null) return;

            var result = MessageBox.Show($"Confirmer la réservation pour le prochain {slot.Day} à {slot.StartTime} ?", "Confirmation", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                ConfirmBooking(slot);
            }
        }

        private void ConfirmBooking(TutorSlot slot)
        {
            try
            {
                using (var db = new AppDbContext())
                {
                    DateTime nextDate = GetNextDateForDay(slot.Day);
                    DateTime fullDate = nextDate.Date.Add(slot.StartTime);

                    db.Appointments.Add(new Appointment
                    {
                        StudentId = _student.Id,
                        TutorId = _tutor.Id,
                        DateRDV = fullDate,
                        Status = "En attente"
                    });

                    var slotInDb = db.TutorSlots.First(s => s.Id == slot.Id);
                    slotInDb.IsBooked = true;

                    db.SaveChanges();
                }

                MessageBox.Show("Demande de réservation envoyée !");
                MainView.Instance.NavigateTo(new HomeView());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la réservation : " + ex.Message);
            }
        }

        private DateTime GetNextDateForDay(DayOfWeek day)
        {
            DateTime start = DateTime.Now.AddDays(1);
            int daysUntil = ((int)day - (int)start.DayOfWeek + 7) % 7;
            return start.AddDays(daysUntil);
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => MainView.Instance.NavigateTo(new TutorListView());
    }
}