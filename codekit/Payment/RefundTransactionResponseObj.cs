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
            public class RefundTransactionResponseObj
            {
                /// <summary>
                /// deserialize RefundTransactionResponse json string to RefundTransactionResponse object
                /// </summary>
                /// <param name="json">json</param>
                public RefundTransactionResponseObj.RootObject deserializeRefundTransactionResponse(string json)
                {
                    var serializer = new JavaScriptSerializer();
                    return serializer.Deserialize<RefundTransactionResponseObj.RootObject>(json);
                }

                public class RootObject
                {
                    public bool IsSuccess { get; set; }
                    public string Version { get; set; }
                    public string TransactionId { get; set; }
                    public string TransactionStatus { get; set; }
                    public string OriginalPurchaseAmount { get; set; }
                    public string CommitConfirmationId { get; set; }
                }
            }
        }
    }
}
