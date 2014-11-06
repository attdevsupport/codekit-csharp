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
    public class GroupObj
    {
        /// <summary>
        /// deserialize Group json string to Group object
        /// </summary>
        /// <param name="json">json</param>
        public GroupObj.RootObject deserializeGroupObj(string json)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<GroupObj.RootObject>(json);
        }

        public class Group
        {
            public string groupId { get; set; }
            public string groupName { get; set; }
            public string groupType { get; set; }
        }

        public class Groups
        {
            public List<Group> group { get; set; }
        }

        public class ResultSet
        {
            public string currentPageIndex { get; set; }
            public string totalRecords { get; set; }
            public string totalPages { get; set; }
            public string previousPage { get; set; }
            public string nextPage { get; set; }
            public Groups groups { get; set; }
        }

        public class RootObject
        {
            public ResultSet resultSet { get; set; }
            public Group group { get; set; }
        }
    }
}
