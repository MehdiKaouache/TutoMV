using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Projet_Session_Entreprise
{
    public partial class MainWindow : Window
    {
        private Tutor selected;

        public MainWindow()
        {
            InitializeComponent();
            LoadData();

            // default selection
            cmbAvailability.SelectedIndex = 0;
        }

        private void LoadData()
        {
            using (var db = new AppDbContext())
            {
                grid.ItemsSource = db.Tutors.ToList();
            }
        }

        private string GetSelectedAvailability()
        {
            return ((ComboBoxItem)cmbAvailability.SelectedItem)?.Content?.ToString() ?? "";
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new AppDbContext())
            {
                Tutor t = new Tutor
                {
                    Nom = txtName.Text,
                    Subject = txtSubject.Text,
                    Availability = GetSelectedAvailability()
                };

                db.Tutors.Add(t);
                db.SaveChanges();
            }

            LoadData();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (selected == null) return;

            using (var db = new AppDbContext())
            {
                var t = db.Tutors.FirstOrDefault(x => x.Id == selected.Id);

                if (t != null)
                {
                    t.Nom = txtName.Text;
                    t.Subject = txtSubject.Text;
                    t.Availability = GetSelectedAvailability();

                    db.SaveChanges();
                }
            }

            LoadData();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (selected == null) return;

            using (var db = new AppDbContext())
            {
                var t = db.Tutors.FirstOrDefault(x => x.Id == selected.Id);

                if (t != null)
                {
                    db.Tutors.Remove(t);
                    db.SaveChanges();
                }
            }

            LoadData();
        }

        private void grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grid.SelectedItem is Tutor t)
            {
                selected = t;

                txtName.Text = t.Nom;
                txtSubject.Text = t.Subject;

                // set ComboBox selection
                foreach (ComboBoxItem item in cmbAvailability.Items)
                {
                    if (item.Content.ToString() == t.Availability)
                    {
                        cmbAvailability.SelectedItem = item;
                        break;
                    }
                }
            }
        }
    }
}