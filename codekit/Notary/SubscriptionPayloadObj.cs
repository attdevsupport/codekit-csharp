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
            public class SubscriptionPayloadObj
            {

                /// <summary>
                /// deserialize SubscriptionPayload json string to SubscriptionPayload object
                /// </summary>
                /// <param name="json">json</param>
                public SubscriptionPayloadObj.RootObject deserializeSubscriptionPayloadObj(string json)
                {
                    var serializer = new JavaScriptSerializer();
                    return serializer.Deserialize<SubscriptionPayloadObj.RootObject>(json);
                }

                public class RootObject
                {
                    public double Amount { get; set; }
                    public int Category { get; set; }
                    public string Channel { get; set; }
                    public string Description { get; set; }
                    public string MerchantTransactionId { get; set; }
                    public string MerchantProductId { get; set; }
                    public string MerchantPaymentRedirectUrl { get; set; }
                    public string MerchantSubscriptionIdList { get; set; }
                    public bool IsPurchaseOnNoActiveSubscription { get; set; }
                    public int SubscriptionRecurrences { get; set; }
                    public string SubscriptionPeriod { get; set; }
                    public int SubscriptionPeriodAmount { get; set; }
                }
            }
        }
    }
}
