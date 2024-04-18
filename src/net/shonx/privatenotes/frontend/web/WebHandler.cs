using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web.UI;

public class WebHandler
{
    private readonly WebApplicationBuilder builder;
    private readonly WebApplication app;
    public WebHandler(string[] args)
    {
        builder = WebApplication.CreateBuilder(args);

        IEnumerable<string>? initialScopes = builder.Configuration["Notes:Scopes"]?.Split(' ');

        builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration, "AzureAd")
            .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
                .AddDownstreamApi("Notes", builder.Configuration.GetSection("Notes"))
                .AddInMemoryTokenCaches();


        
        
        // Add services to the container.
        builder.Services.AddControllersWithViews().AddMvcOptions(options =>
        {
            var policy = new AuthorizationPolicyBuilder()
                          .RequireAuthenticatedUser()
                          .Build();
            options.Filters.Add(new AuthorizeFilter(policy));
        }).AddMicrosoftIdentityUI();


        app = builder.Build();

        app.Use((context, next) =>
        {
            context.Request.Scheme = "https";
            return next();
        });


        
        app.UseStatusCodePagesWithReExecute("/Error/{0}");

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseForwardedHeaders();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{SecretName?}");

        /*   app.MapControllerRoute(
              name: "Download",
              pattern: "Document/Download/{SecretName}",
              new { controller = "Document", action = "Download" });

           app.MapControllerRoute(
              name: "Download",
              pattern: "Document/Edit/{id?}",
              new { controller = "Document", action = "Edit" });

       */
    }

    public void Run()
    {
        app.Run();
    }
}
