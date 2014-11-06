using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using ATT.Codekit.REST;

namespace ATT.Codekit.Messaging
{
    public class MMS
    {
        /// <summary>
        /// APIService instance
        /// </summary>
        public APIService apiService { get; set; }

        /// <summary>
        /// JavaScriptSerializer 
        /// </summary>
        public JavaScriptSerializer serializer { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public MMS(string endPoint, string accessToken)
        {
            apiService = new APIService(endPoint, accessToken, "/mms/v3/messaging/");
        }

        /// <summary>
        /// This function makes API call to get MMS delivery status
        /// </summary>
        public DeliveryInfoObj.RootObject getMMSDeliveryStatus(string messageID)
        {
            APIRequest apiRequest = new APIRequest("outbox/" + messageID);

            apiRequest.accept = "application/json";

            if (apiService.get(apiRequest))
            {
                DeliveryInfoObj ro = new DeliveryInfoObj();
                return ro.deserializeDeliveryInfo(apiService.apiResponse.getResponseData());
            }

            return null;
        }

        /// <summary>
        /// This function makes API call to send MMS
        /// </summary>
        public OutboundMessageResponseObj.RootObject sendMMS(string mmsAddress, string mmsMessage, string mmsFile, string mmsContentType)
        {
            APIRequest apiRequest = new APIRequest("outbox");

            string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");

            apiRequest.accept = "application/json";
            apiRequest.contentType = "multipart/related; type=\"application/x-www-form-urlencoded\"; start=\"<startpart>\"; boundary=\"" + boundary + "\"\r\n";
            apiRequest.binaryData = setFinalByte(mmsAddress, mmsMessage, mmsFile, mmsContentType, false, boundary);

            if (apiService.post(apiRequest))
            {
                OutboundMessageResponseObj ro = new OutboundMessageResponseObj();
                return ro.deserializeOutboundSMSResponse(apiService.apiResponse.getResponseData());
            }

            throw new Exception(apiService.errorResponse);
        }

        private byte[] setFinalByte(string mmsAddress, string mmsMessage, string mmsFile, string mmsContentType, bool mmsNotify, string boundary)
        {
            UTF8Encoding mmsEncoding = new UTF8Encoding();

            byte[] totalpostBytes = mmsEncoding.GetBytes(string.Empty);
            string sendMMSData = "address=" + mmsAddress + "&subject=" + mmsMessage + "&notifyDeliveryStatus=" + mmsNotify;

            string data = string.Empty;
            data += "--" + boundary;
            data += "\r\nContent-Type: application/x-www-form-urlencoded;charset=UTF-8";  
            data += "\r\nContent-Transfer-Encoding: 8bit";
            data += "\r\nContent-Disposition: form-data; name=\"root-fields\"";
            data += "\r\nContent-ID:<startpart>\r\n\r\n" + sendMMSData;

            totalpostBytes = FormMIMEParts(boundary, data, mmsFile, mmsContentType);

            byte[] byteLastBoundary = mmsEncoding.GetBytes("\r\n--" + boundary + "--\r\n");
            int totalSize = totalpostBytes.Length + byteLastBoundary.Length;

            var totalMS = new MemoryStream(new byte[totalSize], 0, totalSize, true, true);
            totalMS.Write(totalpostBytes, 0, totalpostBytes.Length);
            totalMS.Write(byteLastBoundary, 0, byteLastBoundary.Length);

            return totalMS.GetBuffer();
        }

        /// <summary>
        /// Form mime parts for the user input files
        /// </summary>
        /// <param name="boundary">string, boundary data</param>
        /// <param name="data">string, mms message</param>
        /// <returns>returns byte array of files</returns>
        private byte[] FormMIMEParts(string boundary, string data, string mmsFile, string contentType)
        {
            UTF8Encoding encoding = new UTF8Encoding();

            byte[] postBytes = encoding.GetBytes(string.Empty);
            byte[] totalpostBytes = encoding.GetBytes(string.Empty);

            byte[] Head = encoding.GetBytes(data);
            int totalSizeWithHead = totalpostBytes.Length + Head.Length;

            var totalMSWithHead
                = new MemoryStream(new byte[totalSizeWithHead], 0, totalSizeWithHead, true, true);

            totalMSWithHead.Write(totalpostBytes, 0, totalpostBytes.Length);
            totalMSWithHead.Write(Head, 0, Head.Length);
            totalpostBytes = totalMSWithHead.GetBuffer();
            string filename = Path.GetFileName(mmsFile);
            postBytes = this.GetBytesOfFile(boundary, mmsFile, filename, contentType);
            var msOne = this.JoinTwoByteArrays(totalpostBytes, postBytes);
            totalpostBytes = msOne.GetBuffer();

            return totalpostBytes;
        }

        // <summary>
        /// This function adds two byte arrays
        /// </summary>
        /// <param name="firstByteArray">first array of bytes</param>
        /// <param name="secondByteArray">second array of bytes</param>
        /// <returns>returns MemoryStream after joining two byte arrays</returns>
        private MemoryStream JoinTwoByteArrays(byte[] firstByteArray, byte[] secondByteArray)
        {
            int newSize = firstByteArray.Length + secondByteArray.Length;
            var ms = new MemoryStream(new byte[newSize], 0, newSize, true, true);
            ms.Write(firstByteArray, 0, firstByteArray.Length);
            ms.Write(secondByteArray, 0, secondByteArray.Length);
            return ms;
        }

        // <summary>
        /// Gets the bytes representation of file along with mime part
        /// </summary>
        /// <param name="boundary">string, boundary message</param>
        /// <param name="data">string, mms message</param>
        /// <param name="filePath">string, filepath</param>
        /// <returns>byte[], representation of file in bytes</returns>
        private byte[] GetBytesOfFile(string boundary, string filePath, string filename, string contentType)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] postBytes = encoding.GetBytes(string.Empty);
            FileStream fileStream = null;
            BinaryReader binaryReader = null;

            try
            {
                string attachmentContentType = contentType;
                fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                binaryReader = new BinaryReader(fileStream);

                byte[] image = binaryReader.ReadBytes((int)fileStream.Length);

                string data = "\r\n--" + boundary + "\r\n";
                data += "Content-Disposition: attachment; filename=" + filename + "\r\n";
                data += "Content-Type: " + attachmentContentType + ";name=" + filename + "\r\n";
                data += "Content-ID: " + filename + "\r\n";
                data += "Content-Transfer-Encoding: Binary\r\n\r\n";

                byte[] firstPart = encoding.GetBytes(data);
                int newSize = firstPart.Length + image.Length;

                var memoryStream = new MemoryStream(new byte[newSize], 0, newSize, true, true);
                memoryStream.Write(firstPart, 0, firstPart.Length);
                memoryStream.Write(image, 0, image.Length);

                postBytes = memoryStream.GetBuffer();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (null != binaryReader)
                {
                    binaryReader.Close();
                }

                if (null != fileStream)
                {
                    fileStream.Close();
                }
            }

            return postBytes;
        }
    }
}
