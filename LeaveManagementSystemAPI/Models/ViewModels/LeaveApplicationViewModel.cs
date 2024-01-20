namespace LeaveManagementSystemAPI.Models.ViewModels
{
    public class LeaveApplicationViewModel
    {
        public string? LeaveType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Description { get; set; }
        public string? Username { get; set; }
        public int DaysAppliedFor { get; set; }

    }
}
