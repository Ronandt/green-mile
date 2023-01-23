using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    public class DonationRequest
    {
        [Key]
        public int Id { get; set; }
        public Donation? Donation { get; set; }

        public User? Donor { get; set; }

        public User? Recipient { get; set; }

        public RequestStatus Status { get; set; }

    }
}

public enum RequestStatus
{
    Pending,
    Accpeted,
    Done,
}
