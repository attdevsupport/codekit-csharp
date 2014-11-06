using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;


namespace ATT.Codekit.AAB
{
    /// <summary>
    /// Summary description for QuickContact
    /// </summary>
    public class ContactIdObj
    {
        /// <summary>
        /// deserialize ContactId json string to ContactId object
        /// </summary>
        /// <param name="json">json</param>
        public ContactIdObj.RootObject deserializeContactIdObj(string json)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<ContactIdObj.RootObject>(json);
        }
        
        public class ContactIds
        {
            public List<string> id { get; set; }
        }

        public class RootObject
        {
            public ContactIds contactIds { get; set; }
        }
    }
}
