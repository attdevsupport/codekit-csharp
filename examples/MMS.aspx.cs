using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
/// <summary>
///  This Quickstart Guide for the MMS API requires the C# code kit, 
///  which can be found at: https://github.com/attdevsupport/codekit-csharp
///  Make sure that dlls are compiled and referenced in the project.
/// </summary>
using ATT.Codekit.Messaging;
using ATT.Codekit.Authorization;

public partial class _Default : System.Web.UI.Page
{
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

        //Set the scope to MMS
        string scope = "MMS";

        //Create the service for requesting an OAuth access token.
        var oauth = new OAuth(fqdn, clientId, secretKey, scope);

        //Get the OAuth access token using client Credential method.
        if (oauth.GetAccessToken(OAuth.AccessTokenType.ClientCredentials))
        {
            // Get access token 
            OAuthToken at = new OAuthToken();
            string accessToken = at.getAccessToken(oauth.accessTokenJson);

            // Create the service for making the method request.
            var mms = new MMS(fqdn, accessToken);

            // Set params:
            string mmsAddress = Server.UrlEncode("tel:10000000000") + "&"; //e.g.tel:1234567890
            string mmsMessage = Server.UrlEncode("msg txt context");
            string mmsFile = Server.MapPath("~/")+"attachments/att.jpg"; //Attachement path
            string mmsContentType = "image/jpeg";
            string messageId = "";

            try
            {
                // Make an Make a method call to the MMS API.
                // Method takes:
                // param 1: string mmsAddress (phone number(s))
                // param 2: message text content 
                // param 3: file path
                // param 4: multipart content type
                OutboundMessageResponseObj.RootObject SendMMSResponseObj = mms.sendMMS(mmsAddress, mmsMessage, mmsFile, mmsContentType);
                messageId = SendMMSResponseObj.outboundMessageResponse.messageId;
            }
            catch (Exception respex)
            {
                string error = respex.StackTrace;
            }

            try
            {
                // Make an Make a method call to the MMS API.
                // Method takes:
                // param 1: string message id
                DeliveryInfoObj.RootObject getStatusResponseObj = mms.getMMSDeliveryStatus(messageId);
            }
            catch (Exception respex)
            {
                string error = respex.StackTrace;
            }

        }
    }
}
