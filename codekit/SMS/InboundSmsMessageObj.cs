using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ATT.Codekit.Messaging
{
    /// <summary>
    /// create InboundSmsMessageObj object
    /// </summary>
    public class InboundSmsMessageObj
    {
        /// <summary>
        /// deserialize InboundSmsMessageList json string to InboundSmsMessageList object
        /// </summary>
        /// <param name="json">json</param>
        public InboundSmsMessageObj.RootObject deserializeMsgListRequest(string json)
        {
            var serializer = new JavaScriptSerializer();
            var deserializedJsonObj = serializer.Deserialize<InboundSmsMessageObj.RootObject>(json);
            return deserializedJsonObj;
        }
        public class InboundSmsMessage
        {
            public string MessageId { get; set; }
            public string Message { get; set; }
            public string SenderAddress { get; set; }
        }

        public class InboundSmsMessageList
        {
            public List<InboundSmsMessage> InboundSmsMessage { get; set; }
            public string NumberOfMessagesInThisBatch { get; set; }
            public string ResourceUrl { get; set; }
            public string TotalNumberOfPendingMessages { get; set; }
        }

        public class RootObject
        {
            public InboundSmsMessageList InboundSmsMessageList { get; set; }
        }
    }
}
