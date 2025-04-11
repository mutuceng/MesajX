namespace MesajX.ChatService.Events
{
    public class MessageCreatedEvent
    {
        public string MessageId { get; set; }
        public string UserId { get; set; }
        public string ChatRoomId { get; set; }
        public string Content { get; set; }
        public string? MediaUrl { get; set; }
        public DateTime SentAt { get; set; }
    }
}
