using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ATT.Codekit.ADS
{
    /// <summary>
    /// create Recognition object
    /// </summary>
    public class AdsObj
    {
        /// <summary>
        /// deserialize Recognition json string to Recognition object
        /// </summary>
        /// <param name="json">json</param>
        public AdsObj.RootObject deserializeAdsObj(string json)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<AdsObj.RootObject>(json);
        }

        public class Text
        {
        }

        public class Ads
        {
            public string Type { get; set; }
            public string ClickUrl { get; set; }
            public string TrackUrl { get; set; }
            public Text Text { get; set; }
            public string Content { get; set; }
        }

        public class AdsResponse
        {
            public Ads Ads { get; set; }
        }

        public class RootObject
        {
            public AdsResponse AdsResponse { get; set; }
        }
    }
}
