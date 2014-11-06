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
            public class Payment
            {
                /// <summary>
                /// APIService instance
                /// </summary>
                public APIService apiService { get; set; }

                /// <summary>
                /// Subscription notary instance
                /// </summary>
                public Notary subNotary { get; set; }

                /// <summary>
                /// Transaction notary instance
                /// </summary>
                public Notary tranNotary { get; set; }

                /// <summary>
                /// endPoint
                /// </summary>
                public string endPoint { get; set; }

                /// <summary>
                /// JavaScriptSerializer 
                /// </summary>
                public JavaScriptSerializer serializer { get; set; }

                /// <summary>
                /// Constructor
                /// </summary>
                public Payment(string endPoint, string accessToken)
                {
                    apiService = new APIService(endPoint, accessToken, "/rest/3/Commerce/Payment/");

                    subNotary = new Notary(endPoint);

                    tranNotary = new Notary(endPoint);

                    this.endPoint = endPoint;
                }


                /// <summary>
                /// This function return redirect url string for new subscription
                /// </summary>
                /// <param name="apiKey">apiKey</param>
                /// <param name="secretKey">secretKey</param>
                /// <param name="amount">amount</param>
                /// <param name="category">category</param>
                /// <param name="channel">channel</param>
                /// <param name="description">description</param>
                /// <param name="merchantTransactionId">apiKmerchantTransactionIdey</param>
                /// <param name="merchantProductId">merchantProductId</param>
                /// <param name="merchantRedirectURI">merchantRedirectURI</param>
                /// <param name="merchantSubscriptionId">merchantSubscriptionId</param>
                /// <param name="isPurchaseOnNoActiveSubscription">isPurchaseOnNoActiveSubscription</param>
                /// <param name="subscriptionRecurringNumber">subscriptionRecurringNumber</param>
                /// <param name="subscriptionRecurringPeriod">subscriptionRecurringPeriod</param>
                /// <param name="subscriptionRecurringPeriodAmount">subscriptionRecurringPeriodAmount</param>
                public string createNewSubscriptionRedirectUrl(
                    string apiKey,
                    string secretKey,
                    double amount,
                    int category,
                    string channel,
                    string description,
                    string merchantTransactionId,
                    string merchantProductId,
                    string merchantRedirectURI,
                    string merchantSubscriptionId,
                    bool isPurchaseOnNoActiveSubscription,
                    int subscriptionRecurringNumber,
                    string subscriptionRecurringPeriod,
                    int subscriptionRecurringPeriodAmount)
                {
                    string payloadString = subNotary.getPyaloadString(category, amount,
                     channel, description, merchantTransactionId,
                     merchantProductId, merchantRedirectURI, merchantSubscriptionId,
                     isPurchaseOnNoActiveSubscription, subscriptionRecurringNumber,
                     subscriptionRecurringPeriod, subscriptionRecurringPeriodAmount);
                    subNotary.SubmitToNotary(payloadString, apiKey, secretKey);
                    if (String.IsNullOrEmpty(subNotary.signedPayload) || String.IsNullOrEmpty(subNotary.signedSignature))
                    {
                        throw new Exception("There's no signed payload or signed signature.");
                    }
                    else
                    {
                        var builder = new UriBuilder(endPoint + "/rest/3/Commerce/Payment/Subscriptions");
                        var query = HttpUtility.ParseQueryString(builder.Query);

                        query["clientid"] = apiKey;
                        query["SignedPaymentDetail"] = subNotary.signedPayload;
                        query["Signature"] = subNotary.signedSignature;
                        builder.Query = query.ToString();
                        return builder.ToString();
                        
                        //return apiService.requestUri + "Transactions?clientid=" + apiKey + "&SignedPaymentDetail=" + subNotary.signedPayload + "&Signature=" + subNotary.signedSignature;
                    }
                }

                /// <summary>
                /// This function return redirect url string for creating new transaction
                /// </summary>
                /// <summary>
                /// Create new transaction
                /// </summary>
                /// <param name="apiKey">apiKey</param>
                /// <param name="secretKey">secretKey</param>
                /// <param name="amount">amount</param>
                /// <param name="category">category</param>
                /// <param name="channel">channel</param>
                /// <param name="description">description</param>
                /// <param name="merchantTransactionId">apiKmerchantTransactionIdey</param>
                /// <param name="merchantProductId">merchantProductId</param>
                /// <param name="merchantRedirectURI">merchantRedirectURI</param>
                public string createNewTransactionRedirectUrl(
                    string apiKey,
                    string secretKey,
                    string description,
                    double amount,
                    int category,
                    string channel,
                    string merchantRedirectURI,
                    string merchantProductId,
                    string merchantTransactionId
                    )
                {
                    string payloadString = tranNotary.getPyaloadString(description, category, amount,
                     channel, merchantRedirectURI, merchantProductId, merchantTransactionId);

                    tranNotary.SubmitToNotary(payloadString, apiKey, secretKey);
                    if (String.IsNullOrEmpty(tranNotary.signedPayload) || 
                        String.IsNullOrEmpty(tranNotary.signedSignature))
                    {
                        throw new Exception("There's no signed payload or signed signature.");
                    }
                    else
                    {
                        var builder = new UriBuilder(endPoint + "/rest/3/Commerce/Payment/Transactions");
                        var query = HttpUtility.ParseQueryString(builder.Query);

                        query["clientid"] = apiKey;
                        query["SignedPaymentDetail"] = tranNotary.signedPayload;
                        query["Signature"] = tranNotary.signedSignature;
                        builder.Query = query.ToString();
                        return builder.ToString();
                        
                        
                        //return apiService.requestUri 
                        //    + "Transactions?clientid=" + apiKey 
                        //    + "&SignedPaymentDetail=" + tranNotary.signedPayload 
                        //    + "&Signature=" + tranNotary.signedSignature;
                    }
                }

                /// <summary>
                /// This function makes API call to Get Subscription Status
                /// </summary>
                /// <param name="idType">idType</param>
                /// <param name="id">id</param>
                public SubscriptionStatusResponseObj.RootObject getSubscriptionStatus(string idType, string id)
                {
                    APIRequest apiRequest = new APIRequest("Subscriptions/" + idType + "/" + id);

                    apiRequest.accept = "application/json";

                    if (apiService.get(apiRequest))
                    {
                        SubscriptionStatusResponseObj responseObj = new SubscriptionStatusResponseObj();
                        return responseObj.deserializeSubscriptionStatusResponse(apiService.apiResponse.getResponseData());
                    }

                    throw new Exception(apiService.errorResponse);
                }

                /// <summary>
                /// This function makes API call to Get Subscription detail
                /// </summary>
                /// <param name="MerchantSubscriptionId">MerchantSubscriptionId</param>
                /// <param name="ConsumerId">ConsumerId</param>
                public SubscriptionDetailResponseObj.RootObject getSubscriptionDetail(string MerchantSubscriptionId, string ConsumerId)
                {
                    APIRequest apiRequest = new APIRequest("Subscriptions/" + MerchantSubscriptionId + "/Detail/" + ConsumerId);

                    if (apiService.get(apiRequest))
                    {
                        SubscriptionDetailResponseObj responseObj = new SubscriptionDetailResponseObj();
                        return responseObj.deserializeSubscriptionDetailResponse(apiService.apiResponse.getResponseData());
                    }

                    throw new Exception(apiService.errorResponse);
                }

                /// <summary>
                /// This function makes API call to Get Transaction Status
                /// </summary>
                /// <param name="idType">idType</param>
                /// <param name="id">id</param>
                public TransactionStatusResponseObj.RootObject getTransactionStatus(string idType, string id)
                {
                    APIRequest apiRequest = new APIRequest("Transactions/" + idType + "/" + id);
                    
                    if (apiService.get(apiRequest))
                    {
                        TransactionStatusResponseObj responseObj = new TransactionStatusResponseObj();
                        return responseObj.deserializeTransactionStatusResponse(apiService.apiResponse.getResponseData());
                    }

                    throw new Exception(apiService.errorResponse);
                }

                /// <summary>
                /// This function makes API call to Get Transaction Status
                /// </summary>
                /// <param name="idType">idType</param>
                /// <param name="id">id</param>
                public RefundTransactionResponseObj.RootObject RefundTransaction(string refundObjJSON, string TransactionId)
                {
                    APIRequest apiRequest = new APIRequest("Transactions/" + TransactionId + "?Action=refund");
                    
                    apiRequest.setBinaryData(refundObjJSON);

                    if (apiService.put(apiRequest))
                    {
                        RefundTransactionResponseObj responseObj = new RefundTransactionResponseObj();
                        return responseObj.deserializeRefundTransactionResponse(apiService.apiResponse.getResponseData());
                    }

                    throw new Exception(apiService.errorResponse);
                }
            }
        }
    }
}