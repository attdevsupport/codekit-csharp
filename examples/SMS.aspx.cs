using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
/// <summary>
///  This Quickstart Guide for the SMS API requires the C# code kit, 
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

        //Set the scope to AAB
        string scope = "SMS";

        //Create the service for requesting an OAuth access token.
        var oauth = new OAuth(fqdn, clientId, secretKey, scope);

        //Get the OAuth access token using the Client Credentials.
        if (oauth.GetAccessToken(OAuth.AccessTokenType.ClientCredentials))
        {
            // Get access token 
            OAuthToken at = new OAuthToken();
            string accessToken = at.getAccessToken(oauth.accessTokenJson);

            // Create the service for making the method request.
            var sms = new SMS(fqdn, accessToken);

            //*************************************************************************
            // Operation: Send SMS
            
            // Set params:
            string address = "tel:+10000000000";
            string message = "<Enter message>";
            
            string messageId = String.Empty;
            try
            {
                // Make an Make a method call to the SMS API.
                // Method takes:
                // param 1: address
                // param 1: message
                OutboundSMSResponseObj.RootObject sendSMSresponseObj = sms.sendSMS(address, message);
                messageId = sendSMSresponseObj.outboundSMSResponse.messageId;
            }
            catch (Exception respex)
            {
                string error = respex.StackTrace;
            }

            //*************************************************************************
            // Operation: Get SMS

            string RegistrationID = "00000000";

            try
            {
                // Make an Make a method call to the SMS API.
                // Method takes:
                // param 1: RegistrationID
                InboundSmsMessageObj.RootObject getSMSresponseObj = sms.getSMS(RegistrationID);
            }
            catch (Exception respex)
            {
                string error = respex.StackTrace;
            }

            //*************************************************************************
            // Operation: Get SMS Delivery Status

            try
            {
                // Make an Make a method call to the SMS API.
                // Method takes:
                // param 1: messageID
                DeliveryInfoObj.RootObject deliveryStatusResponse = sms.getSMSDeliveryStatus(messageId);
            }
            catch (Exception respex)
            {
                string error = respex.StackTrace;
            }
            
        }
    }
}
