using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Projet_Session_Entreprise.Core.Models;
using Projet_Session_Entreprise.Infrastructure.Data;
using Projet_Session_Entreprise.Infrastructure.Services;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.UI.ViewModels
{
    public class ConversationItem
    {
        public int PartnerId { get; set; }
        public string PartnerName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }

    public partial class MessageListViewModel : ObservableObject
    {
        [ObservableProperty] private ConversationItem? _selectedConversation;
        [ObservableProperty] private object? _chatViewModel;

        public ObservableCollection<ConversationItem> ActiveConversations { get; set; } = new();
        public ObservableCollection<ConversationItem> ArchivedConversations { get; set; } = new();

        public MessageListViewModel()
        {
            _ = LoadConversationsAsync();
        }

        private async Task LoadConversationsAsync()
        {
            var user = CurrentSessionService.CurrentUser;
            if (user == null) return;

            int currentUserId = user is Student s ? s.Id : ((Tutor)user).Id;

            ActiveConversations.Clear();
            ArchivedConversations.Clear();

            using (var db = new AppDbContext())
            {
                var archivedIds = await App.MessageRepo.GetArchivedPartnerIdsAsync(currentUserId);

                if (user is Student student)
                {
                    var partners = db.Appointments
                        .Where(a => a.StudentId == student.Id && (a.Status == "Accepté" || a.Status == "Complété" || a.Status == "Terminé"))
                        .Select(a => a.Tutor)
                        .Distinct()
                        .ToList();

                    foreach (var p in partners)
                    {
                        if (p == null) continue;
                        var item = new ConversationItem { PartnerId = p.Id, PartnerName = $"{p.Prenom} {p.Nom}", Role = "Tuteur" };
                        if (archivedIds.Contains(p.Id)) ArchivedConversations.Add(item);
                        else ActiveConversations.Add(item);
                    }
                }
                else if (user is Tutor tutor)
                {
                    var partners = db.Appointments
                        .Where(a => a.TutorId == tutor.Id && (a.Status == "Accepté" || a.Status == "Complété" || a.Status == "Terminé"))
                        .Select(a => a.Student)
                        .Distinct()
                        .ToList();

                    foreach (var p in partners)
                    {
                        if (p == null) continue;
                        var item = new ConversationItem { PartnerId = p.Id, PartnerName = $"{p.Prenom} {p.Nom}", Role = "Étudiant" };
                        if (archivedIds.Contains(p.Id)) ArchivedConversations.Add(item);
                        else ActiveConversations.Add(item);
                    }
                }
            }
        }

        partial void OnSelectedConversationChanged(ConversationItem? value)
        {
            if (value != null) ChatViewModel = new ChatViewModel(value);
            else ChatViewModel = null;
        }

        [RelayCommand]
        public async Task Archive(ConversationItem item)
        {
            if (item == null) return;
            int currentUserId = CurrentSessionService.CurrentUser is Student s ? s.Id : ((Tutor)CurrentSessionService.CurrentUser!).Id;
            await App.MessageRepo.ArchiveConversationAsync(currentUserId, item.PartnerId);
            await LoadConversationsAsync();
            SelectedConversation = null;
        }

        [RelayCommand]
        public async Task Unarchive(ConversationItem item)
        {
            if (item == null) return;
            int currentUserId = CurrentSessionService.CurrentUser is Student s ? s.Id : ((Tutor)CurrentSessionService.CurrentUser!).Id;
            await App.MessageRepo.UnarchiveConversationAsync(currentUserId, item.PartnerId);
            await LoadConversationsAsync();
            SelectedConversation = null;
        }
    }
}