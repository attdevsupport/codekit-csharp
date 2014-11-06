using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ATT.Codekit.Messaging
{
    /// <summary>
    /// Create OutboundSMSRequest object
    /// </summary>
    public class OutboundSMSRequestObj
    {
        /// <summary>
        /// deserialize OutboundSMSRequest json string to OutboundSMSRequest object
        /// </summary>
        /// <param name="json">json</param>
        public OutboundSMSRequestObj deserializeOutboundSMSRequest(string json)
        {
            var serializer = new JavaScriptSerializer();
            var deserializedJsonObj = serializer.Deserialize<OutboundSMSRequestObj>(json);
            return deserializedJsonObj;
        }

        public class OutboundSMSRequest
        {
            public string address { get; set; }
            public string message { get; set; }
        }

        public class RootObject
        {
            public OutboundSMSRequest outboundSMSRequest { get; set; }
        }
    }
}
