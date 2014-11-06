using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ATT.Codekit.Messaging
{
    /// <summary>
    /// create OutboundSMSResponse object
    /// </summary>
    public class DeliveryInfoObj
    {
        /// <summary>
        /// deserialize DeliveryInfoList json string to DeliveryInfoList object
        /// </summary>
        /// <param name="json">json</param>
        public DeliveryInfoObj.RootObject deserializeDeliveryInfo(string json)
        {
            var serializer = new JavaScriptSerializer();
            var deserializedJsonObj = serializer.Deserialize<DeliveryInfoObj.RootObject>(json);
            return deserializedJsonObj;
        }
        public class DeliveryInfo
        {
            public string Id { get; set; }
            public string Address { get; set; }
            public string DeliveryStatus { get; set; }
        }

        public class DeliveryInfoList
        {
            public List<DeliveryInfo> DeliveryInfo { get; set; }
            public string ResourceUrl { get; set; }
        }

        public class RootObject
        {
            public DeliveryInfoList DeliveryInfoList { get; set; }
        }
    }
}
