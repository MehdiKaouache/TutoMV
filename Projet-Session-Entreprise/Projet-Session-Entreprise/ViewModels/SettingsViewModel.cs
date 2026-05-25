using CommunityToolkit.Mvvm.ComponentModel;
using Projet_Session_Entreprise.Core.Models;
using Projet_Session_Entreprise.Infrastructure.Data;
using Projet_Session_Entreprise.Infrastructure.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Projet_Session_Entreprise.UI.ViewModels
{
    public class HistoryItem
    {
        public string Date { get; set; } = "";
        public string Partenaire { get; set; } = "";
        public string RolePartenaire { get; set; } = "";
    }

    public partial class SettingsViewModel : ObservableObject
    {
        [ObservableProperty] private string _statusMessage = "";
        [ObservableProperty] private bool _isSuccess = true;

        public ObservableCollection<HistoryItem> Historique { get; set; } = new();

        public SettingsViewModel()
        {
            LoadHistorique();
        }

        private void LoadHistorique()
        {
            var user = CurrentSessionService.CurrentUser;
            if (user == null) return;

            using (var db = new AppDbContext())
            {
                var query = db.CompletedAppointments
                              .Include(c => c.Appointment)
                              .AsQueryable();

                if (user is Student s)
                {
                    var studentHistory = query.Where(c => c.StudentId == s.Id).Include(c => c.Tutor).OrderByDescending(c => c.CompletedDate).ToList();
                    foreach (var h in studentHistory)
                    {
                        Historique.Add(new HistoryItem
                        {
                            Date = h.CompletedDate.ToString("dd MMMM yyyy - HH:mm"),
                            Partenaire = $"{h.Tutor?.Prenom} {h.Tutor?.Nom}",
                            RolePartenaire = "Tuteur"
                        });
                    }
                }
                else if (user is Tutor t)
                {
                    var tutorHistory = query.Where(c => c.TutorId == t.Id).Include(c => c.Student).OrderByDescending(c => c.CompletedDate).ToList();
                    foreach (var h in tutorHistory)
                    {
                        Historique.Add(new HistoryItem
                        {
                            Date = h.CompletedDate.ToString("dd MMMM yyyy - HH:mm"),
                            Partenaire = $"{h.Student?.Prenom} {h.Student?.Nom}",
                            RolePartenaire = "Élève"
                        });
                    }
                }
            }
        }

        public async Task UpdateDAAsync(string nouveauDA)
        {
            if (string.IsNullOrWhiteSpace(nouveauDA) || nouveauDA.Length != 7 || !nouveauDA.All(char.IsDigit))
            {
                ShowMessage("Le numéro de DA doit contenir exactement 7 chiffres.", false);
                return;
            }

            using (var db = new AppDbContext())
            {
                bool daDejaUtilise = await db.Students.AnyAsync(s => s.DA == nouveauDA) || await db.Tutors.AnyAsync(t => t.DA == nouveauDA);

                if (daDejaUtilise)
                {
                    ShowMessage("Ce DA est déjà utilisé par un autre compte.", false);
                    return;
                }

                var user = CurrentSessionService.CurrentUser;
                if (user is Student student)
                {
                    var studentDb = await db.Students.FirstOrDefaultAsync(s => s.Id == student.Id);
                    if (studentDb != null) { studentDb.DA = nouveauDA; await db.SaveChangesAsync(); student.DA = nouveauDA; }
                }
                else if (user is Tutor tutor)
                {
                    var tutorDb = await db.Tutors.FirstOrDefaultAsync(t => t.Id == tutor.Id);
                    if (tutorDb != null) { tutorDb.DA = nouveauDA; await db.SaveChangesAsync(); tutor.DA = nouveauDA; }
                }

                ShowMessage("Numéro DA mis à jour avec succès !", true);
            }
        }

        public async Task UpdatePasswordAsync(string actuel, string nouveau, string confirm)
        {
            if (string.IsNullOrWhiteSpace(actuel) || string.IsNullOrWhiteSpace(nouveau) || string.IsNullOrWhiteSpace(confirm))
            {
                ShowMessage("Veuillez remplir tous les champs du mot de passe.", false);
                return;
            }

            if (nouveau.Length < 8 || nouveau != confirm)
            {
                ShowMessage("Le mot de passe doit faire 8 caractères et correspondre à la confirmation.", false);
                return;
            }

            using (var db = new AppDbContext())
            {
                var user = CurrentSessionService.CurrentUser;
                if (user is Student student)
                {
                    var studentDb = await db.Students.FirstOrDefaultAsync(s => s.Id == student.Id);
                    if (studentDb != null && BCrypt.Net.BCrypt.Verify(actuel, studentDb.Password))
                    {
                        studentDb.Password = BCrypt.Net.BCrypt.HashPassword(nouveau);
                        await db.SaveChangesAsync();
                        ShowMessage("Mot de passe mis à jour avec succès !", true);
                    }
                    else ShowMessage("Mot de passe actuel incorrect.", false);
                }
                else if (user is Tutor tutor)
                {
                    var tutorDb = await db.Tutors.FirstOrDefaultAsync(t => t.Id == tutor.Id);
                    if (tutorDb != null && BCrypt.Net.BCrypt.Verify(actuel, tutorDb.Password))
                    {
                        tutorDb.Password = BCrypt.Net.BCrypt.HashPassword(nouveau);
                        await db.SaveChangesAsync();
                        ShowMessage("Mot de passe mis à jour avec succès !", true);
                    }
                    else ShowMessage("Mot de passe actuel incorrect.", false);
                }
            }
        }

        private void ShowMessage(string msg, bool success)
        {
            StatusMessage = msg;
            IsSuccess = success;
        }
    }
}