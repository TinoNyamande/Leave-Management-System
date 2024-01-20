using LeaveManagementSystemAPI.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using LeaveManagementSystemAPI.Models;
using Microsoft.Data.SqlClient;
using LeaveManagementSystemAPI.Configuration;
using LeaveManagementSystemAPI.MyServices;
using LeaveManagementSystemAPI.EmailTemplates;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//   .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("My name is Tinotenda Nyamande and i am tall ")),
        ValidateAudience = false,
        ValidateIssuer = false

    };
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("ConnStr")));

builder.Services.AddIdentity<ApplicationUser ,IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();        
builder.Services.Configure<IdentityOptions>(options => options.SignIn.RequireConfirmedEmail = true);
builder.Services.AddCors(options =>
{
     options.AddPolicy("myAppCors", policy =>
     {
         policy.AllowAnyHeader()
         .AllowAnyOrigin()
         .AllowAnyMethod();
     });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));
builder.Services.AddSingleton<IMailService, MailService>();
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddScoped<IGetApplicationBy, GetApplicationBy>();
builder.Services.AddScoped<ILeaveApplication, LeaveApplication>();
builder.Services.AddScoped<ICalculateLeaveDays, CalculateLeaveDays>();

var app = builder.Build();
app.UseCors("myAppCors");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
