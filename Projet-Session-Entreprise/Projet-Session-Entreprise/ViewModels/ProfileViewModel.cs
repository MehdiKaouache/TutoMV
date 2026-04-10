using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace Projet_Session_Entreprise.ViewModels
{
    public class ProfileViewModel : INotifyPropertyChanged
    {
        private readonly Student? currentStudent;
        private readonly Tutor? currentTutor;

        public event PropertyChangedEventHandler? PropertyChanged;

        public string PageTitle { get; set; } = "Profile";
        public string WelcomeText { get; set; } = "";

        public Visibility StudentSectionVisibility { get; set; } = Visibility.Collapsed;
        public Visibility TutorSectionVisibility { get; set; } = Visibility.Collapsed;

        public ObservableCollection<Tutor> Tutors { get; set; } = new ObservableCollection<Tutor>();

        public ObservableCollection<int> RatingChoices { get; } =
            new ObservableCollection<int> { 1, 2, 3, 4, 5 };

        public ObservableCollection<string> AvailabilityChoices { get; } =
            new ObservableCollection<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };

        private Tutor? selectedTutor;
        public Tutor? SelectedTutor
        {
            get => selectedTutor;
            set
            {
                selectedTutor = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(RatingInfo));
            }
        }

        private int selectedRating = 5;
        public int SelectedRating
        {
            get => selectedRating;
            set
            {
                selectedRating = value;
                OnPropertyChanged();
            }
        }

        public string RatingInfo
        {
            get
            {
                if (SelectedTutor == null)
                    return "Select a tutor to see rating.";

                return "Tutor: " + SelectedTutor.Nom + " " + SelectedTutor.Prenom +
                       " | Rating: " + SelectedTutor.AverageRating.ToString("0.0");
            }
        }

        public string TutorFullName
        {
            get
            {
                if (currentTutor == null) return "";
                return currentTutor.Nom + " " + currentTutor.Prenom;
            }
        }

        public string TutorSubject
        {
            get
            {
                if (currentTutor == null) return "";
                return currentTutor.Subject;
            }
        }

        private string selectedAvailability = "Monday";
        public string SelectedAvailability
        {
            get => selectedAvailability;
            set
            {
                selectedAvailability = value;
                OnPropertyChanged();
            }
        }

        private string tutorInfo = "";
        public string TutorInfo
        {
            get => tutorInfo;
            set
            {
                tutorInfo = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddRatingCommand { get; }
        public ICommand SaveAvailabilityCommand { get; }

        public ProfileViewModel(Student student)
        {
            currentStudent = student;

            PageTitle = "Student Profile";
            WelcomeText = "Welcome " + student.Prenom + " " + student.Nom;
            StudentSectionVisibility = Visibility.Visible;
            TutorSectionVisibility = Visibility.Collapsed;

            AddRatingCommand = new RelayCommand(AddRating);
            SaveAvailabilityCommand = new RelayCommand(_ => { });

            LoadTutors();
        }

        public ProfileViewModel(Tutor tutor)
        {
            currentTutor = tutor;

            PageTitle = "Tutor Profile";
            WelcomeText = "Welcome " + tutor.Prenom + " " + tutor.Nom;
            StudentSectionVisibility = Visibility.Collapsed;
            TutorSectionVisibility = Visibility.Visible;

            AddRatingCommand = new RelayCommand(_ => { });
            SaveAvailabilityCommand = new RelayCommand(SaveAvailability);

            SelectedAvailability = string.IsNullOrWhiteSpace(tutor.Availability)
                ? "Monday"
                : tutor.Availability;

            TutorInfo = "Current availability: " + SelectedAvailability;
        }

        private void LoadTutors()
        {
            using (var db = new AppDbContext())
            {
                var tutors = db.Tutors
                    .Where(t => t.IsValidated)
                    .ToList();

                Tutors.Clear();

                foreach (var tutor in tutors)
                {
                    Tutors.Add(tutor);
                }
            }

            OnPropertyChanged(nameof(Tutors));
            OnPropertyChanged(nameof(RatingInfo));
        }

        private void AddRating(object? obj)
        {
            if (SelectedTutor == null)
            {
                MessageBox.Show("Please select a tutor first.");
                return;
            }

            int selectedTutorId = SelectedTutor.Id;

            using (var db = new AppDbContext())
            {
                var tutor = db.Tutors.FirstOrDefault(t => t.Id == selectedTutorId);

                if (tutor != null)
                {
                    tutor.NumberOfRatings += 1;
                    tutor.TotalRatings += SelectedRating;
                    db.SaveChanges();
                }
            }

            LoadTutors();
            SelectedTutor = Tutors.FirstOrDefault(t => t.Id == selectedTutorId);
            OnPropertyChanged(nameof(RatingInfo));
        }

        private void SaveAvailability(object? obj)
        {
            if (currentTutor == null)
                return;

            using (var db = new AppDbContext())
            {
                var tutor = db.Tutors.FirstOrDefault(t => t.Id == currentTutor.Id);

                if (tutor != null)
                {
                    tutor.Availability = SelectedAvailability;
                    db.SaveChanges();

                    currentTutor.Availability = SelectedAvailability;
                    TutorInfo = "Availability updated to: " + SelectedAvailability;
                    OnPropertyChanged(nameof(TutorInfo));
                }
            }
        }

        private void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public class RelayCommand : ICommand
    {
        private readonly System.Action<object?> execute;

        public RelayCommand(System.Action<object?> execute)
        {
            this.execute = execute;
        }

        public event System.EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter) => execute(parameter);
    }
}