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
    public class OutboundSMSResponseObj
    {
        /// <summary>
        /// deserialize OutboundSMSResponse json string to deserializeOutboundSMSResponse object
        /// </summary>
        /// <param name="json">json</param>
        public OutboundSMSResponseObj.RootObject deserializeOutboundSMSResponse(string json)
        {
            var serializer = new JavaScriptSerializer();
            var deserializedJsonObj = serializer.Deserialize<OutboundSMSResponseObj.RootObject>(json);
            return deserializedJsonObj;
        }

        public class ResourceReference
        {
            public string resourceURL { get; set; }
        }

        public class OutboundSMSResponse
        {
            public string messageId { get; set; }
            public ResourceReference resourceReference { get; set; }
        }

        public class RootObject
        {
            public OutboundSMSResponse outboundSMSResponse { get; set; }
        }
    }
}
