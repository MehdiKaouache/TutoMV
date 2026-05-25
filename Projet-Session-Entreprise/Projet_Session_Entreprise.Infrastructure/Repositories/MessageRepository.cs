using Microsoft.EntityFrameworkCore;
using Projet_Session_Entreprise.Core.Interfaces;
using Projet_Session_Entreprise.Core.Models;
using Projet_Session_Entreprise.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projet_Session_Entreprise.Infrastructure.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        public MessageRepository(AppDbContext context) { }

        public async Task<IEnumerable<Message>> GetConversationAsync(int currentUserId, string currentUserRole, int partnerId)
        {
            using (var db = new AppDbContext())
            {
                return await db.Messages
                    .Where(m => !m.IsDeleted &&
                               ((m.SenderId == currentUserId && m.SenderRole == currentUserRole && m.ReceiverId == partnerId) ||
                                (m.SenderId == partnerId && m.SenderRole != currentUserRole && m.ReceiverId == currentUserId)))
                    .OrderBy(m => m.SentAt)
                    .ToListAsync();
            }
        }

        public async Task<Message> SendMessageAsync(Message message)
        {
            using (var db = new AppDbContext())
            {
                db.Messages.Add(message);
                await db.SaveChangesAsync();
                return message;
            }
        }

        public async Task MarkMessagesAsReadAsync(int currentUserId, string currentUserRole, int partnerId)
        {
            using (var db = new AppDbContext())
            {
                var unread = await db.Messages
                    .Where(m => m.ReceiverId == currentUserId && m.SenderRole != currentUserRole && m.SenderId == partnerId && !m.IsRead)
                    .ToListAsync();

                if (unread.Any())
                {
                    foreach (var m in unread) m.IsRead = true;
                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task DeleteMessageAsync(int messageId)
        {
            using (var db = new AppDbContext())
            {
                var msg = await db.Messages.FindAsync(messageId);
                if (msg != null)
                {
                    msg.IsDeleted = true;
                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task ArchiveConversationAsync(int userId, int partnerId)
        {
            using (var db = new AppDbContext())
            {
                if (!await db.ArchivedConversations.AnyAsync(a => a.UserId == userId && a.PartnerId == partnerId))
                {
                    db.ArchivedConversations.Add(new ArchivedConversation { UserId = userId, PartnerId = partnerId });
                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task UnarchiveConversationAsync(int userId, int partnerId)
        {
            using (var db = new AppDbContext())
            {
                var archive = await db.ArchivedConversations.FirstOrDefaultAsync(a => a.UserId == userId && a.PartnerId == partnerId);
                if (archive != null)
                {
                    db.ArchivedConversations.Remove(archive);
                    await db.SaveChangesAsync();
                }
            }
        }

        public async Task<List<int>> GetArchivedPartnerIdsAsync(int userId)
        {
            using (var db = new AppDbContext())
            {
                return await db.ArchivedConversations.Where(a => a.UserId == userId).Select(a => a.PartnerId).ToListAsync();
            }
        }
    }
}