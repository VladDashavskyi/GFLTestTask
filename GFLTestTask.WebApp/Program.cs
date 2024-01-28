using GFLTestTask.Bll.Implementation;
using GFLTestTask.Bll.Interfaces;
using GFLTestTask.Dal.Context;
using GFLTestTask.Dal.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationContext>(options =>
                                                           options.UseSqlServer(builder.Configuration
                                                                                   .GetConnectionString("WebApiDatabase")), ServiceLifetime.Transient);
builder.Services.AddScoped<IApplicationContext, ApplicationContext>();
builder.Services.AddScoped<ITaskServices, TaskServices>();


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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
