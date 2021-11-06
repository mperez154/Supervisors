using System;
using System.ComponentModel.DataAnnotations;

namespace Supervisors.Models
{
    public class NotificationRegistration
    {
        [Required] // Requirement: If firstName, lastName, or supervisor is not provided, the endpoint should return an error response
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public string Supervisor { get; set; }
    }
}
