using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;

/// <summary>
///  This Code Snippet requires the third party library: Newtonsoft.Json 
///  which can be found at: http://http://james.newtonking.com/json
///  Make sure that Newtonsoft.Json has been installed, then require the class.
/// </summary>
using Newtonsoft.Json;

/// <summary>
///  This Quickstart Guide for the Address Book API requires the C# code kit, 
///  which can be found at: https://github.com/attdevsupport/codekit-csharp
///  Make sure that dlls are compiled and referenced in the project.
/// </summary>
using ATT.Codekit.AAB;
using ATT.Codekit.Authorization;


public partial class AAB : System.Web.UI.Page
{
    /// <summary>
    /// Use the app account settings from developer.att.com for the following values.
    /// Make sure that the API scope is set to AAB for the Address Book API
    /// before retrieving the App Key and App Secret.
    /// </summary>
    protected void Page_Load(object sender, EventArgs e)
    {
        // Enter the value from the 'App Key' field obtained at developer.att.com in your 
        // app account.
        string clientId = "";

        // Enter the value from 'App Secret' field obtained at developer.att.com 
        // in your app account.
        string secretKey = "";

        // Set the fully-qualified domain name to: https://api.att.com
        string fqdn = "https://api.att.com";

        // Set the redirect URI for returning after consent flow.
        string authorizeRedirectUri = "";

        //Set the scope to AAB
        string scope = "AAB";

        // Create the service for requesting an OAuth access token.
        var oauth = new OAuth(fqdn, clientId, secretKey, authorizeRedirectUri, scope);

        // If there's no access token yet go through the consent flow. 
        if (Request["code"] == null)
        //if(false)
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
            //Get code in the query parameters after doing consent flow.
            oauth.authCode = Request["code"].ToString();

            //Get the OAuth access token using the OAuth authentication code.
            if (oauth.GetAccessToken(OAuth.AccessTokenType.AuthorizationCode))
            {
                // Get access token 
                // Method takes:
                // param 1: string access token json object
                OAuthToken at = new OAuthToken();
                var accessToken = at.getAccessToken(oauth.accessTokenJson);

                // Create the service for making the method request.
                var addressbook = new AddressBook(fqdn, accessToken);

                //*************************************************************************
                // Operation: Create Contact
                // Create a new contact for our Address Book

                // Convert contact object to JSON string.
                var createContactObj = new ContactObj.RootObject()
                {
                    contact = new ContactObj.Contact()
                    {
                        firstName = "<Firstname>",
                        lastName = "<lastname>",
                        organization = "<organization>"
                    }
                };

                var contactJson = JsonConvert.SerializeObject(createContactObj, Formatting.Indented,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                try
                {   
                    // Make a method call to the Address Book API.
                    // Method takes:
                    // param 1: string contact json
                    string contactId = addressbook.createContact(contactJson);
                }
                catch(Exception ex)
                {
                    string error = ex.Message;
                }
                

                //*************************************************************************
                // Operation: Get Contact
                // Get an existing contact from Address Book.

                try
                {
                    // Make a method call to the Address Book API.
                    // Method takes:
                    // param 1: string contact id
                    QuickContactObj.RootObject getContactResponseData = addressbook.getContact("<enter contactId>");
                }
                catch(Exception ex)
                {
                    string error = ex.Message;
                }
                

                //*************************************************************************
                //Operation: Get Contacts
                // Search for existing contacts from Address Book

                // Search for the created contact:
                string searchField = "<Enter search word>";

                try
                {
                    // Make a method call to the Address Book API.  
                    // Method takes:
                    // param 1: string searchField
                    QuickContactObj.RootObject getContactsResponseData = addressbook.getContacts(searchField);
                }
                catch(Exception ex)
                {
                    string error = ex.Message;
                }

                //*************************************************************************
                // Operation: Get Contact Groups
                // Retrieving the list of groups a contact is belonging to.

                try
                {
                    // Make a method call to the Address Book API.
                    // Method takes:
                    // param 1: string contact id
                    string getContactGroupsResponseData = addressbook.getContactGroups("<enter contactId>");
                }
                catch(Exception ex)
                {
                    string error = ex.Message;
                }

                //*************************************************************************
                // Operation: Update Contact
                // Updates a contact based on the provided contact data structure.

                // Enter contact id to update
                var updateContactId = "<Enter contactId>";

                //Convert contact object to JSON
                var updateContactObj = new ContactObj.RootObject()
                {
                    contact = new ContactObj.Contact()
                    {
                        firstName = "testFirst",
                        lastName = "testLast",
                        organization = "att",
                    }
                };

                var updateContactJson = JsonConvert.SerializeObject(updateContactObj, Formatting.Indented,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                try
                {
                    // Make a method call to the Address Book API.
                    // Method takes:
                    // param 1: string contact id
                    // param 2: string contact json
                    addressbook.updateContact(updateContactId, updateContactJson);
                }
                catch(Exception ex)
                {
                    string error = ex.Message;
                }
                

                //*************************************************************************
                // Operation: Create Group
                // The Create Group method enables creating a USER group

                // Convert group object to JSON
                var createGroupObj = new GroupObj.RootObject()
                {
                    group = new GroupObj.Group()
                    {
                        groupName = "gGroup",
                        groupType = "gtype"
                    }
                };
                string groupId = String.Empty;

                var createGroupJson = JsonConvert.SerializeObject(createGroupObj, Formatting.Indented,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                try
                {
                    // Make a method call to the Address Book API.
                    // Method takes:
                    // param 1: string contact id
                    groupId = addressbook.createGroup(createGroupJson);
                }
                catch(Exception ex)
                {
                    string error = ex.Message;
                }

                //*************************************************************************
                // Operation: Get Groups
                // The Get Groups method enables retrieving the list of the subscriber’s groups with rules for ordering and pagination.

                // Get group by group id
                string groupsName = "gGroup";

                try
                {
                    // Make a method call to the Address Book API.
                    // Method takes:
                    // param 1: string groups_name 
                    GroupObj.RootObject groups = addressbook.getGroups(groupsName);
                }
                catch(Exception ex)
                {
                    string error = ex.Message;
                }


                //*************************************************************************
                // Operation: Update Groups
                // The Update Group method enables updating an existing USER group

                //Convert group object to JSON
                var updateGroupObj = new GroupObj.RootObject()
                {
                    group = new GroupObj.Group()
                    {
                        groupName = "newName",
                        groupType = "newType"
                    }
                };

                var updateGroupJson = JsonConvert.SerializeObject(updateGroupObj, Formatting.Indented,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                try
                {
                    // Make a method call to the Address Book API.
                    // Method takes:
                    // param 1: string group id
                    // param 2: string group json
                    addressbook.updateGroup(groupId, updateGroupJson);
                }
                catch(Exception ex)
                {
                    string error = ex.Message;
                }

                //*************************************************************************
                // Operation: Add Contact(s) to a Group

                string cGContactId = "<Enter contactId>";
                string cGGroupId = "<Enter groupId>";

                try
                {
                    // Make a method call to the Address Book API.
                    // Method takes:
                    // param 1: string contact id
                    // param 2: string group id
                   addressbook.addContactToGroup(cGGroupId, cGContactId);
                }
                catch(Exception ex)
                {
                    string error = ex.Message;
                }

                //*************************************************************************
                // Operation: Remove Contact(s) from a Group

                string rcGContactId = "<Enter contactId>";
                string rcGGroupId = "<Enter groupId>"; 

                try
                {
                    // Make a method call to the Address Book API.
                    // Method takes:
                    // param 1: string contact id
                    // param 2: string group id
                   addressbook.removeContactsFromGroup(rcGGroupId, rcGContactId);
                }
                catch(Exception ex)
                {
                    string error = ex.Message;
                }

                //*************************************************************************
                // Operation: Get Group Contacts
                // The Get Groups Contact method enables retrieving the list of contacts owned by a group

                string ggcGroupId = "<Enter groupId>"; 
                
                try
                {
                    // Make a method call to the Address Book API.
                    // Method takes:
                    // param 1: string contact id
                    // param 2: string group id
                   string responseGetFGroupContact = addressbook.getGroupContacts(ggcGroupId);
                }
                catch(Exception ex)
                {
                    string error = ex.Message;
                }

                //*************************************************************************
                // Operation: Delete Groups
                // The Delete Group method enables deleting a group.

                string delGroupId = "<Enter groupId>";

                try
                {
                    // Make a method call to the Address Book API.
                    // Method takes:
                    // param 1: string group id
                    addressbook.deleteGroup(delGroupId);
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }

                //*************************************************************************
                // Operation: Get MyInfo
                // Get the subscriber’s personal contact card.

                try
                {
                   // Make AddressBook API call
                   // Method takes: 0 param    
                   addressbook.getMyInfo();
                }
                catch(Exception ex)
                {
                    string error = ex.Message;
                }

                //*************************************************************************
                // Operation: Delete Contact
                // Delete a contact based on the provided contact id.

                // Enter contact id to update
                var deleteContactId = "<Enter contactId>";//"<Enter contact id>";

                try
                {
                    // Make a method call to the Address Book API.
                    // Method takes:
                    // param 1: string contact id
                    addressbook.deleteContact(deleteContactId);
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }
                
                //*************************************************************************
                // Operation: Update MyInfo
                // Update the subscriber’s personal contact card.

                //Convert MyInfo object to JSON
                var updateMyinfoObj = new ContactObj.RootObject()
                {
                    myInfo = new ContactObj.Contact()
                    {
                        firstName = "michelle",
                        lastName = "pai",
                        organization = "att"
                    }
                };

                var updateMyinfoJson = JsonConvert.SerializeObject(updateMyinfoObj, Formatting.Indented,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                try
                {
                    // Make a method call to the Address Book API.
                    // Method takes:
                    // param 1: string myinfo json 
                    addressbook.updateMyInfo(updateMyinfoJson);
                }
                catch (Exception ex)
                {
                    string error = ex.Message;
                }
            }
        }
    }
}
