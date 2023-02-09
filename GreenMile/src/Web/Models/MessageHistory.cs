using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class MessageHistory
    {
        [Key]
        public int Id { get; set; }
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string Text { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
