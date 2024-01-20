using LeaveManagementSystemAPI.Models;
using LeaveManagementSystemAPI.MyServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ICalculateLeaveDays _calculateLeaveDays;
        public ValuesController(ICalculateLeaveDays calculateLeaveDays,ApplicationDbContext context)
        {
            _calculateLeaveDays = calculateLeaveDays;
            _context = context;
        }
        [HttpGet("GetDays")]
        public async Task<IActionResult> GetDays(string username)
        {
            var userExists = await _context.LeaveApplicationForm.FirstOrDefaultAsync(a => a.Username == username);
            return Ok(userExists);

        }
    }
}
