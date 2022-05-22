namespace SpeechApi.Models
{
    public class QueueEntry
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public SpeakDTO Speak { get; set; }
        public string CallbackUrl { get; set; }
    }
}
