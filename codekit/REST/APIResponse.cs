using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace ATT
{
    namespace Codekit
    {
        namespace REST
        {
            public class APIResponse
            {
                public HttpWebResponse response { get; set; }
                public HttpStatusCode statusCode { get; set; }
                public string description { get; set; }
                public NameValueCollection headers { get; set; }
                public Stream stream { get; set; }
                public string lastModified { get; set; }
                public string location { get; set; }

                public APIResponse(HttpWebResponse response)
                {
                    this.response = response;
                    statusCode = response.StatusCode;
                    description = response.StatusDescription;
                    headers = response.Headers;
                    stream = response.GetResponseStream();
                    lastModified = response.GetResponseHeader("Date");

                    if (!String.IsNullOrEmpty(response.GetResponseHeader("Location")))
                    {
                        var locationHeader = response.GetResponseHeader("Location");
                        var locationUri = new Uri(locationHeader);
                        location = locationUri.Segments[locationUri.Segments.Length - 1];
                    }
                }

                public string getResponseData()
                {
                    using (StreamReader streamReader = new StreamReader(this.stream))
                    {
                        string responseData = streamReader.ReadToEnd();
                        streamReader.Close();
                        return responseData;
                    }
                }

                public byte[] getResponseBinary()
                {
                    int offset = 0;
                    int remaining = Convert.ToInt32(response.ContentLength);
                    byte[] receivedBytes;
                    using (var stream = response.GetResponseStream())
                    {
                        receivedBytes = new byte[response.ContentLength];
                        while (remaining > 0)
                        {
                            int read = stream.Read(receivedBytes, offset, remaining);
                            if (read <= 0)
                            {
                                throw new Exception(String.Format("End of stream reached with {0} bytes left to read", remaining));
                            }

                            remaining -= read;
                            offset += read;
                        }
                    }
                    return receivedBytes;
                }
            }
        }
    }
}
