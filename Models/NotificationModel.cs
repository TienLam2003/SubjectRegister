namespace SubjectRegister.Models
{
    public class NotificationModel
    {
        public int MessageId { get; set; }
        public string MessageContent { get; set; }
        public string StudentCode { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
