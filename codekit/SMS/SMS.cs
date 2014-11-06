using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using ATT.Codekit.REST;

namespace ATT.Codekit.Messaging
{
    public class SMS
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
        public SMS(string endPoint, string accessToken)
        {
            apiService = new APIService(endPoint, accessToken, "/sms/v3/messaging/");
        }

        /// <summary>
        /// This funtion makes API call to get SMS
        /// </summary>
        /// <param name="RegistrationID">RegistrationID</param>
        public InboundSmsMessageObj.RootObject getSMS(string RegistrationID)
        {
            APIRequest apiRequest = new APIRequest("inbox/" + RegistrationID);
            apiRequest.accept = "application/json";

            if (apiService.get(apiRequest))
            {
                InboundSmsMessageObj MsgListresponseObj = new InboundSmsMessageObj();
                return MsgListresponseObj.deserializeMsgListRequest(apiService.apiResponse.getResponseData());
            }

            throw new Exception(apiService.errorResponse);
        }

        /// <summary>
        /// This funtion makes API call to get SMS DeliveryStatus
        /// </summary>
        /// <param name="messageID">messageID</param>
        public DeliveryInfoObj.RootObject getSMSDeliveryStatus(string messageID)
        {
            APIRequest apiRequest = new APIRequest("outbox/" + messageID);
            apiRequest.accept = "application/json";

            if (apiService.get(apiRequest))
            {
                DeliveryInfoObj statusResponseObj = new DeliveryInfoObj();
                return statusResponseObj.deserializeDeliveryInfo(apiService.apiResponse.getResponseData());
            }

            throw new Exception(apiService.errorResponse);
        }

        /// <summary>
        /// This funtion makes API call to send SMS
        /// </summary>
        /// <param name="data">JSON string</param>
        public OutboundSMSResponseObj.RootObject sendSMS(string add, string msg)
        {
            var obSMSreqObj = new OutboundSMSRequestObj.RootObject()
            {
                outboundSMSRequest = new OutboundSMSRequestObj.OutboundSMSRequest()
                {
                    address = add,
                    message = msg
                }
            };

            var serializer = new JavaScriptSerializer();
            var data = serializer.Serialize(obSMSreqObj);

            APIRequest apiRequest = new APIRequest("outbox");

            apiRequest.accept = "application/json";
            apiRequest.contentType = "application/json";
            apiRequest.setBinaryData(data);

            if (apiService.post(apiRequest))
            {
                OutboundSMSResponseObj responseObj = new OutboundSMSResponseObj();
                return responseObj.deserializeOutboundSMSResponse(apiService.apiResponse.getResponseData());
            }

            throw new Exception(apiService.errorResponse);
        }
    }
}
