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
            public class SubscriptionDetailResponseObj
            {
                /// <summary>
                /// deserialize SubscriptionDetailResponse json string to SubscriptionDetailResponse object
                /// </summary>
                /// <param name="json">json</param>
                public SubscriptionDetailResponseObj.RootObject deserializeSubscriptionDetailResponse(string json)
                {
                    var serializer = new JavaScriptSerializer();
                    return serializer.Deserialize<SubscriptionDetailResponseObj.RootObject>(json);
                }

                public class RootObject
                {
                    public string Currency { get; set; }
                    public string Status { get; set; }
                    public string CreationDate { get; set; }
                    public double GrossAmount { get; set; }
                    public int Recurrences { get; set; }
                    public bool IsActiveSubscription { get; set; }
                    public string CurrentStartDate { get; set; }
                    public string CurrentEndDate { get; set; }
                    public long RecurrencesLeft { get; set; }
                    public string Version { get; set; }
                    public bool IsSuccess { get; set; }
                }
            }
        }
    }
}
