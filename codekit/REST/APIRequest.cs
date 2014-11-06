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
    public class APIRequest
    {
        /// <summary>
        /// Gets or sets the operationSegment
        /// </summary>
        public string operationSegment { get; set; }

        /// <summary>
        /// headers
        /// </summary>
        public Dictionary<string, string> headers { get; set; }

        /// <summary>
        /// parameters
        /// </summary>
        public Dictionary<string, string> parameters { get; set; }

        /// <summary>
        /// contentType
        /// </summary>
        public string contentType { get; set; }

        /// <summary>
        /// accept
        /// </summary>
        public string accept { get; set; }

        /// <summary>
        /// Request binaryData
        /// </summary>
        public byte[] binaryData { get; set; }

        /// <summary>
        /// Request keepAlive
        /// </summary>
        public bool keepAlive { get; set; }

        /// <summary>
        /// Request SendChunked
        /// </summary>
        public bool SendChunked { get; set; }

        /// <summary>
        /// Request User Agent
        /// </summary>
        public string userAgent { get; set; }

        /// <summary>
        /// Request User Agent
        /// </summary>
        public bool requireAccessToken { get; set; }

        /// <summary>
        /// APIRequest Constructor 
        /// </summary>
        public APIRequest(string operationSegment)
        {
            headers = new Dictionary<string, string>();
            parameters = new Dictionary<string, string>();
            this.operationSegment = operationSegment;
            binaryData = new byte[0];
            keepAlive = true;
            SendChunked = false;
            requireAccessToken = true;
        }

        public void setBinaryData(string data)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            binaryData =  encoding.GetBytes(data);
        }


        /// <summary>
        /// addHeaders
        /// </summary>
        public APIRequest addHeaders(string name, string value)
        {
            headers.Add(name, value);
            return this;
        }

        /// <summary>
        /// addParams
        /// </summary>
        public APIRequest addParams(string name, string value)
        {
            parameters.Add(name, value);
            return this;
        }
    }
}
