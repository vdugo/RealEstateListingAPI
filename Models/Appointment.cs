using System;
using System.ComponentModel.DataAnnotations;

namespace RealEstateListingAPI.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        public int PropertyId { get; set; } // Foreign key reference to the Property

        [Required]
        public int UserId { get; set; } // Foreign key reference to the User (buyer or seller)

        // Navigation properties
        public Property Property { get; set; }
        public User User { get; set; }
    }
}
