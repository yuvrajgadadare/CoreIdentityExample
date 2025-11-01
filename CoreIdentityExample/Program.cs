//using CoreIdentityExample.Services;
using ERP_Models;
using ERP_Services.Implementations;
using ERP_Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
//builder.Services.AddTransient<IExtraService, ExtraService>();
//builder.Services.AddTransient<ITopicService, TopicService>();
//builder.Services.AddTransient<IContentService, ContentService>();
//builder.Services.AddTransient<ICourseService, CourseService>();
//builder.Services.AddTransient<IContentQuestionService, ContentQuestionService>();
var connectionString = builder.Configuration.GetConnectionString("SQLServerIdentityConnection") ?? throw new InvalidOperationException("Connection string 'SQLServerIdentityConnection' not found.");
//builder.Services.AddDbContext<CiitExamPortalContext>(options =>
//    options.UseSqlServer(connectionString));
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
});
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(
               options =>
               {
                   // Password settings
                   options.Password.RequireDigit = true;
                   options.Password.RequiredLength = 8;
                   options.Password.RequireNonAlphanumeric = true;
                   options.Password.RequireUppercase = true;
                   options.Password.RequireLowercase = true;
                   options.Password.RequiredUniqueChars = 4;
                   // Other settings can be configured here
               })
               .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddTransient<IMasterService, MasterService>();
builder.Services.AddTransient<ICourseService, CourseService>();
builder.Services.AddTransient<ITopicService, TopicService>();
builder.Services.AddTransient<IContentService, ContentService>();
builder.Services.AddTransient<IBatchService, BatchService>();
builder.Services.AddTransient<IBranchService, BranchService>();

builder.Services.AddTransient<IEmployeeService, EmployeeService>();
builder.Services.AddTransient<IEnquiryService, EnquiryService>();
builder.Services.AddTransient<IExtraService, ExtraService>();
builder.Services.AddTransient<IInvoiceService, InvoiceService>();
builder.Services.AddTransient<IJobServices, JobServices>();
builder.Services.AddTransient<IQuestionService, QuestionService>();
builder.Services.AddTransient<IStudentService, StudentService>();
builder.Services.AddTransient<ITrainerService, TrainerService>();
builder.Services.AddTransient<IExamService, ExamService>();

builder.Services.ConfigureApplicationCookie(options =>
{
    // If the LoginPath isn't set, ASP.NET Core defaults the path to /Account/Login.
    options.LoginPath = "/Account/Login"; // Set your login path here
    // If the AccessDenied isn't set, ASP.NET Core defaults the path to /Account/AccessDenied
    options.AccessDeniedPath = "/Account/AccessDenied"; // Set your access denied path here
});
var app = builder.Build();

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
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "MyArea",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Student}/{action=Login}/{id?}");
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.MapAreaControllerRoute(
//     name: "Developer",
//     areaName: "Developer",
//      pattern: "{area:Exists}/{controller:Dashboard}/{action=Index}/{id?}"
//    );
//app.MapAreaControllerRoute(
//     name: "Accountant",
//     areaName: "Accountant",
//      pattern: "{area:Exists}/{controller:Dashboard}/{action=Index}/{id?}"
//    );
//app.MapAreaControllerRoute(
//     name: "BatchManagement",
//     areaName: "BatchManagement",
//      pattern: "{area:Exists}/{controller:Dashboard}/{action=Index}/{id?}"
//    );
//app.MapAreaControllerRoute(
//     name:"Master",
//     areaName:"Master",
//      pattern: "{area:Exists}/{controller=Dashboard}/{action=Index}/{id?}"
//    );
//app.MapAreaControllerRoute(
//     name: "Student",
//     areaName: "Student",
//      pattern: "{area:Exists}/{controller=Dashboard}/{action=Index}/{id?}"
//    );
app.Run();
