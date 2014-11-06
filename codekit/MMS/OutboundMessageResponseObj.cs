using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ATT.Codekit.Messaging
{
    /// <summary>
    /// create OutboundMessageResponseObj object
    /// </summary>
    public class OutboundMessageResponseObj
    {
        /// <summary>
        /// deserialize outboundMessageResponse json string to outboundMessageResponse object
        /// </summary>
        /// <param name="json">json</param>
        public OutboundMessageResponseObj.RootObject deserializeOutboundSMSResponse(string json)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<OutboundMessageResponseObj.RootObject>(json);
        }

        public class ResourceReference
        {
            public string resourceURL { get; set; }
        }

        public class OutboundMessageResponse
        {
            public string messageId { get; set; }
            public ResourceReference resourceReference { get; set; }
        }

        public class RootObject
        {
            public OutboundMessageResponse outboundMessageResponse { get; set; }
        }
    }
}
