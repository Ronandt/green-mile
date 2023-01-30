using System.ComponentModel.DataAnnotations;

namespace Web.Models;

/// <summary>
/// Represents a donation offer that a user create.
/// </summary>
public class Donation
{
    /// <summary>
    /// The unique identifier for the donation offer.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// The status of the donation offer, whether pending, active, complete, expired.
    /// </summary>
    public DonationStatus Status { get; set; }

    /// <summary>
    /// The date and time that the donation offer was created.
    /// </summary>
    public DateTime Date { get; set; } = DateTime.Now;

    /// <summary>
    /// The user that created the donation offer.
    /// </summary>

    public User User { get; set; } = new User();

    /// <summary>
    /// Refer to the food item class
    /// </summary>
    public CustomFood CustomFood { get; set; }  = new CustomFood();


}

public enum DonationStatus
{
    ACTIVE,
    RESERVED,
    COMPLETED,
}