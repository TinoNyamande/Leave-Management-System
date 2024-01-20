using LeaveManagementSystemAPI.EmailTemplates;
using LeaveManagementSystemAPI.Models;
using LeaveManagementSystemAPI.MyServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeaveManagementSystemAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly ApplicationDbContext _context;
        private readonly ILeaveApplication _application;
        public EmailController(IEmailService emailService,ApplicationDbContext context,ILeaveApplication applcation)
        {
            _emailService = emailService;
            _context = context;
            _application = applcation;
        }
        [HttpPost("Test")]
        public async Task<IActionResult> Test ()
        {
            var to = new List<string>();
            to.Add("alicenyanyiwa0774@gmail.com");
            MailData testData = new MailData
            (
                to,
                "Test"
            );
            var res = await _emailService.SendAsync(testData, new CancellationToken());
            return Ok(res);

        }
        [HttpPost("Application")]
        public async Task<IActionResult> Application ()
        {
            var app = await _context.LeaveApplicationForm.FirstOrDefaultAsync(a => a.Status == "NEW");
            if (app == null)
            {
                return BadRequest("not found");
            }
            try
            {
                List<string> to = new();
                to.Add("alicenyanyiwa0774@gmail.com");
                var body = _application.Apply(app);
                MailData mailData = new MailData(to,"New Leave application",body);
                var res =  await _emailService.SendAsync(mailData, new CancellationToken());
                app.Status = "NEW_SENT";
                return Ok(res);
            }catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.ToString()
                });
            }
        }
        [HttpPost("Approve")]
        public async Task<IActionResult> Approve()
        {
            var app = await _context.LeaveApplicationForm.FirstOrDefaultAsync(a => a.Status == "APPROVED");
            if (app == null)
            {
                return BadRequest("not found");
            }
            try
            {
                List<string> to = new();
                to.Add("alicenyanyiwa0774@gmail.com");
                var body = _application.Approve(app);
                MailData mailData = new MailData(to, "Leave Application Approval", body);
                await _emailService.SendAsync(mailData, new CancellationToken());
                app.Status = "APPROVE_SENT";
                return Ok("Sent");
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.ToString()
                });
            }
        }
        [HttpPost("Reject")]
        public async Task<IActionResult> Reject()
        {
            var app = await _context.LeaveApplicationForm.FirstOrDefaultAsync(a => a.Status == "REJECTED");
            if (app == null)
            {
                return BadRequest("not found");
            }
            try
            {
                List<string> to = new();
                to.Add("alicenyanyiwa0774@gmail.com");
                var body = _application.Reject(app);
                MailData mailData = new MailData(to, "Leave application rejected", body);
                await _emailService.SendAsync(mailData, new CancellationToken());
                app.Status = "REJECTED_SENT";
                return Ok("Sent");
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.ToString()
                });
            }
        }
    }
}
