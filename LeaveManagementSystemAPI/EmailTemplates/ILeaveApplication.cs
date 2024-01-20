using LeaveManagementSystemAPI.Models;
using LeaveManagementSystemAPI.Models.ViewModels;

namespace LeaveManagementSystemAPI.EmailTemplates
{
    public interface ILeaveApplication
    {
        public string Apply(LeaveApplicationForm application);
        public string Approve(LeaveApplicationForm application);
        public string Reject(LeaveApplicationForm application);
    }
}
