using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ATT.Codekit.IAM
{
    public class MessageObj
    {
        /// <summary>
        /// deserialize MessageObj json string to Messag list object
        /// </summary>
        /// <param name="json">json</param>
        public MessageObj.MessageList deserializeMessageListObj(string json)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<MessageObj.MessageList>(json);
        }

        /// <summary>
        /// deserialize MessageObj json string to Messag object
        /// </summary>
        /// <param name="json">json</param>
        public MessageObj.Message deserializeMessageObj(string json)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<MessageObj.Message>(json);
        }

        public class From
        {
            public string value { get; set; }
        }

        public class Recipient
        {
            public string value { get; set; }
        }

        public class TypeMetaData
        {
            public string subject { get; set; }
        }

        public class MmsContent
        {
            public string contentType { get; set; }
            public string contentName { get; set; }
            public string contentUrl { get; set; }
            public string type { get; set; }
        }
        public class Message
        {
            public string messageId { get; set; }
            public From from { get; set; }
            public List<Recipient> recipients { get; set; }
            public string timeStamp { get; set; }
            public bool isUnread { get; set; }
            public string type { get; set; }
            public TypeMetaData typeMetaData { get; set; }
            public bool isIncoming { get; set; }
            public List<MmsContent> mmsContent { get; set; }
        }

        public class MessageList
        {
            public List<Message> messages { get; set; }
            public int offset { get; set; }
            public int limit { get; set; }
            public int total { get; set; }
            public string state { get; set; }
            public string cacheStatus { get; set; }
            public List<string> failedMessages { get; set; }
        }

        public class RootObject
        {
            public Message message { get; set; }
            public List<Message> messages { get; set; }
            public MessageList messageList { get; set; }

        }
    }
}