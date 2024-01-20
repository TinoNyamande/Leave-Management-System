
using LeaveManagementSystemAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystemAPI.MyServices
{
    public class CalculateLeaveDays : ICalculateLeaveDays
    {
        private readonly ApplicationDbContext _context;
        public CalculateLeaveDays(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<string,int>> CalculateDaysPerMonth(string username)
        {
            var currentDate = DateTime.Now;
            var janDate = new DateTime(currentDate.Year, 1, 31);
            var febDate = new DateTime(currentDate.Year, 2, 28);
            var marDate = new DateTime(currentDate.Year, 3, 31);
            var aprDate = new DateTime(currentDate.Year, 4, 30);
            var mayDate = new DateTime(currentDate.Year, 5, 31);
            var junDate = new DateTime(currentDate.Year, 6, 30);
            var julDate = new DateTime(currentDate.Year, 7, 31);
            var augDate = new DateTime(currentDate.Year, 8, 31);
            var sepDate = new DateTime(currentDate.Year, 9, 30);
            var octDate = new DateTime(currentDate.Year, 10, 31);
            var novDate = new DateTime(currentDate.Year, 11, 30);
            var decDate = new DateTime(currentDate.Year, 12, 31);
            var jan =  _context.LeaveApplicationForm
                .Where(a => a.Username == username && a.StartDate <= janDate)
                .Sum(a=>a.DaysAppliedFor);
            var feb = _context.LeaveApplicationForm
               .Where(a => a.Username == username && a.StartDate >janDate &&  a.StartDate <= febDate)
               .Sum(a => a.DaysAppliedFor);
            var mar = _context.LeaveApplicationForm
               .Where(a => a.Username == username && a.StartDate > febDate && a.StartDate <= marDate)
               .Sum(a => a.DaysAppliedFor);
            var apr = _context.LeaveApplicationForm
               .Where(a => a.Username == username && a.StartDate > marDate && a.StartDate <= aprDate)
               .Sum(a => a.DaysAppliedFor);
            var may = _context.LeaveApplicationForm
               .Where(a => a.Username == username && a.StartDate > aprDate && a.StartDate <= mayDate)
               .Sum(a => a.DaysAppliedFor);
            var jun = _context.LeaveApplicationForm
               .Where(a => a.Username == username && a.StartDate > mayDate && a.StartDate <= junDate)
               .Sum(a => a.DaysAppliedFor);
            var jul = _context.LeaveApplicationForm
               .Where(a => a.Username == username && a.StartDate > junDate && a.StartDate <= julDate)
               .Sum(a => a.DaysAppliedFor);
            var aug = _context.LeaveApplicationForm
               .Where(a => a.Username == username && a.StartDate > julDate && a.StartDate <= augDate)
               .Sum(a => a.DaysAppliedFor);
            var sep = _context.LeaveApplicationForm
               .Where(a => a.Username == username && a.StartDate > augDate && a.StartDate <= sepDate)
               .Sum(a => a.DaysAppliedFor);
            var oct = _context.LeaveApplicationForm
               .Where(a => a.Username == username && a.StartDate > sepDate && a.StartDate <= octDate)
               .Sum(a => a.DaysAppliedFor);
            var nov = _context.LeaveApplicationForm
               .Where(a => a.Username == username && a.StartDate > octDate && a.StartDate <= novDate)
               .Sum(a => a.DaysAppliedFor);
            var dec = _context.LeaveApplicationForm
               .Where(a => a.Username == username && a.StartDate > novDate && a.StartDate <= decDate)
               .Sum(a => a.DaysAppliedFor);

            Dictionary<string, int> daysPerMonth = new Dictionary<string, int>();
            daysPerMonth.Add("Jan", jan);
            daysPerMonth.Add("Feb", feb);
            daysPerMonth.Add("Mar", mar);
            daysPerMonth.Add("Apr", apr);
            daysPerMonth.Add("May", may);
            daysPerMonth.Add("Jun", jun);
            daysPerMonth.Add("Jul", jul);
            daysPerMonth.Add("Aug", aug);
            daysPerMonth.Add("Sep", sep);
            daysPerMonth.Add("Oct", oct);
            daysPerMonth.Add("Nov", nov);
            daysPerMonth.Add("Dec", dec);


            return daysPerMonth;


        }

        public async Task<Dictionary<string,int>> CalculateDaysPerType(string username)
        {
            var sickLeave =  _context.LeaveApplicationForm
                .Where(a=>a.Username == username&&a.LeaveType =="Sick Leave").Sum(a=>a.DaysAppliedFor);
            var studyLeave = _context.LeaveApplicationForm
                 .Where(a => a.Username == username && a.LeaveType == "Study Leave").Sum(a => a.DaysAppliedFor);
            var maternityLeave = _context.LeaveApplicationForm
               .Where(a => a.Username == username && a.LeaveType == "Maternity Leave").Sum(a => a.DaysAppliedFor);
            var annualLeave = _context.LeaveApplicationForm
         .Where(a => a.Username == username && a.LeaveType == "Annual Leave").Sum(a => a.DaysAppliedFor);
            var otherLeave = _context.LeaveApplicationForm
         .Where(a => a.Username == username && a.LeaveType == "Other").Sum(a => a.DaysAppliedFor);

            Dictionary<string, int> leaveTypes = new Dictionary<string, int>();
            leaveTypes.Add("Sick leave", sickLeave);
            leaveTypes.Add("Study leave", studyLeave);
            leaveTypes.Add("Martenity leave", maternityLeave);
            leaveTypes.Add("Annual leave", annualLeave);
            leaveTypes.Add("Other leave", otherLeave);

            return leaveTypes;

        }

        public int DifferenceBetweenDays(DateTime From, DateTime To)
        {
            return (To - From).Days;
        }
    }
}
