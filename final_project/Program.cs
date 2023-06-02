using Microsoft.EntityFrameworkCore;
using models;
using Microsoft.AspNetCore.Identity;
using final_project.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using final_project.Models.Data;
using Services;
using final_project.Services.Country;
using Microsoft.AspNetCore.Identity.UI.Services;
using final_project.Services;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;

namespace final_project
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(300);
            });
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<identityContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("myconnection")));
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<identityContext>()
                .AddDefaultUI()
                .AddRoles<IdentityRole>()
                .AddDefaultTokenProviders();
            builder.Services.AddAuthentication()
                    .AddGoogle(googleOptions =>
                    {
                        googleOptions.ClientId = "340977863358-el3qvlgmi2s30hjajlvg64p766lf96o5.apps.googleusercontent.com";
                        googleOptions.ClientSecret = "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";
                        googleOptions.CorrelationCookie.SameSite = SameSiteMode.Unspecified;
                        googleOptions.CallbackPath = PathString.FromUriComponent(new Uri("http://localhost:5265/Home/Index"));

                    });

            //builder.Services.AddAuthentication(options =>
            //{
            //    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            //})
            //.AddCookie()
            //.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
            //{
            //    options.ClientId = "605134423421-09m0vks13s9ahf1pesf5vs7i8tli7c71.apps.googleusercontent.com";
            //    options.ClientSecret = "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";
            //    options.ClaimActions.MapJsonKey("urn:google:picture","picture","url");
            //});
            //builder.Services.AddIdentityServer().AddApiAuthorization<ApplicationUser, identityContext>();
            //builder.Services.AddAuthentication()
            //    .AddIdentityServerJwt()
            //    .AddGoogle(googleOptions =>
            //    {
            //        googleOptions.ClientId = "605134423421-09m0vks13s9ahf1pesf5vs7i8tli7c71.apps.googleusercontent.com";
            //        googleOptions.ClientSecret = "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";
            //    });
            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
            });
            builder.Services.AddSingleton<IHttpContextAccessor,HttpContextAccessor>();
            builder.Services.AddScoped<Iuserservice, userservice>();
            builder.Services.AddScoped<Idoctorservice, doctorservice>();
            builder.Services.AddScoped<Idepartmentservice, departmentservice>();
            builder.Services.AddScoped<Ipatientservice, patientservice>();
            builder.Services.AddScoped<Iclinicservice, clinicservice>();
            builder.Services.AddScoped<Ireservationservice,reservationservice>();
            builder.Services.AddScoped<Ischaduleservice, schaduleservice>();
            builder.Services.AddScoped<Ifeedbackservice, feedbackservice>();
            builder.Services.AddScoped<Ireservationservice, reservationservice>();
            builder.Services.AddScoped<Icountryservice, countryservice>();
            builder.Services.AddScoped<Ititleservice,titleservice>();
            builder.Services.AddTransient<IEmailSender, EmailSender>();
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
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapRazorPages();
            app.MapControllerRoute(
                name: "area",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}",
                new string[]{ "final_project.Controllers"}
                );
            using (var scope=app.Services.CreateScope()) {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var roles = new[] { "Admin", "Patient", "Doctor" };
                foreach(var role in roles) {
                    if (!await roleManager.RoleExistsAsync(role)) {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
                
            }
            using (var scope = app.Services.CreateScope())
            {
                var userManager =
                    scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var adminUser = new ApplicationUser { UserName = "admin@gmail.com", Email = "admin@gmail.com",EmailConfirmed=true, FirstName = "admin", LastName = "admin", PhoneNumber = "01011120182", Gender=gender.female };
                var result = await userManager.CreateAsync(adminUser, "Admin@1234");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
				adminUser = new ApplicationUser { UserName = "admin1@gmail.com", Email = "admin1@gmail.com", EmailConfirmed = true, FirstName = "admin", LastName = "admin", PhoneNumber = "01063911120", Gender = gender.male };
				result = await userManager.CreateAsync(adminUser, "Admin@1234");
				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(adminUser, "Admin");
				}
				adminUser = new ApplicationUser { UserName = "admin2@gmail.com", Email = "admin2@gmail.com", EmailConfirmed = true, FirstName = "admin", LastName = "admin", PhoneNumber = "01063026492", Gender = gender.female };
				result = await userManager.CreateAsync(adminUser, "Admin@1234");
				if (result.Succeeded)
				{
					await userManager.AddToRoleAsync(adminUser, "Admin");
				}
			}
				app.Run();
        }
    }
}