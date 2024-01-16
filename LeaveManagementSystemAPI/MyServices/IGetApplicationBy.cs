using LeaveManagementSystemAPI.Models;

namespace LeaveManagementSystemAPI.MyServices
{
    public interface IGetApplicationBy
    {
        public  Task<List<LeaveApplicationForm>> GetAllApplicationsBy(string? Id,
            string? Status,
            string? Username ,
            string? LeaveType 
            );
    }
}
