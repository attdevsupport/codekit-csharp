using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
/// <summary>
///  This Quickstart Guide for the Device Capabilities API requires the C# code kit, 
///  which can be found at: https://github.com/attdevsupport/codekit-csharp
///  Make sure that dlls are compiled and referenced in the project.
/// </summary>
using ATT.Codekit.DC;
using ATT.Codekit.Authorization;

public partial class _Default : System.Web.UI.Page
{
    public OAuth oauth;
    public Devices devices;
    public string endPoint;
    public string scope;
    public string authCode;
    public string accessToken;
    public string refreshToken;
    public string refreshTokenExpiryTime;
    public string refreshExpiry;

    /// <summary>
    /// Use the app account settings from developer.att.com for the following values.
    /// Make sure DC is enabled for the app key/secret
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        // Enter the value from the 'App Key' field obtained at developer.att.com in your 
        // app account.
        string clientId = "";

        // Enter the value from the 'App Secret' field obtained at developer.att.com 
        // in your app account.
        string secretKey = "";

        // Set the fully-qualified domain name to: https://api.att.com
        string fqdn = "https://api.att.com";

        // Set the redirect URI for returning after consent flow.
        string authorizeRedirectUri = "http://localhost/DC.aspx";

        // Set the scope to DC
        string scope = "DC";

        // Create the service for requesting an OAuth access token.
        oauth = new OAuth(fqdn, clientId, secretKey, authorizeRedirectUri, scope);

        // If there's no access token yet go through the concent flow. 
        if (Request["code"] == null)
        {
            // Authenticate the user. Note: This requires a Web browser.
            // Obtain the url string that is used for consent flow.
            if (!oauth.GetAuthCode())
            {
                // Get any error codes returned by the API Gateway.
                string authCodeError = oauth.getAuthCodeError;
            }
        }
        else
        {
            //Get code in the query parameters after doing consent flow.
            this.oauth.authCode = Request["code"].ToString();

            // Get the OAuth access token using the OAuth authentication code.
            if (oauth.GetAccessToken(OAuth.AccessTokenType.AuthorizationCode))
            {
                // Get access token 
                OAuthToken at = new OAuthToken();
                accessToken = at.getAccessToken(oauth.accessTokenJson);

                // Create the service for making the method request.
                devices = new Devices(fqdn, accessToken);

                try
                {
                    // Make an Make a method call to the Device Capabilities API.
                    DeviceIdObj.RootObject dc = devices.GetDeviceCapabilities();
                }
                catch (Exception respex)
                {
                    string error = respex.StackTrace;
                }
            }
        }
    }
}
