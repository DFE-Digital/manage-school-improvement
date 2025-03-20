using Dfe.ManageSchoolImprovement.Frontend.Authorization;
using Dfe.ManageSchoolImprovement.Frontend.Configuration;
using Dfe.ManageSchoolImprovement.Frontend.Models;
using Dfe.ManageSchoolImprovement.Frontend.Security;
using Dfe.ManageSchoolImprovement.Frontend.Services;
using Dfe.ManageSchoolImprovement.Frontend.Services.AzureAd;
using Dfe.ManageSchoolImprovement.Frontend.Services.Http;
using DfE.CoreLibs.Http.Interfaces;
using DfE.CoreLibs.Http.Middlewares.CorrelationId;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Identity.Web;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add User Secrets in Development
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

var config = builder.Configuration;

var _authenticationExpiration = TimeSpan.FromMinutes(int.Parse(config["AuthenticationExpirationInMinutes"] ?? "60"));

builder.Services.Configure<AzureAdOptions>(config.GetSection("AzureAd"));

builder.Services.AddHttpClient(DfeHttpClientFactory.AcademiesClientName, (sp, client) =>
{
    var academiesApiSection = config.GetSection("AcademiesApi");
    client.BaseAddress = new Uri(academiesApiSection["Url"]);
    client.DefaultRequestHeaders.Add("ApiKey", academiesApiSection["ApiKey"]);
    client.DefaultRequestHeaders.Add("User-Agent", "ManageSchoolImprovement/1.0");

});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = new TimeSpan(10000);
    options.Cookie.Name = ".ManageSchoolImprovement.Session";
    options.Cookie.IsEssential = true;
    options.Cookie.HttpOnly = true;

    if (string.IsNullOrWhiteSpace(config["CI"]))
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.AddScoped(sp => sp.GetService<IHttpContextAccessor>()?.HttpContext?.Session);
// Add services to the container.
builder.Services.AddRazorPages(options =>
        {
            options.Conventions.AuthorizeFolder("/");
            options.Conventions.AllowAnonymousToPage("/public/maintenance");
            options.Conventions.AllowAnonymousToPage("/public/accessibilitystatement");
            options.Conventions.AllowAnonymousToPage("/public/cookiepreferences");
            options.Conventions.AllowAnonymousToPage("/public/privacy");
            options.Conventions.AllowAnonymousToPage("/public/privacy1");
            options.Conventions.AllowAnonymousToPage("/AccessDenied");
        })
        .AddViewOptions(options =>
        {
            options.HtmlHelperOptions.ClientValidationEnabled = false;
        }).AddMvcOptions(options =>
        {
            options.MaxModelValidationErrors = 50;
        });

builder.Services.AddMicrosoftIdentityWebAppAuthentication(config);
builder.Services.AddAuthorization(options => { options.DefaultPolicy = SetupAuthorizationPolicyBuilder().Build(); });

AuthorizationPolicyBuilder SetupAuthorizationPolicyBuilder()
{
    AuthorizationPolicyBuilder policyBuilder = new();
    policyBuilder.RequireAuthenticatedUser();

    string allowedRoles = config.GetSection("AzureAd")["AllowedRoles"]!;
    if (!string.IsNullOrWhiteSpace(allowedRoles))
    {
        policyBuilder.RequireClaim(ClaimTypes.Role, allowedRoles.Split(','));
    }

    return policyBuilder;
}

// Enforce HTTPS in ASP.NET Core
// @link https://learn.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?
builder.Services.AddHsts(options =>
{
    options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(365);
});

builder.Services.Configure<CookieAuthenticationOptions>(CookieAuthenticationDefaults.AuthenticationScheme,
   options =>
   {
       options.AccessDeniedPath = "/access-denied";
       options.Cookie.Name = ".ManageSchoolImprovement.Login";
       options.Cookie.HttpOnly = true;
       options.Cookie.IsEssential = true;
       options.ExpireTimeSpan = _authenticationExpiration;
       options.SlidingExpiration = true;

       if (string.IsNullOrEmpty(config["CI"]))
           options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
   });

builder.Services.Configure<ApplicationInsightsOptions>(config.GetSection("ApplicationInsights"));
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ErrorService>();
builder.Services.AddScoped<IDfeHttpClientFactory, DfeHttpClientFactory>();
builder.Services.AddScoped<IGetEstablishment, EstablishmentService>();
builder.Services.Decorate<IGetEstablishment, GetEstablishmentItemCacheDecorator>();
builder.Services.AddScoped<ICorrelationContext, CorrelationContext>();
builder.Services.AddScoped<IHttpClientService, HttpClientService>();
builder.Services.AddScoped<IGraphClientFactory, GraphClientFactory>();
builder.Services.AddScoped<IGraphUserService, GraphUserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddSingleton<IAuthorizationHandler, HeaderRequirementHandler>();
builder.Services.AddSingleton<IAuthorizationHandler, ClaimsRequirementHandler>();

builder.Services.AddApplicationDependencyGroup(builder.Configuration);
builder.Services.AddInfrastructureDependencyGroup(builder.Configuration);

var app = builder.Build();

var forwardOptions = new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.All,
    RequireHeaderSymmetry = false
};
forwardOptions.KnownNetworks.Clear();
forwardOptions.KnownProxies.Clear();
app.UseForwardedHeaders(forwardOptions);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Errors");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}
app.UseStatusCodePagesWithReExecute("/Errors", "?statusCode={0}");

app.UseSecurityHeaders(SecurityHeadersDefinitions.GetHeaderPolicyCollection(app.Environment.IsDevelopment()));
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<CorrelationIdMiddleware>();
app.UseCookiePolicy(new CookiePolicyOptions { Secure = CookieSecurePolicy.Always, HttpOnly = HttpOnlyPolicy.Always });

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", context =>
    {
        context.Response.Redirect("schools-identified-for-targeted-intervention", false);
        return Task.CompletedTask;
    });
    endpoints.MapRazorPages();
    endpoints.MapControllerRoute("default", "{controller}/{action}/");
});

app.UseHealthChecks("/health");

await app.RunAsync();

public partial class Program { } // Make the Program class partial for testing