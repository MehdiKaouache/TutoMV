using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.EntityFrameworkCore;
using Projet_Session_Entreprise.Models;
using Projet_Session_Entreprise.Services;

namespace Projet_Session_Entreprise.Views
{
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        private async void BtnUpdateDA_Click(object sender, RoutedEventArgs e)
        {
            string nouveauDA = txtNouveauDA.Text.Trim();

            if (string.IsNullOrEmpty(nouveauDA))
            {
                txtStatut.Text = "Le nouveau DA ne peut pas être vide.";
                return;
            }

            try
            {
                using (var db = new AppDbContext())
                {
                    bool daDejaUtilise = await db.Students.AnyAsync(s => s.DA == nouveauDA)
                                     || await db.Tutors.AnyAsync(t => t.DA == nouveauDA);

                    if (daDejaUtilise)
                    {
                        txtStatut.Text = "Ce DA est déjà utilisé par un autre compte.";
                        return;
                    }

                    if (CurrentSessionService.CurrentUser is Student student)
                    {
                        var studentDb = await db.Students.FirstOrDefaultAsync(s => s.Id == student.Id);
                        if (studentDb != null)
                        {
                            studentDb.DA = nouveauDA;
                            await db.SaveChangesAsync();
                            student.DA = nouveauDA;
                            txtStatut.Text = "Numéro DA mis à jour avec succès.";
                        }
                    }
                    else if (CurrentSessionService.CurrentUser is Tutor tutor)
                    {
                        var tutorDb = await db.Tutors.FirstOrDefaultAsync(t => t.Id == tutor.Id);
                        if (tutorDb != null)
                        {
                            tutorDb.DA = nouveauDA;
                            await db.SaveChangesAsync();
                            tutor.DA = nouveauDA;
                            txtStatut.Text = "Numéro DA mis à jour avec succès.";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                txtStatut.Text = "Erreur : " + ex.Message;
            }
        }

        private async void BtnUpdatePassword_Click(object sender, RoutedEventArgs e)
        {
            string motDePasseActuel = txtMotDePasseActuel.Password;
            string nouveauMotDePasse = txtNouveauMotDePasse.Password;
            string confirmMotDePasse = txtConfirmMotDePasse.Password;

            if (string.IsNullOrEmpty(motDePasseActuel) || string.IsNullOrEmpty(nouveauMotDePasse) || string.IsNullOrEmpty(confirmMotDePasse))
            {
                txtStatut.Text = "Tous les champs sont obligatoires.";
                return;
            }

            if (nouveauMotDePasse != confirmMotDePasse)
            {
                txtStatut.Text = "Le nouveau mot de passe et la confirmation ne correspondent pas.";
                return;
            }

            try
            {
                using (var db = new AppDbContext())
                {
                    if (CurrentSessionService.CurrentUser is Student student)
                    {
                        var studentDb = await db.Students.FirstOrDefaultAsync(s => s.Id == student.Id);
                        if (studentDb == null) return;

                        if (!BCrypt.Net.BCrypt.Verify(motDePasseActuel, studentDb.Password))
                        {
                            txtStatut.Text = "Mot de passe actuel incorrect.";
                            return;
                        }

                        studentDb.Password = BCrypt.Net.BCrypt.HashPassword(nouveauMotDePasse);
                        await db.SaveChangesAsync();
                        txtStatut.Text = "Mot de passe mis à jour avec succès.";
                    }
                    else if (CurrentSessionService.CurrentUser is Tutor tutor)
                    {
                        var tutorDb = await db.Tutors.FirstOrDefaultAsync(t => t.Id == tutor.Id);
                        if (tutorDb == null) return;

                        if (!BCrypt.Net.BCrypt.Verify(motDePasseActuel, tutorDb.Password))
                        {
                            txtStatut.Text = "Mot de passe actuel incorrect.";
                            return;
                        }

                        tutorDb.Password = BCrypt.Net.BCrypt.HashPassword(nouveauMotDePasse);
                        await db.SaveChangesAsync();
                        txtStatut.Text = "Mot de passe mis à jour avec succès.";
                    }
                }

                txtMotDePasseActuel.Clear();
                txtNouveauMotDePasse.Clear();
                txtConfirmMotDePasse.Clear();
            }
            catch (Exception ex)
            {
                txtStatut.Text = "Erreur : " + ex.Message;
            }
        }
    }
}
