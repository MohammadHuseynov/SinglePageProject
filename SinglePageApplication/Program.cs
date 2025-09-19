using Microsoft.EntityFrameworkCore;
using SinglePageApplication.ApplicationServices.Services;
using SinglePageApplication.ApplicationServices.Services.Contracts;
using SinglePageApplication.Models;
using SinglePageApplication.Models.Services.Contracts;
using SinglePageApplication.Models.Services.Repositories;


#region [- Building the app object -]
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<SinglePageApplicationDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("Default")
));


builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.AddScoped<IProductApplicationService, ProductApplicationService>();




var app = builder.Build();

#endregion






#region [- adding middlewares in the road of running -]

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
#endregion

app.Run();
