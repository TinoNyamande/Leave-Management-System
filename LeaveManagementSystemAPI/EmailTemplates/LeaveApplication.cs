using LeaveManagementSystemAPI.Models;
using LeaveManagementSystemAPI.Models.ViewModels;

namespace LeaveManagementSystemAPI.EmailTemplates
{
    public class LeaveApplication : ILeaveApplication
    {
        public string Apply(LeaveApplicationForm application)
        {
            return $@"<!DOCTYPE html>
            <html lang=""en"">
  <head>
    <meta charset=""UTF-8"" />
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
  </head>
  <style>
    .email-container {{
      background-color: azure;
      width: 90%;
      height: 100vh;
      margin-left: auto;
      margin-right: auto;
      padding: 5%;
      font-family: Century Gothic !important;
    }}
    .email-salutation {{
      margin-top: 2%;
    }}
    .email-closing {{
      margin-top: 2%;
    }}
    .signature-container {{
      font-family: Century Gothic !important;
      font-size: 62.5% !important;
      width: 70% !important;
    }} 

  .sig-name {{
    font-size: 150%;
  }}
  .sig-email-web {{
    font-size: 120%;
  }}
  .sig-tel-mobile {{
    font-size: 120%;
  }}
  .base-image {{
    width: 50%;
    height: 5vh;
  }}
  </style>
  <body>
    <div class=""email-container"">
      <div class=""email-salutation"">
        <h2>Dear Name</h2>
      </div>
      <div class=""email-body"">
        <p>
          You have a new leave application from {application.Username}. 
        </p>
        <p>The details of the leave are :</p>
        <p>Start Date {application.StartDate}</p>
        <p>End Date {application.EndDate}</p>
        <p>Total working days {application.LeaveType}</p>
        <p>Leave Type {application.LeaveType}</p>
        <p>Click <a>here</a> to approve or reject</p>
      </div>
      <div class=""email-closing"">
        <p>Yours Sincerely</p>
      </div>
      <div>
        <br />
        <div class=""signature-container"">
          <div class=""sig-name"">
            <p>Tinotenda Nyamande</p>
          </div>
          <div class=""sig-email-web"">
            <p>email :email@gmail.com | web :myweb.com</p>
          </div>
          <div class=""sig-tel-mobile"">
            <p>tel : +263 XXX XXX | mobile +242 XXX XXX</p>
          </div>
        </div>
        <div>
          <img
            class=""base-image""
            src=""C:/Users/tnyamande/Downloads/Untitled design1 (1).jpg""
          />
        </div>
      </div>
    </div>
  </body>
</html>
";
        }

        public string Approve(LeaveApplicationForm application)
        {
            return $@"
<!DOCTYPE html>
<html lang=""en"">
  <head>
    <meta charset=""UTF-8"" />
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
  </head>
  <style>
    .email-container {{
      background-color: azure;
      width: 90%;
      height: 100vh;
      margin-left: auto;
      margin-right: auto;
      padding: 5%;
      font-family: Century Gothic !important;
    }}
    .email-salutation {{
      margin-top: 2%;
    }}
    .email-closing {{
      margin-top: 2%;
    }}
    .signature-container {{
      font-family: Century Gothic !important;
      font-size: 62.5% !important;
      width: 70% !important;
    }} 

  .sig-name {{
    font-size: 150%;
  }}
  .sig-email-web {{
    font-size: 120%;
  }}
  .sig-tel-mobile {{
    font-size: 120%;
  }}
  .base-image {{
    width: 50%;
    height: 5vh;
  }}
  </style>
  <body>
    <div class=""email-container"">
      <div class=""email-salutation"">
        <h2>Dear {application.Username}</h2>
      </div>
      <div class=""email-body"">
        <p>
          Your application for  {application.LeaveType} has been approved
        </p>
        <p>The details of the leave are :</p>
        <p>Start Date {application.StartDate}</p>
        <p>End Date {application.EndDate}</p>
        <p>Total working days {application.LeaveType}</p>
        <p>Click <a>here</a> for details</p>
      </div>
      <div class=""email-closing"">
        <p>Yours Sincerely</p>
      </div>
      <div>
        <br />
        <div class=""signature-container"">
          <div class=""sig-name"">
            <p>Tinotenda Nyamande</p>
          </div>
          <div class=""sig-email-web"">
            <p>email :email@gmail.com | web :myweb.com</p>
          </div>
          <div class=""sig-tel-mobile"">
            <p>tel : +263 XXX XXX | mobile +242 XXX XXX</p>
          </div>
        </div>
        <div>
          <img
            class=""base-image""
            src=""C:/Users/tnyamande/Downloads/Untitled design1 (1).jpg""
          />
        </div>
      </div>
    </div>
  </body>
</html>
";
        }

        public string Reject(LeaveApplicationForm application)
        {
            return $@"
<!DOCTYPE html>
<html lang=""en"">
  <head>
    <meta charset=""UTF-8"" />
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"" />
  </head>
  <style>
    .email-container {{
      background-color: azure;
      width: 90%;
      height: 100vh;
      margin-left: auto;
      margin-right: auto;
      padding: 5%;
      font-family: Century Gothic !important;
    }}
    .email-salutation {{
      margin-top: 2%;
    }}
    .email-closing {{
      margin-top: 2%;
    }}
    .signature-container {{
      font-family: Century Gothic !important;
      font-size: 62.5% !important;
      width: 70% !important;
    }} 

  .sig-name {{
    font-size: 150%;
  }}
  .sig-email-web {{
    font-size: 120%;
  }}
  .sig-tel-mobile {{
    font-size: 120%;
  }}
  .base-image {{
    width: 50%;
    height: 5vh;
  }}
  </style>
  <body>
    <div class=""email-container"">
      <div class=""email-salutation"">
        <h2>Dear {application.Username}</h2>
      </div>
      <div class=""email-body"">
        <p>
          Your application for  {application.LeaveType} has been rejected
        </p>
        <p>The details of the leave are :</p>
        <p>Start Date {application.StartDate}</p>
        <p>End Date {application.EndDate}</p>
        <p>Total working days {application.LeaveType}</p>
        <p>Click <a>here</a> for details</p>
      </div>
      <div class=""email-closing"">
        <p>Yours Sincerely</p>
      </div>
      <div>
        <br />
        <div class=""signature-container"">
          <div class=""sig-name"">
            <p>Tinotenda Nyamande</p>
          </div>
          <div class=""sig-email-web"">
            <p>email :email@gmail.com | web :myweb.com</p>
          </div>
          <div class=""sig-tel-mobile"">
            <p>tel : +263 XXX XXX | mobile +242 XXX XXX</p>
          </div>
        </div>
        <div>
          <img
            class=""base-image""
            src=""C:/Users/tnyamande/Downloads/Untitled design1 (1).jpg""
          />
        </div>
      </div>
    </div>
  </body>
</html>
";
        }
    }
}
