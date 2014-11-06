using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ATT.Codekit.IAM
{
    public class MessageRequestObj
    {
        /// <summary>
        /// deserialize MessageRequestObj json string to MessageRequest object
        /// </summary>
        /// <param name="json">json</param>
        public MessageRequestObj.RootObject deserializeMessageRequestObj(string json)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<MessageRequestObj.RootObject>(json);
        }

        public class MessageRequest
        {
            public List<string> addresses { get; set; }
            public string text { get; set; }
        }

        public class RootObject
        {
            public MessageRequest messageRequest { get; set; }
        }
    }
}
