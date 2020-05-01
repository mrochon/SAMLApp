using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using SAMLTEST.Options;
using SAMLTEST.SAMLObjects;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SAMLTEST.Pages
{
    /// <summary>
    /// This is the Index Page Model for the Service Provider
    /// </summary>
    public class IndexModel : PageModel
    {

        /// <summary>
        /// This Constructor is used to retrieve the Appsettings data
        /// </summary>
        public IndexModel(IOptionsMonitor<SAMLProviderOptions> options)
        {
            _options = options;
        }
        private readonly IOptionsMonitor<SAMLProviderOptions> _options;
        /// <summary>
        /// This Post Action is used to Generate the AuthN Request and redirect to the B2C Login endpoint
        /// </summary>
        public IActionResult OnPost()
        {
            var options = _options.CurrentValue;
            var tenantId = options.Tenant.ToLower()?.Replace(".onmicrosoft.com", "");
            var SamlRequest = string.Empty;
            string b2cloginurl = tenantId + ".b2clogin.com";
            var policy = options.Policy.StartsWith("B2C_1A_") ? options.Policy : "B2C_1A_" + options.Policy;
            var tenant = (options.Tenant.ToLower().Contains("onmicrosoft.com") || options.Tenant.ToLower().Contains(".net")) ? options.Tenant : options.Tenant + ".onmicrosoft.com";
            var dcInfo = string.IsNullOrWhiteSpace(options.DCInfo) ? string.Empty : "&" + options.DCInfo;
            var issuer = string.IsNullOrWhiteSpace(options.Issuer) ? SAMLHelper.GetThisURL(this) : options.Issuer;

            var RelayState = SAMLHelper.toB64(tenant) + "." + SAMLHelper.toB64(policy) + "." + SAMLHelper.toB64(issuer);

            if (!string.IsNullOrEmpty(dcInfo))
            {
                RelayState = RelayState + "." + SAMLHelper.toB64(dcInfo);
            }

            AuthnRequest AuthnReq;
            var URL = "https://" + b2cloginurl + "/" + tenant + "/" + policy + "/samlp/sso/login?" + dcInfo;
            AuthnReq = new AuthnRequest(URL, SAMLHelper.GetThisURL(this), issuer);
            var cdoc = SAMLHelper.Compress(AuthnReq.ToString());
            URL = URL + "&SAMLRequest=" + System.Web.HttpUtility.UrlEncode(cdoc) + "&RelayState=" + System.Web.HttpUtility.UrlEncode(RelayState);
            return Redirect(URL);
        }

        public IActionResult SendAzureAdRequest(string Tenant)
        {
            AuthnRequest AuthnReq;
            AuthnReq = new AuthnRequest("https://login.microsoftonline.com/00000000-0000-0000-0000-000000000000/saml2", SAMLHelper.GetThisURL(this), string.Empty);

            string cdoc = SAMLHelper.Compress(AuthnReq.ToString());
            string URL = $"https://login.microsoftonline.com/00000000-0000-0000-0000-000000000000/saml2?SAMLRequest=" + System.Web.HttpUtility.UrlEncode(cdoc);
            return Redirect(URL);
        }

    }
}
