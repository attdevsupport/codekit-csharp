using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using ATT.Codekit.REST;

namespace ATT.Codekit.IAM
{
    public class MyMessages
    {
        /// <summary>
        /// APIService instance
        /// </summary>
        public APIService apiService { get; set; }

        /// <summary>
        /// JavaScriptSerializer 
        /// </summary>
        public JavaScriptSerializer serializer { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public MyMessages(string endPoint, string accessToken)
        {
            apiService = new APIService(endPoint, accessToken, "/myMessages/v2/");
        }

        /// <summary>
        /// This using makes API call to create message index
        /// </summary>
        public bool createMessageIndex()
        {
            APIRequest apiRequest = new APIRequest("messages/index");

            apiRequest.accept = "application/json";

            return apiService.post(apiRequest);
        }

        /// <summary>
        /// This using makes API call to delete message 
        /// </summary>
        public bool deleteMessage(string messageId)
        {
            APIRequest apiRequest = new APIRequest("messages/" + messageId);

            apiRequest.accept = "application/json";

            if (apiService.delete(apiRequest))
            {
                return true;
            }

            throw new Exception(apiService.errorResponse);
        }

        /// <summary>
        /// This function makes API call to delete messages
        /// </summary>
        public bool deleteMessages(string quertString)
        {
            APIRequest apiRequest = new APIRequest("messages?" + quertString);

            apiRequest.accept = "application/json";

            if (apiService.delete(apiRequest))
            {
                return true;
            }

            throw new Exception(apiService.errorResponse);
        }

        /// <summary>
        /// This function makes API call to get message
        /// </summary>
        public MessageObj.Message getMessage(string messageId)
        {
            APIRequest apiRequest = new APIRequest("messages/" + messageId);

            apiRequest.accept = "application/json";

            if (apiService.get(apiRequest))
            {
                var message = new MessageObj();
                return message.deserializeMessageObj(apiService.apiResponse.getResponseData());
            }

            throw new Exception(apiService.errorResponse);
        }

        /// <summary>
        /// This function makes API call to get message content
        /// </summary>
        public string getMessageContent(string messageId, string partId)
        {
            APIRequest apiRequest = new APIRequest("messages/" + messageId + "/parts/" + partId);

            apiRequest.accept = "application/json";

            if (apiService.get(apiRequest))
            {
                return apiService.apiResponse.getResponseData();
            }

            throw new Exception(apiService.errorResponse);
        }

        /// <summary>
        /// This function makes API call to get message index info
        /// </summary>
        public string getMessageIndexInfo()
        {
            APIRequest apiRequest = new APIRequest("messages/index/info");

            apiRequest.accept = "application/json";


            if (apiService.get(apiRequest))
            {
                return apiService.apiResponse.getResponseData();
            }

            throw new Exception(apiService.errorResponse);

        }

        /// <summary>
        /// This function makes API call to get message list
        /// </summary>
        public MessageObj.MessageList getMessageList(string limit, string offset)
        {
            if (String.IsNullOrEmpty(limit))
            {
                limit = "500";
            }

            if (String.IsNullOrEmpty(offset))
            {
                offset = "50";
            }

            APIRequest apiRequest = new APIRequest("messages?limit=" + limit + "&offset=" + offset);

            apiRequest.accept = "application/json";

            if (apiService.get(apiRequest))
            {
                var message = new MessageObj();
                return message.deserializeMessageListObj(apiService.apiResponse.getResponseData());
            }

            throw new Exception(apiService.errorResponse);
        }

        /// <summary>
        /// This function makes API call to get message delta
        /// </summary>
        public string getMessageDelta(string state)
        {
            APIRequest apiRequest = new APIRequest("delta?state=" + state);

            apiRequest.accept = "application/json";

            if (apiService.get(apiRequest))
            {
                return apiService.apiResponse.getResponseData();
            }
            throw new Exception(apiService.errorResponse);
        }

        /// <summary>
        /// This function makes API call to get message delta
        /// </summary>
        public string getMessageNotificationConnectionDetails(string queues)
        {
            APIRequest apiRequest = new APIRequest("notificationConnectionDetails?queues=" + queues);

            apiRequest.accept = "application/json";

            if (apiService.get(apiRequest))
            {
                return apiService.apiResponse.getResponseData();
            }

            throw new Exception(apiService.errorResponse);
        }
        /// <summary>
        /// This function makes API call to send message
        /// </summary>
        public bool SendMessage(string data)
        {
            APIRequest apiRequest = new APIRequest("messages");

            apiRequest.accept = "application/json";
            apiRequest.contentType = "application/json";
            apiRequest.setBinaryData(data);

            if (apiService.post(apiRequest))
            {
                return true;
            }

            throw new Exception(apiService.errorResponse);
        }

        /// <summary>
        /// This function makes API call to update message
        /// </summary>
        public bool updateMessage(string data, string messageId)
        {
            APIRequest apiRequest = new APIRequest("messages/" + messageId);

            apiRequest.accept = "application/json";
            apiRequest.contentType = "application/json";
            apiRequest.setBinaryData(data);

            if (apiService.put(apiRequest))
            {
                return true;
            }

            throw new Exception(apiService.errorResponse);
        }


        /// <summary>
        /// This function makes API call to update messages
        /// </summary>
        public bool updateMessages(string data)
        {
            APIRequest apiRequest = new APIRequest("messages");

            apiRequest.accept = "application/json";
            apiRequest.contentType = "application/json";
            apiRequest.setBinaryData(data);

            if (apiService.put(apiRequest))
            {
                return true;
            }

            throw new Exception(apiService.errorResponse);
        }

    }
}
