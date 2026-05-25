using Projet_Session_Entreprise.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.Core.Interfaces
{
    public interface IMessageRepository
    {
        Task<IEnumerable<Message>> GetConversationAsync(int currentUserId, string currentUserRole, int partnerId);
        Task<Message> SendMessageAsync(Message message);
        Task MarkMessagesAsReadAsync(int currentUserId, string currentUserRole, int partnerId);
        Task DeleteMessageAsync(int messageId);
        Task ArchiveConversationAsync(int userId, int partnerId);
        Task UnarchiveConversationAsync(int userId, int partnerId);
        Task<List<int>> GetArchivedPartnerIdsAsync(int userId);
    }
}