using LeaveManagementSystemAPI.Models;
using LeaveManagementSystemAPI.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystemAPI.MyServices
{
    public class GetApplicationBy : IGetApplicationBy
    {
        private readonly ApplicationDbContext _context;
        public GetApplicationBy(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<LeaveApplicationForm>> GetAllApplicationsBy(string? Id, string? Status,string? Username,string? LeaveType)

        {
            if (Id !=null)
            {
                var applications = await _context.LeaveApplicationForm.Where(a => a.Id == Id).ToListAsync();
                return applications;
            }
            if (Status != null && Username != null && LeaveType != null)
            {
                var applications = await _context.LeaveApplicationForm.Where(a => a.Status == Status)
                                                                      .Where(a=>a.Username == Username)
                                                                      .Where(a=>a.LeaveType == LeaveType)
                                                                      .ToListAsync();
                return applications;
            }
            if (Status != null && Username != null && LeaveType == null)
            {
                var applications = await _context.LeaveApplicationForm.Where(a => a.Status == Status)
                                                      .Where(a => a.Username == Username)
                                                      .ToListAsync();
                return applications;
            }
            if (Status != null && Username == null && LeaveType != null)
            {
                var applications = await _context.LeaveApplicationForm.Where(a => a.Status == Status)
                                                      .Where(a => a.LeaveType == LeaveType)
                                                      .ToListAsync();
                return applications;
            }
            if (Status == null && Username != null && LeaveType != null)
            {
                var applications = await _context.LeaveApplicationForm.Where(a => a.Username == Username)
                                                      .Where(a => a.LeaveType == LeaveType)
                                                      .ToListAsync();
                return applications;
            }
            if (Status == null && Username == null && LeaveType != null)
            {
                var applications = await _context.LeaveApplicationForm
                                                      .Where(a => a.LeaveType == LeaveType)
                                                      .ToListAsync();
                return applications;
            }
            if (Status == null && Username != null && LeaveType == null)
            {
                var applications = await _context.LeaveApplicationForm
                                                      .Where(a => a.Username == Username)
                                                      .ToListAsync();
                return applications;
            }
            if (Status != null && Username == null && LeaveType == null)
            {
                var applications = await _context.LeaveApplicationForm
                                                      .Where(a => a.Status == Status)
                                                      .ToListAsync();
                return applications;
            }
                var apps = await _context.LeaveApplicationForm.ToListAsync();
                return apps;
            
        }
    }
}
