using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Projet_Session_Entreprise.Models;
using Projet_Session_Entreprise.Services;

namespace Projet_Session_Entreprise.Views
{
    public partial class RequeteRoleTuteurView : UserControl
    {
        private Tutor _tutor;

        public RequeteRoleTuteurView(Tutor tutor)
        {
            InitializeComponent();
            _tutor = tutor;
        }

        private void BtnFinalize_Click(object sender, RoutedEventArgs e)
        {
            string courseGradeInput = txtGradeCourse.Text.Trim();
            var selectedSubjects = lstSubjects.SelectedItems.Cast<ListBoxItem>()
                .Select(i => i.Content?.ToString() ?? "")
                .Where(s => !string.IsNullOrEmpty(s))
                .ToList();

            if (selectedSubjects.Count == 0 || string.IsNullOrEmpty(txtCourseTargeted.Text) || string.IsNullOrEmpty(courseGradeInput))
            {
                MessageBox.Show("Veuillez remplir tous les détails.");
                return;
            }

            if (!double.TryParse(courseGradeInput, out double courseGrade) || courseGrade < 80)
            {
                MessageBox.Show("Erreur : Une note de 80% dans le cours est requise.");
                return;
            }

            try
            {
                using (var db = new AppDbContext())
                {
                    Tutor tutorToUpdate = null;

                    if (_tutor == null)
                    {
                        string currentDa = null;

                        if (CurrentSessionService.CurrentUser is Student s) currentDa = s.DA;
                        else if (CurrentSessionService.CurrentUser is Tutor t) currentDa = t.DA;

                        if (!string.IsNullOrEmpty(currentDa))
                        {
                            tutorToUpdate = db.Tutors.FirstOrDefault(t => t.DA == currentDa);
                        }
                    }
                    else
                    {
                        tutorToUpdate = db.Tutors.FirstOrDefault(t => t.Id == _tutor.Id);
                    }

                    if (tutorToUpdate != null)
                    {
                        tutorToUpdate.Subject = string.Join(", ", selectedSubjects);
                        tutorToUpdate.Availability = (cmbAvailability.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Lundi";
                        tutorToUpdate.IsValidated = true;

                        db.SaveChanges();
                        CurrentSessionService.CurrentUser = tutorToUpdate;
                    }
                    else if (_tutor == null && CurrentSessionService.CurrentUser is Student stud)
                    {
                        var newTutor = new Tutor
                        {
                            Nom = stud.Nom,
                            Prenom = stud.Prenom,
                            DA = stud.DA,
                            Password = stud.Password,
                            AverageGrade = courseGrade,
                            Subject = string.Join(", ", selectedSubjects),
                            Availability = (cmbAvailability.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Lundi",
                            Role = "Tuteur",
                            IsValidated = true
                        };
                        db.Tutors.Add(newTutor);
                        db.SaveChanges();
                        CurrentSessionService.CurrentUser = newTutor;
                    }
                }

                MessageBox.Show("Inscription terminée avec succès !");
                MainView.Instance.NavigateTo(new HomeView());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de la finalisation : " + ex.Message);
            }
        }
    }
}