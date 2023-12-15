using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RealEstateListingAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; } // Store password as a hash

        [Required]
        public string Role { get; set; } // Could be 'Buyer', 'Seller', 'Admin'

        // Navigation properties
        public ICollection<Property> Properties { get; set; } // If implementing a one-to-many relationship with Properties
        public ICollection<Appointment> Appointments { get; set; } // If implementing a relationship with Appointments
    }
}
