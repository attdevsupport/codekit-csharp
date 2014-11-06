using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using ATT.Codekit.REST;

/// <summary>
/// Summary description for AddressBook
/// </summary>

namespace ATT.Codekit.AAB
{
    public class AddressBook
    {
        /// <summary>
        /// APIService instance
        /// </summary>
        public APIService apiService { get; set; }

        /// <summary>
        /// Location id
        /// </summary>
        public string location { get; set; }

        /// <summary>
        /// Addressbook constructor
        /// </summary>
        public AddressBook(string endPoint, string accessToken)
        {
            apiService = new APIService(endPoint, accessToken, "/addressBook/v1/");
        }

        /// <summary>
        /// This function makes API call to create contact operation
        /// </summary>
        public string createContact(string data)
        {
            APIRequest apiRequest = new APIRequest("contacts");

            apiRequest.contentType = "application/json";
            apiRequest.setBinaryData(data);

            if (apiService.post(apiRequest))
            {
                return apiService.apiResponse.location;
            }

            throw new Exception(apiService.errorResponse);
        }

        /// <summary>
        /// This function makes API call to get contact operation
        /// </summary>
        public QuickContactObj.RootObject getContact(string contactid)
        {
            APIRequest apiRequest = new APIRequest("contacts/" + contactid);

            apiRequest.accept = "application/json";
            apiRequest.addHeaders("x-fields", "shallow");

            if (apiService.get(apiRequest))
            {
                var contact = new QuickContactObj();
                return contact.deserializeQuickContactObj(apiService.apiResponse.getResponseData());
            }

            throw new Exception(apiService.errorResponse);
        }

        /// <summary>
        /// This function makes API call to get contacts operation
        /// </summary>
        public QuickContactObj.RootObject getContacts(string search)
        {
            APIRequest apiRequest = new APIRequest("contacts?" + "search=" + search);

            apiRequest.accept = "application/json";
            apiRequest.addHeaders("x-fields", "shallow");

            if (apiService.get(apiRequest))
            {
                var contact = new QuickContactObj();
                return contact.deserializeQuickContactsObj(apiService.apiResponse.getResponseData());
            }

            throw new Exception(apiService.errorResponse);
        }

        /// <summary>
        /// This function makes API call to get contact groups operation
        /// </summary>
        public string getContactGroups(string contactID)
        {
            APIRequest apiRequest = new APIRequest("contacts/" + contactID + "/groups");

            apiRequest.accept = "application/json";

            if (apiService.get(apiRequest))
            {
                return apiService.apiResponse.getResponseData();
            }

            throw new Exception(apiService.errorResponse);
        }

        /// <summary>
        /// This function makes API call to update contact operation
        /// </summary>
        public bool updateContact(string contactID, string data)
        {
            APIRequest apiRequest = new APIRequest("contacts/" + contactID);

            apiRequest.contentType = "application/json";
            apiRequest.setBinaryData(data);

            if (apiService.patch(apiRequest))
            {
                return true;
                
            }

            throw new Exception(apiService.errorResponse);
        }

        /// <summary>
        /// This function makes API call to delete contact operation
        /// </summary>
        public bool deleteContact(string contactID)
        {
            APIRequest apiRequest = new APIRequest("contacts/" + contactID);

            if (apiService.delete(apiRequest))
            {
                return true;
            }

            throw new Exception(apiService.errorResponse);
        }

        /// <summary>
        /// This function makes API call to create group operation
        /// </summary>
        public string createGroup(string data)
        {
            APIRequest apiRequest = new APIRequest("groups");

            apiRequest.contentType = "application/json";
            apiRequest.setBinaryData(data);

            if (apiService.post(apiRequest))
            {
                return apiService.apiResponse.location;
            }

            throw new Exception(apiService.errorResponse);
        }

        /// <summary>
        /// This function makes API call to get groups operation
        /// </summary>
        public GroupObj.RootObject getGroups(string groupsName)
        {
            APIRequest apiRequest = new APIRequest("groups??groupName=" + groupsName);

            apiRequest.accept = "application/json";

            if (apiService.get(apiRequest))
            {
                var group = new GroupObj();
                return group.deserializeGroupObj(apiService.apiResponse.getResponseData());
            }

            throw new Exception(apiService.errorResponse);
        }

        
        /// <summary>
        /// This function makes API call to add contact to group operation
        /// </summary>
        public bool addContactToGroup(string groupID, string contactids)
        {
            APIRequest apiRequest = new APIRequest("groups/" + groupID + "/contacts?contactIds=" + contactids);

            if (apiService.post(apiRequest))
            {
                return true;
               
            }

            throw new Exception(apiService.errorResponse);
        }

        /// <summary>
        /// This function makes API call to remove contact from group operation
        /// </summary>
        public bool removeContactsFromGroup(string groupID, string contactids)
        {
            APIRequest apiRequest = new APIRequest("groups/" + groupID + "/contacts?contactIds=" + contactids);

            if (apiService.delete(apiRequest))
            {
                return true;
            }

            throw new Exception(apiService.errorResponse);
        }

        /// <summary>
        /// This function makes API call to get group contacts operation
        /// </summary>
        public string getGroupContacts(string groupID)
        {
            APIRequest apiRequest = new APIRequest("groups/" + groupID + "/contacts?");

            apiRequest.accept = "application/json";

            if (apiService.get(apiRequest))
            {
                return apiService.apiResponse.getResponseData();
            }

            throw new Exception(apiService.errorResponse);
        }

        /// <summary>
        /// This function makes API call to get contacts operation
        /// </summary>
        public String getMyInfo()
        {
            APIRequest apiRequest = new APIRequest("myInfo");

            apiRequest.accept = "application/json";

            if (apiService.get(apiRequest))
            {
                //return true;
                return apiService.apiResponse.getResponseData();
            }

            throw new Exception(apiService.errorResponse);
        }

        /// <summary>
        /// This function makes API call to update group operation
        /// </summary>
        public bool updateGroup(string groupID, string data)
        {
            APIRequest apiRequest = new APIRequest("groups/" + groupID);

            apiRequest.contentType = "application/json";
            apiRequest.setBinaryData(data);

            if (apiService.patch(apiRequest))
            {
                return true;
            }

            throw new Exception(apiService.errorResponse);
        }

        /// <summary>
        /// This function makes API call to delete group operation
        /// </summary>
        public bool deleteGroup(string groupID)
        {
            APIRequest apiRequest = new APIRequest("groups/" + groupID);

            if (apiService.delete(apiRequest))
            {
                return true;
            }

            throw new Exception(apiService.errorResponse);
        }

        /// <summary>
        /// This function makes API call to update my info operation
        /// </summary>
        public bool updateMyInfo(string data)
        {
            APIRequest apiRequest = new APIRequest("myInfo");

            apiRequest.contentType = "application/json";
            apiRequest.setBinaryData(data);

            if (apiService.patch(apiRequest))
            {
                return true;
            }

            throw new Exception(apiService.errorResponse);
        }

    }
}
