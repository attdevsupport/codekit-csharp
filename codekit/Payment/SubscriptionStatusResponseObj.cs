using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ATT
{
    namespace Codekit
    {
        namespace PAYMENT
        {
            public class SubscriptionStatusResponseObj
            {
                /// <summary>
                /// deserialize SubscriptionStatusResponse json string to SubscriptionStatusResponse object
                /// </summary>
                /// <param name="json">json</param>
                public SubscriptionStatusResponseObj.RootObject deserializeSubscriptionStatusResponse(string json)
                {
                    var serializer = new JavaScriptSerializer();
                    return serializer.Deserialize<SubscriptionStatusResponseObj.RootObject>(json);
                }

                public class RootObject
                {
                    public string Version { get; set; }
                    public string IsSuccess { get; set; }
                    public string Amount { get; set; }
                    public string Channel { get; set; }
                    public string Description { get; set; }
                    public string Currency { get; set; }
                    public string SubscriptionType { get; set; }
                    public string SubscriptionStatus { get; set; }
                    public string ConsumerId { get; set; }
                    public string MerchantTransactionId { get; set; }
                    public string MerchantApplicationId { get; set; }
                    public string SubscriptionId { get; set; }
                    public string OriginalTransactionId { get; set; }
                    public string ContentCategory { get; set; }
                    public string MerchantProductId { get; set; }
                    public string MerchantId { get; set; }
                    public string MerchantSubscriptionId { get; set; }
                    public string PeriodAmount { get; set; }
                    public string Recurrences { get; set; }
                    public string SubscriptionPeriod { get; set; }
                }
            }
        }
    }
}