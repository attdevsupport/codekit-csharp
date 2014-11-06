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
            public class TransactiontionPayloadObj
            {
                /// <summary>
                /// deserialize TransactionStatusResponse json string to TransactionStatusResponse object
                /// </summary>
                /// <param name="json">json</param>
                public TransactiontionPayloadObj.RootObject deserializeTransactiontionPayloadObj(string json)
                {
                    var serializer = new JavaScriptSerializer();
                    return serializer.Deserialize<TransactiontionPayloadObj.RootObject>(json);
                }

                public class RootObject
                {
                    public string Description { get; set; }
                    public int Category { get; set; }
                    public double Amount { get; set; }
                    public string Channel { get; set; }
                    public string MerchantPaymentRedirectUrl { get; set; }
                    public string MerchantProductId { get; set; }
                    public string MerchantTransactionId { get; set; }
                }
            }
        }
    }
}
