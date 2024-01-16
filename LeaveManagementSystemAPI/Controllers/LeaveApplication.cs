using LeaveManagementSystemAPI.Models;
using LeaveManagementSystemAPI.Models.ViewModels;
using LeaveManagementSystemAPI.MyServices;
using MailKit.Net.Imap;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web.Resource;

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

        public LeaveApplication(ApplicationDbContext context, IGetApplicationBy getApplicationBy
,IConfiguration configuration,IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
            _getApplicationBy = getApplicationBy;

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
            string wwwRootPath = _hostEnvironment.WebRootPath;
            string fileName = Path.GetFileNameWithoutExtension(leaveApplicationVM.Myfile.FileName);
            string extension = Path.GetExtension(leaveApplicationVM.Myfile.FileName);
            var myFileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            string path = Path.Combine(wwwRootPath + "/Image/", myFileName);
            using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await leaveApplicationVM.Myfile.CopyToAsync(fileStream);
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
                FilePath = myFileName

        };
            _context.Add(leaveApplicationForm);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                message = "Application saved successfully"
            });
        }
        [HttpGet("GetApplicationByUsername")]
        public async Task<IActionResult> GetApplicationByUserName (string Username)
        {
            var applications = _context.LeaveApplicationForm.Where(a => a.Username == Username).ToList();
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
            var application = _context.LeaveApplicationForm.FirstOrDefault(a => a.Id == Id);
            if (application == null)
            {
                return BadRequest(new
                {
                    message = "Application not found"
                });
            }
            string wwwRootPath = _hostEnvironment.WebRootPath;

            GetApplication leaveApplicationVM = new GetApplication
            {
                Username = application.Username,
                StartDate = application.StartDate,
                EndDate = application.EndDate,
                LeaveType = application.LeaveType,
                Description = application.Description,
                FileName = application.FilePath,
                Myfile = System.IO.File.ReadAllBytes(Path.Combine(wwwRootPath + "/Image/", application.FilePath)),
                Status = application.Status
            };
            return Ok(leaveApplicationVM);
        }
        [HttpGet("GetAllNewApplications")]
        public async Task<IActionResult> GetAllNewApplications()
        {
            var applications = _context.LeaveApplicationForm.Where(a=>a.Status == "NEW").ToList();
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
            var applications = _context.LeaveApplicationForm.ToList();
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
