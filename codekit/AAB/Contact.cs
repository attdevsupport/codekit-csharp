using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;

namespace ATT.Codekit.AAB
{
    /// <summary>
    /// Summary description for QuickContact
    /// </summary>

    public class ContactObj
    {
        /// <summary>
        /// deserialize Contact json string to Contact object
        /// </summary>
        /// <param name="json">json</param>
        public ContactObj.RootObject deserializeContactObj(string json)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<ContactObj.RootObject>(json);
        }

        
        public class Phone
        {
            public string type { get; set; }
            public string number { get; set; }
            public bool preferred { get; set; }
        }
        public class Phones
        {
            public List<Phone> phone { get; set; }
        }
        public class Address
        {
            public string type { get; set; }
            public bool preferred { get; set; }
            public string poBox { get; set; }
            public string addressLine1 { get; set; }
            public string addressLine2 { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string zipcode { get; set; }
            public string country { get; set; }
        }
        public class Addresses
        {
            public List<Address> address { get; set; }
        }
        public class Email
        {
            public string type { get; set; }
            public bool preferred { get; set; }
            public string emailAddress { get; set; }
        }
        public class Emails
        {
            public List<Email> email { get; set; }
        }
        public class Im
        {
            public string type { get; set; }
            public string imUri { get; set; }
            public bool preferred { get; set; }
        }
        public class Ims
        {
            public List<Im> im { get; set; }
        }
        public class WebUrl
        {
            public string type { get; set; }
            public string url { get; set; }
            public bool preferred { get; set; }
        }
        public class Weburls
        {
            public List<WebUrl> webUrl { get; set; }
        }
        public class Photo
        {
            public string encoding { get; set; }
            public string value { get; set; }
        }
        public class Contact
        {
            public string creationDate { get; set; }
            public string contactId { get; set; }
            public string modificationDate { get; set; }
            public string formattedName { get; set; }
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string prefix { get; set; }
            public string suffix { get; set; }
            public string nickName { get; set; }
            public string organization { get; set; }
            public string jobTitle { get; set; }
            public string anniversary { get; set; }
            public string gender { get; set; }
            public string spouse { get; set; }
            public string hobby { get; set; }
            public string assistant { get; set; }
            public Phones phones { get; set; }
            public Addresses addresses { get; set; }
            public Emails emails { get; set; }
            public Ims ims { get; set; }
            public Weburls weburls { get; set; }
            public Photo photo { get; set; }
        }
        public class RootObject
        {
            public Contact contact { get; set; }
            public Contact myInfo { get; set; }
        }
    }
}


