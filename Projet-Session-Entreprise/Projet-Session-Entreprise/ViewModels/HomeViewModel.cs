using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Projet_Session_Entreprise.Infrastructure.Data;
using Projet_Session_Entreprise.Core.Models;
using Projet_Session_Entreprise.Infrastructure.Services;
using Projet_Session_Entreprise.UI.Views;
using System;
using System.Linq;
using System.Collections.ObjectModel;

namespace Projet_Session_Entreprise.UI.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        [ObservableProperty] private bool _isGuest;
        [ObservableProperty] private bool _isStudent;
        [ObservableProperty] private bool _isTutor;

        [ObservableProperty] private string _studentTotalSeances = "0";
        [ObservableProperty] private string _studentHeures = "0h";
        [ObservableProperty] private string _studentMoyenne = "-";
        [ObservableProperty] private string _studentName = "";

        [ObservableProperty] private string _tutorEtudiants = "0";
        [ObservableProperty] private string _tutorHeures = "0h";
        [ObservableProperty] private string _tutorNote = "-/5";
        [ObservableProperty] private string _tutorName = "";

        [ObservableProperty] private bool _hasNotifications;
        [ObservableProperty] private string _notificationText = "";

        public ObservableCollection<Appointment> UpcomingAppointments { get; set; } = new();
        public ObservableCollection<Appointment> PendingRequestsList { get; set; } = new();

        public HomeViewModel()
        {
            LoadData();
        }

        private void LoadData()
        {
            var user = CurrentSessionService.CurrentUser;

            IsGuest = user == null;
            IsTutor = user is Tutor;
            IsStudent = user is Student;

            UpcomingAppointments.Clear();
            PendingRequestsList.Clear();

            if (IsGuest || CurrentSessionService.HasSeenNotifications) return;

            using (var db = new AppDbContext())
            {
                if (IsTutor && user is Tutor t)
                {
                    TutorName = t.Prenom;
                    var appts = db.Appointments.Include(a => a.Student).Where(a => a.TutorId == t.Id).ToList();
                    var revs = db.Reviews.Where(r => r.TutorId == t.Id).ToList();

                    var completedAppts = appts.Where(a => a.Status != null && (a.Status.ToLower() == "complété" || a.Status.ToLower() == "terminé")).ToList();

                    TutorEtudiants = completedAppts.Select(a => a.StudentId).Distinct().Count().ToString();
                    TutorHeures = completedAppts.Count.ToString() + "h";
                    TutorNote = revs.Any() ? Math.Round(revs.Average(r => r.Rating), 1).ToString() + "/5" : "-/5";

                    var pending = appts.Where(a => a.Status == "En attente").OrderBy(a => a.DateRDV).ToList();
                    foreach (var p in pending) PendingRequestsList.Add(p);

                    var unreadMsgs = db.Messages.Count(m => m.ReceiverId == t.Id && m.SenderRole == "Student" && !m.IsRead && !m.IsDeleted);

                    string notifMsg = "";
                    if (unreadMsgs > 0) notifMsg += $"💬 Vous avez {unreadMsgs} message(s) non lu(s).\n";
                    if (pending.Any()) notifMsg += $"⏳ Vous avez {pending.Count} demande(s) en attente.\n";

                    if (!string.IsNullOrEmpty(notifMsg))
                    {
                        NotificationText = notifMsg.TrimEnd('\n');
                        HasNotifications = true;
                    }
                }
                else if (IsStudent && user is Student s)
                {
                    StudentName = s.Prenom;
                    StudentMoyenne = s.GPA.ToString("0.0") + " %";

                    var appts = db.Appointments.Include(a => a.Tutor).Where(a => a.StudentId == s.Id).ToList();
                    var completedAppts = appts.Where(a => a.Status != null && (a.Status.ToLower() == "complété" || a.Status.ToLower() == "terminé")).ToList();

                    StudentTotalSeances = completedAppts.Count.ToString();
                    StudentHeures = completedAppts.Count.ToString() + "h";

                    var upcoming = appts.Where(a => a.Status == "Accepté").OrderBy(a => a.DateRDV).ToList();
                    foreach (var u in upcoming) UpcomingAppointments.Add(u);

                    var unreadMsgs = db.Messages.Count(m => m.ReceiverId == s.Id && m.SenderRole == "Tutor" && !m.IsRead && !m.IsDeleted);

                    string notifMsg = "";
                    if (unreadMsgs > 0) notifMsg += $"💬 Vous avez {unreadMsgs} message(s) non lu(s).\n";
                    if (upcoming.Any()) notifMsg += $"✅ Vous avez {upcoming.Count} séance(s) prévue(s) !";

                    if (!string.IsNullOrEmpty(notifMsg))
                    {
                        NotificationText = notifMsg.TrimEnd('\n');
                        HasNotifications = true;
                    }
                }
            }
        }

        [RelayCommand]
        public void RegisterGuest() => MainView.Instance.NavigateTo(new RoleSelectionView());

        [RelayCommand]
        public void Explore() => MainView.Instance.NavigateTo(new TutorListView());

        [RelayCommand]
        public void ManageRequests()
        {
            if (CurrentSessionService.CurrentUser is Tutor t)
                MainView.Instance.NavigateTo(new ReceivedRequestsView(t));
        }

        [RelayCommand]
        public void GoToAppointments()
        {
            var user = CurrentSessionService.CurrentUser;
            if (user != null) MainView.Instance.NavigateTo(new AppointmentsView(user));
        }
    }
}