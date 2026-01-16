using Dfe.ManageSchoolImprovement.Frontend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Dfe.ManageSchoolImprovement.Frontend.Pages.Public;

[AllowAnonymous]
public class CookiePreferences(ILogger<CookiePreferences> logger) : PageModel
{
    private readonly string[] CONSENT_COOKIE_NAMES = [".ManageSchoolImprovement.Consent"];
    public bool? Consent { get; set; }
    public bool PreferencesSet { get; set; }
    public string ReturnPath { get; set; }

    public ActionResult OnGet(bool? consent, string returnUrl)
    {
        ReturnPath = returnUrl;

        if (Request.Cookies.ContainsKey(CONSENT_COOKIE_NAMES[0]))
        {
            Consent = bool.Parse(Request.Cookies[CONSENT_COOKIE_NAMES[0]] ?? string.Empty);
        }

        if (consent.HasValue)
        {
            PreferencesSet = true;
            ApplyCookieConsent(consent);

            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToPage(Links.Public.CookiePreferences);
        }

        return Page();
    }

    public IActionResult OnPost(bool? consent, string returnUrl)
    {
        ReturnPath = returnUrl;

        if (Request.Cookies.ContainsKey(CONSENT_COOKIE_NAMES[0]))
        {
            Consent = bool.Parse(Request.Cookies[CONSENT_COOKIE_NAMES[0]] ?? string.Empty);
        }

        if (consent.HasValue)
        {
            Consent = consent;
            PreferencesSet = true;

            AppendCookies(consent);

            if (!consent.Value)
            {
                ApplyCookieConsent(false);
            }

            return Page();
        }

        return Page();
    }

    private void AppendCookies(bool? consent)
    {
        foreach (var CONSENT_COOKIE_NAME in CONSENT_COOKIE_NAMES)
        {
            CookieOptions cookieOptions = new() { Expires = DateTime.Today.AddMonths(6), Secure = true, HttpOnly = true, SameSite = SameSiteMode.Lax };
            Response.Cookies.Append(CONSENT_COOKIE_NAME, consent.Value.ToString(), cookieOptions);
        }
    }

    private void ApplyCookieConsent(bool? consent)
    {
        if (consent.HasValue)
        {
            AppendCookies(consent);
        }

        if (consent is false)
        {
            foreach (string cookie in Request.Cookies.Keys)
            {
                // Google Analytics
                if (cookie.StartsWith("_ga") || cookie.Equals("_gid"))
                {
                    logger.LogInformation("Expiring Google analytics cookie: {Cookie}", cookie);
                    Response.Cookies.Append(cookie, string.Empty, new CookieOptions { Expires = DateTime.Now.AddDays(-1), Secure = true, SameSite = SameSiteMode.Lax, HttpOnly = true });
                }
                // App Insights
                if (cookie.StartsWith("ai_"))
                {
                    logger.LogInformation("Expiring App insights cookie: {Cookie}", cookie);
                    Response.Cookies.Append(cookie, string.Empty, new CookieOptions { Expires = DateTime.Now.AddYears(-1), Secure = true, SameSite = SameSiteMode.Lax, HttpOnly = true });
                }
            }
        }
    }
}
