using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ATT.Codekit.SPEECH
{
    /// <summary>
    /// create Recognition object
    /// </summary>
    public class RecognitionObj
    {
        /// <summary>
        /// deserialize Recognition json string to Recognition object
        /// </summary>
        /// <param name="json">json</param>
        public RecognitionObj.RootObject deserializeRecognition(string json)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<RecognitionObj.RootObject>(json);
        }

        public class NBest
        {
            public string Hypothesis { get; set; }
            public string LanguageId { get; set; }
            public double Confidence { get; set; }
            public string Grade { get; set; }
            public string ResultText { get; set; }
            public List<string> Words { get; set; }
            public List<double> WordScores { get; set; }
        }

        public class Recognition
        {
            public string Status { get; set; }
            public string ResponseId { get; set; }
            public List<NBest> NBest { get; set; }
        }

        public class RootObject
        {
            public Recognition Recognition { get; set; }
        }
    }
}
