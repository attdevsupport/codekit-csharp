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
    public class QuickContactObj
    {
        /// <summary>
        /// deserialize QuickContact json string to QuickContact object
        /// </summary>
        /// <param name="json">json</param>
        public QuickContactObj.RootObject deserializeQuickContactsObj(string json)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<QuickContactObj.RootObject>(json);
        }

        public QuickContactObj.RootObject deserializeQuickContactObj(string json)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<QuickContactObj.RootObject>(json);
        }

        public class Phone
        {
            public string type { get; set; }
            public string number { get; set; }
        }

        public class Address
        {
            public string type { get; set; }
            public string preferred { get; set; }
            public string poBox { get; set; }
            public string addressLine1 { get; set; }
            public string addressLine2 { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string zip { get; set; }
            public string country { get; set; }
        }

        public class Email
        {
            public string type { get; set; }
            public string emailAddress { get; set; }
        }

        public class Im
        {
            public string type { get; set; }
            public string imUri { get; set; }
        }

        public class QuickContact
        {
            public string contactId { get; set; }
            public string formattedName { get; set; }
            public string firstName { get; set; }
            public string middleName { get; set; }
            public string lastName { get; set; }
            public string prefix { get; set; }
            public string suffix { get; set; }
            public string nickName { get; set; }
            public string organization { get; set; }
            public Phone phone { get; set; }
            public Address address { get; set; }
            public Email email { get; set; }
            public Im im { get; set; }
        }

        public class QuickContacts
        {
            public List<QuickContact> quickContact { get; set; }
        }

        public class ResultSet
        {
            public string currentPageIndex { get; set; }
            public string totalRecords { get; set; }
            public string totalPages { get; set; }
            public string previousPage { get; set; }
            public string nextPage { get; set; }
            public QuickContacts quickContacts { get; set; }
        }

        public class RootObject
        {
            public ResultSet resultSet { get; set; }
            public QuickContact quickContact { get; set; }
        }
    }
}
