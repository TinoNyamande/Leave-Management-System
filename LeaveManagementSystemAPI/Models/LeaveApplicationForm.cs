    namespace LeaveManagementSystemAPI.Models
{
    public class LeaveApplicationForm
    {
        public string? Id { get; set; } 
        public string? LeaveType { get; set; }
        public DateTime ApplicationDate { get; set; }
        public DateTime StartDate { get;set; }
        public DateTime EndDate { get; set; }
        public string? Username { get; set; }
        public string? Description { get; set; }

        public string? Status { get; set; }

        public string? FilePath { get; set; }
        public int DaysAppliedFor { get; set; }


    }
}
