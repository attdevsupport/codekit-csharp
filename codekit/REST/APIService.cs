using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace ATT.Codekit.REST
{
    public class APIService
    {
        /// <summary>
        /// Gets or sets the error reponse from HTTP request
        /// </summary>
        public string errorResponse { get; set; }

        /// <summary>
        /// Gets or sets the end point 
        /// </summary>
        public string endPoint { get; set; }

        /// <summary>
        /// Gets or sets the access token
        /// </summary>
        public string accessToken { get; set; }

        /// <summary>
        /// Gets or sets the api partial segment
        /// </summary>
        public string apiPartialSegment { get; set; }

        /// <summary>
        /// Stores the API response object that came back from request
        /// </summary>
        public APIRequest apiRequest { get; set; }

        /// <summary>
        /// Stores the API response object that came back from request
        /// </summary>
        public APIResponse apiResponse { get; set; }

        /// <summary>
        /// Form the request Uri
        /// </summary>
        public string requestUri
        {
            get { return endPoint + apiPartialSegment + apiRequest.operationSegment; }
        }

        public APIService()
        {
            this.endPoint = string.Empty;
            this.apiPartialSegment = string.Empty;
            errorResponse = string.Empty;
            apiResponse = null;
        }

        public APIService(string endPoint, string apiPartialSegment)
        {
            this.endPoint = endPoint;
            this.apiPartialSegment = apiPartialSegment;
            errorResponse = string.Empty;
            apiResponse = null;
        }

        /// <summary>
        /// APIservice constructor
        /// </summary>
        /// <param name="apiPartialSegment">endPoint</param>
        /// <param name="accessToken">accessToken</param>
        /// <param name="endPoint">apiPartialSegment</param>
        public APIService(string endPoint, string accessToken, string apiPartialSegment)
        {
            this.accessToken = accessToken;
            this.endPoint = endPoint;
            this.apiPartialSegment = apiPartialSegment;
            errorResponse = string.Empty;
            apiResponse = null;
        }

        public bool post(APIRequest apiRequest)
        {
            this.apiRequest = apiRequest;
            return httpRequst("POST");
        }

        public bool get(APIRequest apiRequest)
        {
            this.apiRequest = apiRequest;
            return httpRequst("GET");
        }

        public bool put(APIRequest apiRequest)
        {
            this.apiRequest = apiRequest;
            return httpRequst("PUT");
        }

        public bool delete(APIRequest apiRequest)
        {
            this.apiRequest = apiRequest;
            return httpRequst("DELETE");
        }

        public bool patch(APIRequest apiRequest)
        {
            this.apiRequest = apiRequest;
            return httpRequst("PATCH");
        }


        public bool httpRequst(string action)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(requestUri);

                if (apiRequest.requireAccessToken)
                {
                    request.Headers.Add("Authorization", "Bearer " + this.accessToken);
                }

                foreach (KeyValuePair<string, string> header in apiRequest.headers)
                {
                    request.Headers.Add(header.Key, header.Value);
                }
                
                request.Accept = apiRequest.accept;
                request.Method = action;
                request.ContentType = apiRequest.contentType;
                request.KeepAlive = apiRequest.keepAlive;
                request.SendChunked = apiRequest.SendChunked;
                request.UserAgent = apiRequest.userAgent;

                if (apiRequest.binaryData.Length > 0)
                {
                    request.ContentLength = apiRequest.binaryData.Length;
                    Stream dataStream;
                    dataStream = request.GetRequestStream();
                    dataStream.Write(apiRequest.binaryData, 0, apiRequest.binaryData.Length);
                    dataStream.Close();
                }
                else
                {
                    request.ContentLength = 0;
                }
                

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                apiResponse = new APIResponse(response);

            }
            catch (WebException we)
            {
                errorResponse = getWebException(we);
                return false;
            }
            catch (Exception ex)
            {
                errorResponse = ex.Message;
                return false;
            }
            return true;
        }

        public string getWebException(WebException we)
        {
            string errorResponse;
            try
            {
                using (StreamReader sr = new StreamReader(we.Response.GetResponseStream()))
                {
                    errorResponse = sr.ReadToEnd();
                    sr.Close();
                }
            }
            catch
            {
                errorResponse = "Unable to get response: " + we.Message;
            }
            return errorResponse;
        }
    }
}
