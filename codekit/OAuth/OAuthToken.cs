using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ATT.Codekit.Authorization
{
    public class OAuthToken
    {
        /// <summary>
        /// Deserialize json and get the access token
        /// </summary>
        public string getAccessToken(string json)
        {
            var serializer = new JavaScriptSerializer();
            var deserializedJsonObj = serializer.Deserialize<OAuthToken.AccessToken>(json);
            return deserializedJsonObj.access_token;
        }
        /// <summary>
        /// Class to hold access token response
        /// </summary>
        public class AccessToken
        {
            /// <summary>
            /// Gets or sets access token
            /// </summary>
            public string access_token { get; set;}
            
            /// <summary>
            /// Gets or sets refresh token
            /// </summary>
            public string refresh_token { get; set;}
            
            /// <summary>
            /// Gets or sets expires in
            /// </summary>
            public string expires_in { get; set;}

        }
    }
}
