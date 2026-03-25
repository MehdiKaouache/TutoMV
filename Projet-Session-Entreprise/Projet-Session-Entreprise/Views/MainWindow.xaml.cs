using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Projet_Session_Entreprise
{
    public partial class MainWindow : Window
    {
        private Teacher? selected;

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
                grid.ItemsSource = db.Teachers.ToList();
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
                Teacher t = new Teacher
                {
                    Name = txtName.Text,
                    Subject = txtSubject.Text,
                    Availability = GetSelectedAvailability()
                };

                db.Teachers.Add(t);
                db.SaveChanges();
            }

            LoadData();
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            if (selected == null) return;

            using (var db = new AppDbContext())
            {
                var t = db.Teachers.FirstOrDefault(x => x.Id == selected.Id);

                if (t != null)
                {
                    t.Name = txtName.Text;
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
                var t = db.Teachers.FirstOrDefault(x => x.Id == selected.Id);

                if (t != null)
                {
                    db.Teachers.Remove(t);
                    db.SaveChanges();
                }
            }

            LoadData();
        }

        private void grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (grid.SelectedItem is Teacher t)
            {
                selected = t;

                txtName.Text = t.Name;
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