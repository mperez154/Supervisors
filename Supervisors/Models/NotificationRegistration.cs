using System;
using System.ComponentModel.DataAnnotations;

namespace Supervisors.Models
{
    public class NotificationRegistration
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        [Required]
        public string Supervisor { get; set; }
    }
}
