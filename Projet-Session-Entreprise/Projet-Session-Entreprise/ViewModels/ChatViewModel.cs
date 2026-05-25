using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Projet_Session_Entreprise.Core.Models;
using Projet_Session_Entreprise.Infrastructure.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.UI.ViewModels
{
    public class ChatMessageDisplay
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public string Timestamp { get; set; } = string.Empty;
        public bool IsMine { get; set; }
        public string BubbleColor => IsMine ? "#7B61FF" : "#2A2A4A";
        public string Alignment => IsMine ? "Right" : "Left";
    }

    public partial class ChatViewModel : ObservableObject
    {
        private readonly ConversationItem _conversation;
        private int _currentUserId;
        private string _currentUserRole = "";

        [ObservableProperty] private string _chatPartnerName = string.Empty;
        [ObservableProperty] private string _newMessageText = string.Empty;

        public ObservableCollection<ChatMessageDisplay> Messages { get; set; } = new();

        public ChatViewModel(ConversationItem conversation)
        {
            _conversation = conversation;
            ChatPartnerName = conversation.PartnerName;

            if (CurrentSessionService.CurrentUser is Student s)
            {
                _currentUserId = s.Id;
                _currentUserRole = "Student";
            }
            else if (CurrentSessionService.CurrentUser is Tutor t)
            {
                _currentUserId = t.Id;
                _currentUserRole = "Tutor";
            }

            _ = LoadMessagesAsync();
        }

        private async Task LoadMessagesAsync()
        {
            var msgs = await App.MessageRepo.GetConversationAsync(_currentUserId, _currentUserRole, _conversation.PartnerId);

            Messages.Clear();
            foreach (var m in msgs)
            {
                Messages.Add(new ChatMessageDisplay
                {
                    Id = m.Id,
                    Content = m.Content,
                    Timestamp = m.SentAt.ToString("dd/MM HH:mm"),
                    IsMine = (m.SenderId == _currentUserId && m.SenderRole == _currentUserRole)
                });
            }

            await App.MessageRepo.MarkMessagesAsReadAsync(_currentUserId, _currentUserRole, _conversation.PartnerId);
        }

        [RelayCommand]
        public async Task SendMessageAsync()
        {
            if (string.IsNullOrWhiteSpace(NewMessageText)) return;

            var msg = new Message
            {
                SenderId = _currentUserId,
                SenderRole = _currentUserRole,
                ReceiverId = _conversation.PartnerId,
                Content = NewMessageText.Trim(),
                SentAt = DateTime.Now,
                IsRead = false,
                IsDeleted = false
            };

            await App.MessageRepo.SendMessageAsync(msg);

            Messages.Add(new ChatMessageDisplay
            {
                Id = msg.Id,
                Content = msg.Content,
                Timestamp = msg.SentAt.ToString("dd/MM HH:mm"),
                IsMine = true
            });

            NewMessageText = string.Empty;
        }

        [RelayCommand]
        public async Task DeleteMessageAsync(int messageId)
        {
            await App.MessageRepo.DeleteMessageAsync(messageId);
            var msg = Messages.FirstOrDefault(m => m.Id == messageId);
            if (msg != null) Messages.Remove(msg);
        }
    }
}