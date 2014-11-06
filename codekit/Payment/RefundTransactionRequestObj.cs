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
            public class RefundTransactionRequestObj
            {
                /// <summary>
                /// deserialize RefundTransactionRequest json string to RefundTransactionRequest object
                /// </summary>
                /// <param name="json">json</param>
                public RefundTransactionRequestObj.RootObject deserializeSubscriptionDetailResponse(string json)
                {
                    var serializer = new JavaScriptSerializer();
                    return serializer.Deserialize<RefundTransactionRequestObj.RootObject>(json);
                }

                public class RootObject
                {
                    public string TransactionOperationStatus { get; set; }
                    public int RefundReasonCode { get; set; }
                    public string RefundReasonText { get; set; }
                }
            }
        }
    }
}