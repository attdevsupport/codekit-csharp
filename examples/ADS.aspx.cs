using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
/// <summary>
///  This Quickstart Guide for the Advertising API requires the C# code kit, 
///  which can be found at: https://github.com/attdevsupport/codekit-csharp
///  Make sure that dlls are compiled and referenced in the project.
/// </summary>
using ATT.Codekit.ADS;
using ATT.Codekit.Authorization;

public partial class _Default : System.Web.UI.Page
{
    /// <summary>
    /// Use the app account settings from developer.att.com for the following values.
    /// Make sure ADS is enabled for the App Key and the App Secret.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        // Enter the value from the 'App Key' field obtained at developer.att.com in your 
        // app account.
        string appKey = "";

        // Enter the value from the 'App Secret' field obtained at developer.att.com 
        // in your app account.
        string appSecret = "";

        // Set the fully-qualified domain name to: https://api.att.com
        string fqdn = "https://api.att.com";

        //Set the scope to ADS
        string scope = "ADS";

        // Create the service for requesting an OAuth access token.
        var oauth = new OAuth(fqdn, appKey, appSecret, scope);

        // Get the OAuth access token using the Client Credentials.
        if (oauth.GetAccessToken(OAuth.AccessTokenType.ClientCredentials))
        {
            // Get access token 
            OAuthToken at = new OAuthToken();
            var accessToken = at.getAccessToken(oauth.accessTokenJson);

            // Create the service for making the method request.
            var ads = new Ads(fqdn, accessToken);

            // Set params:
            string category = "Auto";
            string udid = "123456789012345678901234567890";
            string userAgent ="Mozilla/5.0 (Linux; Android 4.0.4; Galaxy Nexus Build/IMM76B) AppleWebKit/535.19 (KHTML, like Gecko) Chrome/18.0.1025.133 Mobile Safari/535.19";
            
            try
            {
                // Make an Make a method call to the Advertising API.
                // param 1 category
                // param 2 udid
                // param 3 user agent 
                AdsObj.RootObject ads_obj = ads.GetAds(category, udid, userAgent);
            }
            catch (Exception ex)
            {
                string error = ex.StackTrace;
            }
        }
    }
}
