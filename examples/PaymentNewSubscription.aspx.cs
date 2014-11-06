using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using System.IO;
/// <summary>
///  This Quickstart Guide for the Payment API requires the C# code kit, 
///  which can be found at: https://github.com/attdevsupport/codekit-csharp
///  Make sure that dlls are compiled and referenced in the project.
/// </summary>
using ATT.Codekit.PAYMENT;
using ATT.Codekit.Authorization;

public partial class _Default : System.Web.UI.Page
{
    // Error response for create new subscription and transaction
    public Dictionary<string, string> errorCreateNew { get; set; }

    // SubscriptionAuthCode
    public string SubscriptionAuthCode { get; set; }

    // TransactionAuthCode
    public string TransactionAuthCode { get; set; }

    // subscriptionid for create new subscription
    public string merchantSubscriptionId
    {
        get
        {
            Random rnd = new Random();
            int num = rnd.Next(1000, 9999);
            return "subid" + num.ToString();
        }
    }

    // subscriptionid for create new subscription
    public string merchantTransactionId
    {
        get
        {
            Random rnd = new Random();
            int num = rnd.Next(1000, 9999);
            return "mtid" + num.ToString();
        }
    }

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

        //Set the scope to Payment
        string scope = "Payment";

        // Create the service for requesting an OAuth access token.
        var oauth = new OAuth(fqdn, clientId, secretKey, scope);

        // Get the OAuth access token using Client Credential method.
        if (oauth.GetAccessToken(OAuth.AccessTokenType.ClientCredentials))
        {
            // Get access token 
            OAuthToken at = new OAuthToken();
            string accessToken = at.getAccessToken(oauth.accessTokenJson);

            // Create the service for making the method request.
            var payment = new Payment(fqdn, accessToken);
            if (string.IsNullOrEmpty((string)Session["NewSubscription"]))
            {
                /// <summary>
                /// Create new subscription
                /// </summary>
                /// <param name="apiKey">apiKey</param>
                /// <param name="secretKey">secretKey</param>
                /// <param name="amount">amount</param>
                /// <param name="category">category</param>
                /// <param name="channel">channel</param>
                /// <param name="description">description</param>
                /// <param name="merchantTransactionId">merchantTransactionId</param>
                /// <param name="merchantProductId">merchantProductId</param>
                /// <param name="merchantRedirectURI">merchantRedirectURI</param>
                /// <param name="merchantSubscriptionId">merchantSubscriptionId</param>
                /// <param name="isPurchaseOnNoActiveSubscription">isPurchaseOnNoActiveSubscription</param>
                /// <param name="subscriptionRecurringNumber">subscriptionRecurringNumber</param>
                /// <param name="subscriptionRecurringPeriod">subscriptionRecurringPeriod</param>
                /// <param name="subscriptionRecurringPeriodAmount">subscriptionRecurringPeriodAmount</param>
                string redirectUrl = payment.createNewSubscriptionRedirectUrl(
                    clientId,
                    secretKey,
                    0.01,
                    3,
                    "MOBILE_WEB",
                    "description",
                    merchantTransactionId,
                    "merchantProductId000111",
                    "http://localhost/PaymentNewSubscription.aspx",
                    merchantSubscriptionId,
                    false,
                    99999,
                    "MONTHLY",
                    1);
                Session["NewSubscription"] = "created";
                //Response.Redirect(redirectUrl);
            }


            if ((Request["success"] != null) && Request.QueryString["success"].ToString() == "false")
            {
                errorCreateNew = new Dictionary<string, string>();

                foreach (String key in Request.QueryString.AllKeys)
                {
                    errorCreateNew.Add(key, Request.QueryString[key]);
                }
                throw new Exception(Request.QueryString["faultDescription"]);
            }

            if ((Request["SubscriptionAuthCode"] != null))
            {
                SubscriptionAuthCode = Request.QueryString["SubscriptionAuthCode"].ToString();
            }

            /// <summary>
            /// Call getSubscriptionStatus operation return getSubscriptionStatus object
            /// </summary>
            /// <param name="idType">idType</param>
            /// <param name="id">id</param>
            
            SubscriptionStatusResponseObj.RootObject getSubscriptionStatus = 
                payment.getSubscriptionStatus("SubscriptionAuthCode", SubscriptionAuthCode);
            

            /// <summary>
            /// Call SubscriptionDetail operation return SubscriptionDetailResponse object
            /// </summary>
            /// <param name="MerchantSubscriptionId">MerchantSubscriptionId</param>
            /// <param name="ConsumerId">ConsumerId</param>

            SubscriptionDetailResponseObj.RootObject getSubscriptionDetail = 
                payment.getSubscriptionDetail(getSubscriptionStatus.MerchantSubscriptionId, 
                                                getSubscriptionStatus.ConsumerId);


           

        }
    }
}
