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
            public class TransactionStatusResponseObj
            {
                /// <summary>
                /// deserialize TransactionStatusResponse json string to TransactionStatusResponse object
                /// </summary>
                /// <param name="json">json</param>
                public TransactionStatusResponseObj.RootObject deserializeTransactionStatusResponse(string json)
                {
                    var serializer = new JavaScriptSerializer();
                    return serializer.Deserialize<TransactionStatusResponseObj.RootObject>(json);
                }

                public class RootObject
                {
                    public string Channel { get; set; }
                    public string Description { get; set; }
                    public string Currency { get; set; }
                    public string TransactionType { get; set; }
                    public string TransactionStatus { get; set; }
                    public string ConsumerId { get; set; }
                    public string MerchantTransactionId { get; set; }
                    public string MerchantApplicationId { get; set; }
                    public string TransactionId { get; set; }
                    public string OriginalTransactionId { get; set; }
                    public string ContentCategory { get; set; }
                    public string MerchantProductId { get; set; }
                    public string MerchantId { get; set; }
                    public string Amount { get; set; }
                    public string Version { get; set; }
                    public string IsSuccess { get; set; }
                }
            }
        }
    }
}