using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows;
using Projet_Session_Entreprise.Models;

namespace Projet_Session_Entreprise.ViewModels
{
    public partial class ReceivedRequestViewModel : ObservableObject
    {
        private readonly Tutor _tutor;

        public ObservableCollection<Appointment> Appointments { get; set; } = new();

        public ReceivedRequestViewModel(Tutor tutor)
        {
            _tutor = tutor;
            LoadAppointments();
        }

        private void LoadAppointments()
        {
            using (var db = new AppDbContext())
            {
                var list = db.Appointments
                             .Where(a => a.TutorId == _tutor.Id && a.Status == "En attente")
                             .ToList();

                Appointments.Clear();
                foreach (var a in list)
                {
                    Appointments.Add(a);
                }
            }
        }

        [RelayCommand]
        private void Accepter(Appointment appointment)
        {
            using (var db = new AppDbContext())
            {
                var a = db.Appointments.Find(appointment.Id);
                if (a != null)
                {
                    a.Status = "Accepté";
                    db.SaveChanges();
                    MessageBox.Show("Rendez-vous accepté !");
                }
            }
            LoadAppointments();
        }

        [RelayCommand]
        private void Refuser(Appointment appointment)
        {
            using (var db = new AppDbContext())
            {
                var a = db.Appointments.Find(appointment.Id);
                if (a != null)
                {
                    a.Status = "Refusé";
                    db.SaveChanges();
                    MessageBox.Show("Rendez-vous refusé.");
                }
            }
            LoadAppointments();
        }
    }
}