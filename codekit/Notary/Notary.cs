using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using ATT.Codekit.REST;

namespace ATT
{
    namespace Codekit
    {
        namespace PAYMENT
        {
            public class Notary
            {
                APIService apiService;

                public string signedData { get; set; }

                public string signedPayload { get; set; }

                public string signedSignature { get; set; }

                public string requestUri { get; set; }

                /// <summary>
                /// Notary Constructor
                /// </summary>
                /// <param name="endPoint">endPoint</param>
                /// <param name="apiKey">apiKey</param>
                /// <param name="secretKey">secretKey</param>
                public Notary(string endPoint)
                {
                    signedData = String.Empty;
                    signedPayload = String.Empty;
                    signedSignature = String.Empty;

                    apiService = new APIService(endPoint, "", "/Security/Notary/Rest/1/");

                }

                /// <summary>
                /// This function to get payload string for  creating new subscription
                /// </summary>
                public string getPyaloadString(
                    int category,
                    double amount,
                    string channel,
                    string description,
                    string merchantTransactionId,
                    string merchantProductId,
                    string merchantRedirectURI,
                    string merchantSubscriptionIdList,
                    bool isPurchaseOnNoActiveSubscription,
                    int subscriptionRecurrences,
                    string subscriptionRecurringPeriod,
                    int subscriptionRecurringPeriodAmount)
                {
                    SubscriptionPayloadObj.RootObject payload = new SubscriptionPayloadObj.RootObject()
                    {
                        Amount = amount,
                        Category = category,
                        Channel = channel,
                        Description = description,
                        MerchantTransactionId = merchantTransactionId,
                        MerchantProductId = merchantProductId,
                        MerchantPaymentRedirectUrl = merchantRedirectURI,
                        MerchantSubscriptionIdList = merchantSubscriptionIdList,
                        IsPurchaseOnNoActiveSubscription = isPurchaseOnNoActiveSubscription,
                        SubscriptionRecurrences = subscriptionRecurrences,
                        SubscriptionPeriod = subscriptionRecurringPeriod,
                        SubscriptionPeriodAmount = subscriptionRecurringPeriodAmount
                    };

                    var serializer = new JavaScriptSerializer();
                    return serializer.Serialize(payload);
                }

                /// <summary>
                /// This function to get payload string for creating new transaction 
                /// </summary>
                public string getPyaloadString(
                    string description,
                    int category,
                    double amount,
                    string channel,
                    string merchantRedirectURI,
                    string merchantProductId,
                    string merchantTransactionId)
                {
                    TransactiontionPayloadObj.RootObject payload = new TransactiontionPayloadObj.RootObject()
                    {

                        Description = description,
                        Amount = amount,
                        Category = category,
                        Channel = channel,
                        MerchantPaymentRedirectUrl = merchantRedirectURI,
                        MerchantProductId = merchantProductId,
                        MerchantTransactionId = merchantTransactionId
                    };

                    var serializer = new JavaScriptSerializer();
                    return serializer.Serialize(payload);
                }

                /// <summary>
                /// This function making http post request to get signedData, signedPayload, signedSignature
                /// </summary>
                public void SubmitToNotary(string sendingData, string appKey, string secretKey)
                {
                    APIRequest apiRequest = new APIRequest("SignedPayload");

                    apiRequest.addHeaders("client_id", appKey);
                    apiRequest.addHeaders("client_secret", secretKey);
                    apiRequest.requireAccessToken = false;
                    apiRequest.setBinaryData(sendingData);
                    apiRequest.contentType = "application/json";
                    apiRequest.accept = "application/json";

                    if (apiService.post(apiRequest))
                    {
                        string responseData = apiService.apiResponse.getResponseData();

                        var notaryResponse = new NotaryResponse();
                        var notaryObj = notaryResponse.deserializeNotaryResponse(responseData);
                        
                        signedPayload = notaryObj.SignedDocument;
                        signedSignature = notaryObj.Signature;
                    }
                    else
                    {
                        throw new Exception(apiService.errorResponse);
                    }                    
                }
            }
        }
    }
}