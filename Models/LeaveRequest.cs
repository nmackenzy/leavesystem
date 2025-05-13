using System.ComponentModel.DataAnnotations;

namespace simple_api.Models
{
    public class LeaveRequest
    {
        public int RequestId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public string Reason { get; set; } = string.Empty;

        public string Status { get; set; } = "Pending";

        public string? ManagerComments { get; set; }

        public User? User { get; set; }
    }
}
