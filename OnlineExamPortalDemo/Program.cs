using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OnlineExamPortalDemo.Data;
using OnlineExamPortalDemo.Models;

			var builder = WebApplication.CreateBuilder(args);
/*builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
});*/
/*builder.Services.AddAuthorization(options =>
{
	options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
	options.AddPolicy("UserOnly", policy => policy.RequireRole("User"));
});*/
// Add services to the container.
/*builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options =>
				{
					options.Cookie.Name = "MySessionCookie";
					options.LoginPath = "/Account/Login";
					options.SlidingExpiration = true;
				});*/

builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});
builder.Services.AddSession();

builder.Services.AddDbContext<ExamDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("ExamConnectionString")));
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddDefaultIdentity<AppUser>
	(options => options.SignIn.RequireConfirmedAccount = true)
	.AddDefaultTokenProviders()
	.AddRoles<AppRole>()
	.AddEntityFrameworkStores<ExamDbContext>();
builder.Services.AddHttpContextAccessor();

/*builder.Services.Configure<IdentityOptions>(options =>
{
	options.Password.RequireDigit = true;
	options.Password.RequireLowercase = true;
	options.Password.RequireUppercase = true;
	options.Password.RequireNonAlphanumeric = false;
	options.Password.RequiredLength = 8;
});*/
/*builder.Services.ConfigureApplicationCookie(options =>
{
	options.Cookie.HttpOnly = true;
	options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
	options.LoginPath = "/Account/Login";
});*/




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
app.UseSession();
/*var cookiePolicyOptions = new CookiePolicyOptions
{
	MinimumSameSitePolicy = SameSiteMode.Strict,
	HttpOnly = Microsoft.AspNetCore.CookiePolicy.HttpOnlyPolicy.Always,
	Secure = CookieSecurePolicy.None,
};*/

//app.UseCookiePolicy(cookiePolicyOptions);
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
