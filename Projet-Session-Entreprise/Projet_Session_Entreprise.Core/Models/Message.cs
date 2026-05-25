using System;

namespace Projet_Session_Entreprise.Core.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderRole { get; set; } = string.Empty;
        public int ReceiverId { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime SentAt { get; set; } = DateTime.Now;
        public bool IsRead { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }
}