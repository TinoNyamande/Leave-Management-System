using LeaveManagementSystemAPI.Models;
using LeaveManagementSystemAPI.Models.ViewModels;
using LeaveManagementSystemAPI.MyServices;
using MailKit.Net.Imap;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web.Resource;
using System;

namespace LeaveManagementSystemAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [RequiredScope(RequiredScopesConfigurationKey = "AzureAd:Scopes")]
    public class LeaveApplication : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IGetApplicationBy _getApplicationBy;
        private readonly ICalculateLeaveDays _calculateLeaveDays;

        public LeaveApplication(ICalculateLeaveDays calculateLeaveDays, ApplicationDbContext context, IGetApplicationBy getApplicationBy
,IConfiguration configuration,IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
            _getApplicationBy = getApplicationBy;
            _calculateLeaveDays = calculateLeaveDays;

        }
        [HttpPost("LeaveApplications")]
        public async Task<IActionResult> LeaveApplications(LeaveApplicationViewModel leaveApplicationVM)
        {
            if (leaveApplicationVM == null)
            {
                return BadRequest(new
                {
                    message = "Invalid request"
                });
            }
         
            
            //product.Id = Guid.NewGuid().ToString();
            LeaveApplicationForm leaveApplicationForm = new LeaveApplicationForm
            {
                Id = Guid.NewGuid().ToString(),
                LeaveType = leaveApplicationVM.LeaveType,
                StartDate = leaveApplicationVM.StartDate,
                EndDate = leaveApplicationVM.EndDate,
                Description = leaveApplicationVM.Description,
                Username = leaveApplicationVM.Username,
                Status = "NEW",
                ApplicationDate = DateTime.Now,
                DaysAppliedFor = leaveApplicationVM.DaysAppliedFor
                

        };
            await _context.AddAsync(leaveApplicationForm);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "Application saved successfully"
            });
        }
        [HttpGet("GetApplicationByUsername")]
        public async Task<IActionResult> GetApplicationByUserName (string Username)
        {
            var applications = await _context.LeaveApplicationForm.Where(a => a.Username == Username).ToListAsync();
            if (applications == null)
            {
                return BadRequest(new
                {
                    message = "Application not found"
                }) ;
            }
            return Ok(applications);
        }
        [HttpGet("GetApplicationById")]
        public async Task<IActionResult> GetApplicationById(string Id)
        {
            var application = await _context.LeaveApplicationForm.FirstOrDefaultAsync(a => a.Id == Id);
            if (application == null)
            {
                return BadRequest(new
                {
                    message = "Application not found"
                });
            }

            GetApplication leaveApplicationVM = new GetApplication
            {
                Username = application.Username,
                StartDate = application.StartDate,
                EndDate = application.EndDate,
                LeaveType = application.LeaveType,
                Description = application.Description,
                Status = application.Status
            };
            return Ok(leaveApplicationVM);
        }
        //[Authorize]
        [HttpGet("GetAllNewApplications")]
        public async Task<IActionResult> GetAllNewApplications()
        {
            var applications = await _context.LeaveApplicationForm.Where(a=>a.Status == "NEW").ToListAsync();
            if (applications == null)
            {
                return BadRequest(new
                {
                    message = "No new applications"
                });
            }
            return Ok(applications);
        }

        [HttpGet("GetAllApplications")]
        public async Task<IActionResult> GetAllApplications()
        {
            var applications = await _context.LeaveApplicationForm.ToListAsync();
            if (applications == null)
            {
                return BadRequest(new
                {
                    message = "No applications found"
                });
            }
            return Ok(applications);
        }

        [HttpPost("ApproveApplication")]
        public async Task<IActionResult> ApproveApplication(String applicationId)
        {
            var application = _context.LeaveApplicationForm.SingleOrDefault(a => a.Id == applicationId);
            if (application == null)
            {
                return BadRequest(new
                {
                    message = "Application not found in database"
                });
            }
            application.Status = "APPROVED";
            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "Leave Application has been approved"
            });
        }

        [HttpPost("RejectApplication")]
        public async Task<IActionResult> RejectApplication(String applicationId)
        {
            var application = _context.LeaveApplicationForm.SingleOrDefault(a => a.Id == applicationId);
            if (application == null)
            {
                return BadRequest(new
                {
                    message = "Application not found in database"
                });
            }
            application.Status = "REJECTED";
            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "Leave Application has been rejected"
            });
        }
        [HttpGet("GetDays")]
        public async Task<IActionResult> GetDays(string username)
        {
            var currentDate = DateTime.Now;
            var date = new DateTime(currentDate.Year, 1, 31);

            var application =  _context.LeaveApplicationForm
                .Where(a => a.Username == username && a.StartDate <= date)
                .Sum(a=>a.DaysAppliedFor);

            return Ok(application);



        }
        [HttpGet("GetDaysPerMonth")]
        public async Task<IActionResult> GetDaysPerMonth(string username)
        {

            var userExists = await _context.LeaveApplicationForm.FirstOrDefaultAsync(a => a.Username == username);
            if (userExists == null)
            {
                return BadRequest(new
                {
                    message = "No applications by this user"
                });
            }
            try
            {
                var daysPerMonth = await _calculateLeaveDays.CalculateDaysPerMonth(username);
                var daysPerType = await _calculateLeaveDays.CalculateDaysPerType(username);
                var res = new
                {
                    DaysPerMonth = daysPerMonth,
                    DaysPerType = daysPerType
                };
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.ToString()
                });
            }
        }
        [HttpGet("GetApplicationBy")]
        public async Task<IActionResult> GetApplicationBy(string? Id, string? Status, string? Username, string? LeaveType)
        {
            try
            {
                if (Id != null)
                {
                    var applications = await _context.LeaveApplicationForm.Where(a => a.Id == Id).ToListAsync();
                    return Ok(applications);
                }
                else if (Status != null && Username != null && LeaveType != null)
                {
                    var applications = await _context.LeaveApplicationForm.Where(a => a.Status == Status)
                                                                          .Where(a => a.Username == Username)
                                                                          .Where(a => a.LeaveType == LeaveType)
                                                                          .ToListAsync();
                    return Ok(applications);
                }
                else if (Status != null && Username != null && LeaveType == null)
                {
                    var applications = await _context.LeaveApplicationForm.Where(a => a.Status == Status)
                                                          .Where(a => a.Username == Username)
                                                          .ToListAsync();
                    return Ok(applications);
                }
                else if (Status != null && Username == null && LeaveType != null)
                {
                    var applications = await _context.LeaveApplicationForm.Where(a => a.Status == Status)
                                                          .Where(a => a.LeaveType == LeaveType)
                                                          .ToListAsync();
                    return Ok(applications);
                }
                else if (Status == null && Username != null && LeaveType != null)
                {
                    var applications = await _context.LeaveApplicationForm.Where(a => a.Username == Username)
                                                          .Where(a => a.LeaveType == LeaveType)
                                                          .ToListAsync();
                    return Ok(applications);
                }
                else if (Status == null && Username == null && LeaveType != null)
                {
                    var applications = await _context.LeaveApplicationForm
                                                          .Where(a => a.LeaveType == LeaveType)
                                                          .ToListAsync();
                    return Ok(applications);
                }
                else if (Status == null && Username != null && LeaveType == null)
                {
                    var applications = await _context.LeaveApplicationForm
                                                          .Where(a => a.Username == Username)
                                                          .ToListAsync();
                    return Ok(applications);
                }
                else if (Status != null && Username == null && LeaveType == null)
                {
                    var applications = await _context.LeaveApplicationForm
                                                          .Where(a => a.Status == Status)
                                                          .ToListAsync();
                    return Ok(applications);
                }
                else
                {
                    var apps = await _context.LeaveApplicationForm.ToListAsync();
                    return Ok(apps);
                }
              
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }
 
    }
}
