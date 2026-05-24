using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Projet_Session_Entreprise.Infrastructure.Data;
using Projet_Session_Entreprise.Core.Models;
using Projet_Session_Entreprise.Infrastructure.Services;
using Projet_Session_Entreprise.UI.Views;
using System;
using System.Linq;

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

        [ObservableProperty] private string _tutorEtudiants = "0";
        [ObservableProperty] private string _tutorHeures = "0h";
        [ObservableProperty] private string _tutorNote = "-/5";

        [ObservableProperty] private bool _hasNotifications;
        [ObservableProperty] private string _notificationText = "Aucune nouvelle notification.";

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

            if (IsGuest || CurrentSessionService.HasSeenNotifications) return;

            using (var db = new AppDbContext())
            {
                if (IsTutor && user is Tutor t)
                {
                    var appts = db.Appointments.Where(a => a.TutorId == t.Id).ToList();
                    var revs = db.Reviews.Where(r => r.TutorId == t.Id).ToList();

                    var completedAppts = appts.Where(a => a.Status != null && (a.Status.ToLower() == "complété" || a.Status.ToLower() == "terminé")).ToList();

                    TutorEtudiants = completedAppts.Select(a => a.StudentId).Distinct().Count().ToString();
                    TutorHeures = completedAppts.Count.ToString() + "h";
                    TutorNote = revs.Any() ? Math.Round(revs.Average(r => r.Rating), 1).ToString() + "/5" : "-/5";

                    var pendingRequests = appts.Count(a => a.Status == "En attente");
                    var upcomingAppts = appts.Count(a => a.Status == "Accepté");

                    if (pendingRequests > 0 && upcomingAppts > 0)
                    {
                        NotificationText = $"Vous avez {pendingRequests} demande(s) en attente et {upcomingAppts} séance(s) prévue(s).";
                        HasNotifications = true;
                    }
                    else if (pendingRequests > 0)
                    {
                        NotificationText = $"Vous avez {pendingRequests} demande(s) de rendez-vous en attente.";
                        HasNotifications = true;
                    }
                    else if (upcomingAppts > 0)
                    {
                        NotificationText = $"Vous avez {upcomingAppts} séance(s) prévue(s).";
                        HasNotifications = true;
                    }
                }
                else if (IsStudent && user is Student s)
                {
                    var appts = db.Appointments.Where(a => a.StudentId == s.Id).ToList();
                    var completedAppts = appts.Where(a => a.Status != null && (a.Status.ToLower() == "complété" || a.Status.ToLower() == "terminé")).ToList();

                    StudentTotalSeances = completedAppts.Count.ToString();
                    StudentHeures = completedAppts.Count.ToString() + "h";

                    var pendingRequests = appts.Count(a => a.Status == "En attente");
                    var upcomingAppts = appts.Count(a => a.Status == "Accepté");

                    if (pendingRequests > 0 && upcomingAppts > 0)
                    {
                        NotificationText = $"Vous avez {pendingRequests} demande(s) en attente et {upcomingAppts} séance(s) prévue(s).";
                        HasNotifications = true;
                    }
                    else if (pendingRequests > 0)
                    {
                        NotificationText = $"Vous avez {pendingRequests} demande(s) de rendez-vous en attente.";
                        HasNotifications = true;
                    }
                    else if (upcomingAppts > 0)
                    {
                        NotificationText = $"Vous avez {upcomingAppts} séance(s) prévue(s).";
                        HasNotifications = true;
                    }
                }
            }
        }

        [RelayCommand]
        public void RegisterGuest() => MainView.Instance.NavigateTo(new RoleSelectionView());

        [RelayCommand]
        public void Explore()
        {
            if (IsStudent) MainView.Instance.NavigateTo(new TutorListView());
            else MainView.Instance.NavigateTo(new LoginView());
        }

        [RelayCommand]
        public void ManageRequests()
        {
            if (CurrentSessionService.CurrentUser is Tutor t)
                MainView.Instance.NavigateTo(new ReceivedRequestsView(t));
        }
    }
}