using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Data.SqlClient;
using Rotativa.AspNetCore;
using SubjectRegister.BLL.Implements;
using SubjectRegister.BLL.Interfaces;
using SubjectRegister.Common.HostedService;
using SubjectRegister.Comon;
using SubjectRegister.Controllers.Validator;
using SubjectRegister.DAL;
using SubjectRegister.Services;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

//Đăng ký session
builder.Services.AddHttpContextAccessor(); // Đăng ký IHttpContextAccessor
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); //Session tồn tại trong 30p
    options.Cookie.HttpOnly = true; //Giúp bảo mật session và cookie
    options.Cookie.IsEssential = true; //Cookie luôn gửi khi request
});

// Cấu hình Authentication và Authorization
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", config =>
    {
        config.Cookie.Name = "UserLoginCookie"; // Tên cookie xác thực
        config.LoginPath = "/Login/Auth";   // Dành cho user chưa đăng nhập
        config.AccessDeniedPath = "/Login/AccessDenied"; // Dành cho user đã đăng nhập nhưng không có quyền
    });

builder.Services.AddAuthorization(); // Thêm dịch vụ phân quyền

// Add services to the container.
builder.Services.AddControllersWithViews();
// Add Validation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

builder.Services.AddMemoryCache();
builder.Services.AddSignalR();
//Đăng ký IDbConnection để sử dụng dapper
builder.Services.AddScoped<IDbConnection>(db => new
SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHostedService<NotificationService>();
builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

//Đăng ký lớp để quản lý file
builder.Services.AddSingleton<FileManagement>();

//Đăng ký lớp để tiêm giữa các tầng
builder.Services.AddScoped<ILoginBL, LoginBL>();
builder.Services.AddScoped<LoginDAO>();
builder.Services.AddScoped<IStudentBL, StudentBL>();
builder.Services.AddScoped<StudentDAO>();
builder.Services.AddScoped<ITeacherBL, TeacherBL>();
builder.Services.AddScoped<TeacherDAO>();
builder.Services.AddScoped<ISemesterBL, SemesterBL>();
builder.Services.AddScoped<SemesterDAO>();
builder.Services.AddScoped<ISubjectBL, SubjectBL>();
builder.Services.AddScoped<SubjectDAO>();
builder.Services.AddScoped<IPlannedSubjectBL, PlannedSubjectBL>();
builder.Services.AddScoped<PlannedSubjectDAO>();
builder.Services.AddScoped<IStudyPlanBL, StudyPlanBL>();
builder.Services.AddScoped<StudyPlanDAO>();
builder.Services.AddScoped<IClassSectionBL, ClassSectionBL>();
builder.Services.AddScoped<ClassSectionDAO>();
builder.Services.AddScoped<IRegisterClassBL, RegisterClassBL>();
builder.Services.AddScoped<RegisterClassDAO>();

//
builder.Services.AddScoped<ISetTimePlanSchedule, SetTime>();
builder.Services.AddScoped<ISetTimeRegisterClassSection, SetTime>();

var mailsettings = builder.Configuration.GetSection("MailSettings");

//Đăng ký lớp để khởi tạo các lưu trữ cần thiết khi bắt đầu chạy hệ thống
builder.Services.AddHostedService<StartupService>();

var app = builder.Build();

// Cấu hình Rotativa
RotativaConfiguration.Setup(app.Environment.WebRootPath, "wkhtmltopdf");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession(); // Đảm bảo gọi trước các middleware khác liên quan đến xác thực   

app.UseRouting();

app.UseAuthentication(); // Phải trước `UseAuthorization`
app.UseAuthorization();
app.MapHub<NotificationHub>("/notificationHub");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Auth}/{id?}");

app.Run();
