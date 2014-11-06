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

public partial class CreateNewTransaction : System.Web.UI.Page
{
    // Error response for create new subscription and transaction
    public Dictionary<string, string> errorCreateNew { get; set; }

    // TransactionAuthCode
    public string TransactionAuthCode { get; set; }

    // subscriptionid for create new transaction
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

        //Set the scope to AAB
        string scope = "Payment";
        
        // Create the service for requesting an OAuth access token.
        var oauth = new OAuth(fqdn, clientId, secretKey, scope);

        // Get the OAuth access token using the Client Credentials.
        if (oauth.GetAccessToken(OAuth.AccessTokenType.ClientCredentials))
        {
            // Get access token 
            OAuthToken at = new OAuthToken();
            string accessToken = at.getAccessToken(oauth.accessTokenJson);

            // Create the service for making the method request.
            var payment = new Payment(fqdn, accessToken);
            
            if (string.IsNullOrEmpty((string)Session["NewTransaction"]))
            {
                /// <summary>
                /// Create new transaction
                /// </summary>
                /// <param name="apiKey">apiKey</param>
                /// <param name="secretKey">secretKey</param>
                /// <param name="description">description</param>
                /// <param name="amount">amount</param>
                /// <param name="category">category</param>
                /// <param name="channel">channel</param>
                /// <param name="merchantRedirectURI">merchantRedirectURI</param>
                /// <param name="merchantProductId">merchantProductId</param>
                /// <param name="merchantTransactionId">merchantTransactionId</param>

                var redirectUrl = payment.createNewTransactionRedirectUrl(
                    clientId,
                    secretKey,
                    "Sample Product",
                    0.01,
                    2,
                    "MOBILE_WEB",
                    "http://localhost/PaymentNewTransaction.aspx",
                    "whateverprod",
                    merchantTransactionId);

                Session["NewTransaction"] = "created";
                Response.Redirect(redirectUrl);
            }

            if (Request.QueryString["success"] != null && Request.QueryString["success"].ToString() == "false")
            {
                errorCreateNew = new Dictionary<string, string>();

                foreach (String key in Request.QueryString.AllKeys)
                {
                    errorCreateNew.Add(key, Request.QueryString[key]);
                }
                throw new Exception(Request.QueryString["faultDescription"]);
            }

            if ((Request["TransactionAuthCode"] != null))
            {
                TransactionAuthCode = Request.QueryString["TransactionAuthCode"].ToString();
            }


            /// <summary>
            /// Call getTransactionStatus operation return getTransactionStatus object
            /// </summary>
            /// <param name="idType">idType</param>
            /// <param name="id">id</param>
            TransactionStatusResponseObj.RootObject getTransactionStatus 
                = payment.getTransactionStatus("TransactionAuthCode", TransactionAuthCode);
            

            //Get Payment notification
            //Stream inputstream = Request.InputStream;
            //int streamLength = Convert.ToInt32(inputstream.Length);
            //byte[] stringArray = new byte[streamLength];
            //inputstream.Read(stringArray, 0, streamLength);

            //string xmlString = System.Text.Encoding.UTF8.GetString(stringArray);

            string xmlString = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><ownershipEvent type=\"revoke\" timestamp=\"2014-05-02T06:12:33+00:00\" effective=\"2014-05-02T06:12:18+00:00\"><networkOperatorId>cingular</networkOperatorId><ownerIdentifier>N_NBI_PNW_1369816836000116729061</ownerIdentifier><purchaseDate>2014-05-02T06:12:05+00:00</purchaseDate><productIdentifier>Onetime_Cat1</productIdentifier><purchaseActivityIdentifier>Z89EZXmzjy2yu6s7wFY88cZM9lgztD6PRyo8</purchaseActivityIdentifier><instanceIdentifier>ada820f1-ce48-499b-8b46-eac60ef28a2a-CTASTransactionP1</instanceIdentifier><minIdentifier>4256586023</minIdentifier><sequenceNumber>178</sequenceNumber><reasonCode>1760</reasonCode><reasonMessage>CP Requested Refund</reasonMessage><vendorPurchaseIdentifier>M789819033</vendorPurchaseIdentifier></ownershipEvent>";

            XmlSerializer deserializer = new XmlSerializer(typeof(ownershipEvent));
            TextReader textReader = new StringReader(xmlString);
            ownershipEvent notificationObj;
            notificationObj = (ownershipEvent)deserializer.Deserialize(textReader);
            textReader.Close();


            //create refund transaction object
            RefundTransactionRequestObj.RootObject refund 
                = new RefundTransactionRequestObj.RootObject()
            {
                TransactionOperationStatus = "Refunded",
                RefundReasonCode = 1,
                RefundReasonText = "Customer was not happy"
            };

            var serializer = new JavaScriptSerializer();
            var refundJSON = serializer.Serialize(refund);

            RefundTransactionResponseObj.RootObject refundResponse 
                = payment.RefundTransaction(refundJSON, getTransactionStatus.TransactionId);

        }
    }
}