using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
/// <summary>
///  This Quickstart Guide for the In-App Messaging API requires the C# code kit, 
///  which can be found at: https://github.com/attdevsupport/codekit-csharp
///  Make sure that dlls are compiled and referenced in the project.
/// </summary>
using ATT.Codekit.IAM;
using ATT.Codekit.Authorization;

public partial class _Default : System.Web.UI.Page
{
    /// <summary>
    /// Use the app account settings from developer.att.com for the following values.
    /// Make sure MIM,IMMN is enabled for the app key/secret
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
        string authorizeRedirectUri = "";

        //Set the scope to MIM,IMMN
        string scope = "MIM,IMMN";

        //Create the service for requesting an OAuth access token.
        var oauth = new OAuth(fqdn, clientId, secretKey, authorizeRedirectUri, scope);

        // If there's no access token yet go through the concent flow. 
        if (Request["code"] == null)
        {
            // Authenticate the user. Note: This requires a Web browser.
            // Obtain the url string that is used for consent flow.
            if (!oauth.GetAuthCode())
            {
                //Get any error codes returned by the API Gateway.
                 string authCodeError = oauth.getAuthCodeError;
            }
        }
        else
        {
            // Get code in the query parameters after doing consent flow
            oauth.authCode = Request["code"].ToString();

            // Get the OAuth access token using the OAuth authentication code.
            if (oauth.GetAccessToken(OAuth.AccessTokenType.AuthorizationCode))
            {
                // Get access token 
                OAuthToken at = new OAuthToken();
                var accessToken = at.getAccessToken(oauth.accessTokenJson);

                // Create the service for making the method request.
                var mymessages = new MyMessages(fqdn, accessToken);

                // Create an instance for JavaScriptSerializer.
                var serializer = new JavaScriptSerializer();

                //*************************************************************************
                // Operation: Create Message Index
                // The Create Message Index method allows the developer to create an index 
                // cache for the user's AT&T Message inbox with prior consent.

                
                try
                {
                    // Make an Make a method call to the In-App Messaging API.
                    // Method takes no param
                    mymessages.createMessageIndex();
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }

                //*************************************************************************
                // Operation: Get Message List
                // Get meta-data for one or more messages from the customer's AT&T Message inbox.

                
                string limit = "<Enter limit>"; // e.g 500
                string offset = "<Enter pffset>"; // e.g. 50 
                
                try
                {
                    // Make a method call to the In-App Messaging API.
                    // Method takes no param
                    MessageObj.MessageList responseData = mymessages.getMessageList(limit, offset);
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }
                
                //*************************************************************************
                // Operation: Get Message 
                // Get one specific message from a user's AT&T Message inbox.

                var getMsg_messageId = "<Enter Message Id>";

                try
                {
                    // Make a method call to the In-App Messaging API.
                    // Method takes:
                    // param 1: string message id
                    MessageObj.Message responseData = mymessages.getMessage(getMsg_messageId);
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }

                //*************************************************************************
                // Operation: Get Message Content
                // Get media associated with a user message.

                string getMsgCtntMessageId = "<Enter Message Id>";
                string getMsgCtntPartId = "<Enter Part Id>"; // e.g. 0

                
                try
                {
                    // Make a method call to the In-App Messaging API.
                    // Method takes:
                    // param 1: string message id
                    // param 2: string part id
                    string responseData = mymessages.getMessageContent(getMsgCtntMessageId, getMsgCtntPartId);
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }

                //*************************************************************************
                // Operation: Get Message Index Info
                // Get the state, status, and message count of the index cache.

                try
                {
                    // Make a method call to the In-App Messaging API.
                    // Method takes no param
                    string responseData = mymessages.getMessageIndexInfo();
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }


                //*************************************************************************
                // Operation: Delete Message 
                // The Delete Message method gives the developer the ability to delete one 
                // specific message from a user's AT&T Message inbox with prior consent.

                var delMsg_messageId = "<Enter Message Id>";

                try
                {
                    // Make a method call to the In-App Messaging API.
                    // Method takes:
                    // param 1: string message id
                    mymessages.deleteMessage(delMsg_messageId);
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }

                //*************************************************************************
                // Operation: Delete Messages
                // The Delete Message method gives the developer the ability to delete one 
                // or more specific messages.

                var delMsg_messageId1 = "<Enter Message ID>";
                var delMsg_messageId2 = "<Enter Message ID>";
                var delMsg_messageId3 = "<Enter Message ID>";
                var queryString = "messageIds=" + delMsg_messageId1 + "%2" + delMsg_messageId2 + "%2" + delMsg_messageId3;

                try
                {
                    // Make a method call to the In-App Messaging API.
                    // Method takes:
                    // param 1: string query string
                    mymessages.deleteMessage(queryString);
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }

                //*************************************************************************
                // Operation: Get Message Delta
                // provides the capability to check for updates by passing in a client state.

                string state = "1403732028949";
                
                try
                {
                    // Make a method call to the In-App Messaging API.
                    // Method takes no param
                    string responseData = mymessages.getMessageDelta(state);
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }

                //*************************************************************************
                // Operation: Get Message Notification Connection Details
                // Retrieves details about the credentials, endpoint, and resource information 
                // to required to set up a notification connection. 
                
                string queues = "MMS";//MMS or TEXT
                 
                try
                {
                    // Make a method call to the In-App Messaging API.
                    // Method takes param 1: queues
                    string responseData = mymessages.getMessageNotificationConnectionDetails(queues);
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }
                
                //*************************************************************************
                // Operation: Send Message
                // The Send Message method sends messages on behalf of an AT&T Wireless user.

                //Convert Message request object to JSON
                string smAddress = "<Enter address>";//e.g. tel:2060000000
                string smText = "<Enter messgae>"; // message content

                var addressesList = new List<string>();
                addressesList.Add(smAddress); 

                var sendMsgObj = new MessageRequestObj.RootObject()
                {
                    messageRequest = new MessageRequestObj.MessageRequest()
                    {
                        addresses = addressesList,
                        text = smText
                    }
                };

                var sendMsgJson = serializer.Serialize(sendMsgObj);

                try
                {
                    // Make a method call to the In-App Messaging API.
                    // Method takes:
                    // param 1: string json
                    mymessages.SendMessage(sendMsgJson);
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }

                //*************************************************************************
                // Operation: Update Message
                // Update some flags related to one of the messages in a user's AT&T Message inbox. 

                var isUnread = true;

                //Convert Message object to JSON
                var updateMsgObj = new MessageObj.RootObject()
                {
                    message = new MessageObj.Message()
                    {
                        isUnread = isUnread
                    }
                };
                var updateMsgJson = serializer.Serialize(updateMsgObj);

                string umMessageId = "<Enter Message Id>";

                try
                {
                    // Make a method call to the In-App Messaging API.
                    // Method takes:
                    // param 1: string json
                    // param 2: string message id
                    mymessages.updateMessage(updateMsgJson, umMessageId);
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }

                //*************************************************************************
                // Operation: Update Messages
                // Update some flags related to one of the messages in a user's AT&T Message inbox. 

                //Convert Message object to JSON
                bool umIsUnread = true;
                var umMessages = new List<MessageObj.Message>();
                
                umMessages.Add(new MessageObj.Message()
                {
                    messageId = "<Enter Message Id>",
                    isUnread = umIsUnread
                });
               
                var updateMsgsObj = new MessageObj.RootObject();
                updateMsgsObj.messages = umMessages;
                
                var updateMsgsJson = serializer.Serialize(updateMsgsObj);

                try
                {
                    // Make a method call to the In-App Messaging API.
                    // Method takes:
                    // param 1: string json
                    mymessages.updateMessages(updateMsgsJson);
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }
            }
        }
    }
}
