using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
namespace ATT
{
    namespace Codekit
    {
        namespace PAYMENT
        {
            public class NotaryResponse
            {
                /// <summary>
                /// deserialize NotaryResponse json string to NotaryResponse object
                /// </summary>
                /// <param name="json">json</param>
                public NotaryResponse.RootObject deserializeNotaryResponse(string json)
                {
                    var serializer = new JavaScriptSerializer();
                    return serializer.Deserialize<NotaryResponse.RootObject>(json);
                }

                public class RootObject
                {
                    public string SignedDocument { get; set; }
                    public string Signature { get; set; }
                }
            }
        }
    }
}