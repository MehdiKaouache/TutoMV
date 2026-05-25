using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Projet_Session_Entreprise.Core.Models;
using Projet_Session_Entreprise.Infrastructure.Data;

namespace Projet_Session_Entreprise.UI.Views
{
    public partial class BookingView : UserControl
    {
        private Student _student;
        private Tutor _tutor;
        private TutorSlot? _pendingSlot;
        private bool _navigateHomeOnOk;

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
                ShowModal("Erreur BDD", ex.InnerException?.Message ?? ex.Message, false);
            }
        }

        private void Slot_Click(object sender, RoutedEventArgs e)
        {
            _pendingSlot = (sender as Button)?.DataContext as TutorSlot;
            if (_pendingSlot == null) return;

            string jourFr = TraduireJour(_pendingSlot.Day);
            ShowModal("Confirmation requise", $"Confirmer la réservation pour le prochain {jourFr} à {_pendingSlot.StartTime:hh\\:mm} ?", true);
        }

        private void ConfirmBooking()
        {
            if (_pendingSlot == null) return;

            try
            {
                using (var db = new AppDbContext())
                {
                    DateTime nextDate = GetNextDateForDay(_pendingSlot.Day);
                    DateTime fullDate = nextDate.Date.Add(_pendingSlot.StartTime);

                    db.Appointments.Add(new Appointment
                    {
                        StudentId = _student.Id,
                        TutorId = _tutor.Id,
                        DateRDV = fullDate,
                        Status = "En attente"
                    });

                    var slotInDb = db.TutorSlots.First(s => s.Id == _pendingSlot.Id);
                    slotInDb.IsBooked = true;

                    db.SaveChanges();
                }

                _navigateHomeOnOk = true;
                ShowModal("Succès", "Demande de réservation envoyée ! Le tuteur devra confirmer votre créneau.", false);
            }
            catch (Exception ex)
            {
                ShowModal("Erreur", "Erreur lors de la réservation : " + (ex.InnerException?.Message ?? ex.Message), false);
            }
        }

        private DateTime GetNextDateForDay(DayOfWeek day)
        {
            DateTime start = DateTime.Now.AddDays(1);
            int daysUntil = ((int)day - (int)start.DayOfWeek + 7) % 7;
            return start.AddDays(daysUntil);
        }

        private string TraduireJour(DayOfWeek day)
        {
            return day switch
            {
                DayOfWeek.Monday => "lundi",
                DayOfWeek.Tuesday => "mardi",
                DayOfWeek.Wednesday => "mercredi",
                DayOfWeek.Thursday => "jeudi",
                DayOfWeek.Friday => "vendredi",
                DayOfWeek.Saturday => "samedi",
                DayOfWeek.Sunday => "dimanche",
                _ => day.ToString()
            };
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) => MainView.Instance.NavigateTo(new TutorListView());

        private void ShowModal(string title, string message, bool isConfirmation)
        {
            txtModalTitle.Text = title;
            txtModalMessage.Text = message;

            if (isConfirmation)
            {
                ModalActionButtons.Visibility = Visibility.Visible;
                ModalOkButton.Visibility = Visibility.Collapsed;
            }
            else
            {
                ModalActionButtons.Visibility = Visibility.Collapsed;
                ModalOkButton.Visibility = Visibility.Visible;
            }

            ModalOverlay.Visibility = Visibility.Visible;
        }

        private void ModalConfirm_Click(object sender, RoutedEventArgs e)
        {
            ModalOverlay.Visibility = Visibility.Collapsed;
            ConfirmBooking();
        }

        private void ModalCancel_Click(object sender, RoutedEventArgs e)
        {
            _pendingSlot = null;
            ModalOverlay.Visibility = Visibility.Collapsed;
        }

        private void ModalOk_Click(object sender, RoutedEventArgs e)
        {
            ModalOverlay.Visibility = Visibility.Collapsed;
            if (_navigateHomeOnOk)
            {
                MainView.Instance.NavigateTo(new HomeView());
            }
        }
    }
}