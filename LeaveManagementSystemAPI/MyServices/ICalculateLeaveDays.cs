namespace LeaveManagementSystemAPI.MyServices
{
    public interface ICalculateLeaveDays
    {
        public Task<Dictionary<string,int>> CalculateDaysPerMonth(string username);
        public Task<Dictionary<string,int>> CalculateDaysPerType(string username);
        public int DifferenceBetweenDays(DateTime From, DateTime To);
    }
}
