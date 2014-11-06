using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using ATT.Codekit.REST;


namespace ATT.Codekit.SPEECH
{
    public class Speech
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
        public Speech(string endPoint, string accessToken)
        {
            apiService = new APIService(endPoint, accessToken, "/speech/v3/");
        }

        /// <summary>
        /// This funtion makes API call to get SMS
        /// </summary>
        public RecognitionObj.RootObject speechToText(string parXspeechContext,
                                                        string parXArgs,
                                                        string parSpeechFilePath,
                                                        bool parChunked)
        {
            var audioFileStream = new FileStream(parSpeechFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(audioFileStream);
            byte[] binaryData = reader.ReadBytes((int)audioFileStream.Length);
            reader.Close();
            audioFileStream.Close();

            if (binaryData.Length > 0)
            {
                APIRequest apiRequest = new APIRequest("speechToText");

                apiRequest.accept = "application/json";
                apiRequest.addHeaders("X-SpeechContext", parXspeechContext);
                apiRequest.addHeaders("X-Arg", parXArgs);
                apiRequest.contentType = "audio/wav";
                apiRequest.accept = "application/json";
                apiRequest.SendChunked = parChunked;
                apiRequest.binaryData = binaryData;

                if (apiService.post(apiRequest))
                {
                    RecognitionObj robj = new RecognitionObj();
                    return robj.deserializeRecognition(apiService.apiResponse.getResponseData());
                }
                throw new Exception(apiService.errorResponse);
            }
            throw new Exception("No audio data.");
        }

        /// <summary>
        /// This funtion makes API call to get SMS
        /// </summary>
        public RecognitionObj.RootObject speechToTextCustom(string mimeData,
                                                            string XspeechContext,
                                                            string XArgs,
                                                            string SpeechFilePath,
                                                            string xdictionaryContent,
                                                            string xgrammerContent)
        {
            
            
            APIRequest apiRequest = new APIRequest("speechToTextCustom");

            string boundary = "----------------------------" + DateTime.Now.Ticks.ToString("x");

            apiRequest.accept = "application/json";
            apiRequest.addHeaders("Content-Language", "en-us");
            apiRequest.addHeaders("X-Arg", XArgs);
            apiRequest.contentType = "multipart/x-srgs-audio; " + "boundary=" + boundary;
            apiRequest.accept = "application/json";
            apiRequest.binaryData = getTotalByte(boundary, xdictionaryContent, SpeechFilePath, xgrammerContent);


            if (apiService.post(apiRequest))
            {
                RecognitionObj robj = new RecognitionObj();
                return robj.deserializeRecognition(apiService.apiResponse.getResponseData());
            }

            throw new Exception(apiService.errorResponse);
        }

        /// <summary>
        /// This funtion makes API call to send SMS
        /// </summary>
        public byte[] textToSpeech(string data)
        {
            APIRequest apiRequest = new APIRequest("textToSpeech");

            apiRequest.accept = "audio/amr-wb";
            apiRequest.contentType = "text/plain";
            apiRequest.setBinaryData(data);

            if (apiService.post(apiRequest))
            {
                return apiService.apiResponse.getResponseBinary();
            }
            return null;
        }

        private byte[] getTotalByte(string boundary, string xdictionaryContent, string SpeechFilePath, string xgrammerContent)
        {
            FileStream audioFileStream = null;
            audioFileStream = new FileStream(SpeechFilePath, FileMode.Open, FileAccess.Read);
            BinaryReader reader = new BinaryReader(audioFileStream);
            
            byte[] binaryData = reader.ReadBytes((int)audioFileStream.Length);

            string contentType = this.MapContentTypeFromExtension(Path.GetExtension(SpeechFilePath));

            string data = string.Empty;
            data += "--" + boundary;
            data += "\r\nContent-Disposition: form-data; name=\"x-dictionary\"; filename=\"speech_alpha.pls\"";
            data += "\r\nContent-Type: application/pls+xml\r\n";
            data += "\r\n" + xdictionaryContent + "\r\n\r\n";
            
            data += "\r\n--" + boundary;
            data += "\r\n" + "Content-Disposition: form-data; name=\"x-grammar\"";
            data += "\r\nContent-Type: application/srgs+xml \r\n";
            data += "\r\n" + xgrammerContent + "\r\n\r\n";
            
            data += "\r\n--" + boundary;
            data += "\r\nContent-Disposition: form-data; name=\"x-voice\"; filename=\"" + SpeechFilePath + "\"";
            data += "\r\nContent-Type: " + contentType + "\r\n\r\n";
            
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] firstPart = encoding.GetBytes(data);
            int newSize = firstPart.Length + binaryData.Length;

            var memoryStream = new MemoryStream(new byte[newSize], 0, newSize, true, true);
            memoryStream.Write(firstPart, 0, firstPart.Length);
            memoryStream.Write(binaryData, 0, binaryData.Length);

            byte[] postBytes = memoryStream.GetBuffer();

            byte[] byteLastBoundary = encoding.GetBytes("\r\n\r\n" + "--" + boundary + "--");
            int totalSize = postBytes.Length + byteLastBoundary.Length;

            var totalMS = new MemoryStream(new byte[totalSize], 0, totalSize, true, true);
            totalMS.Write(postBytes, 0, postBytes.Length);
            totalMS.Write(byteLastBoundary, 0, byteLastBoundary.Length);

            return totalMS.GetBuffer();
        }

        private string MapContentTypeFromExtension(string extension)
        {
            Dictionary<string, string> extensionToContentTypeMapping
                = new Dictionary<string, string>()
                        {
                            { ".amr", "audio/amr" }, { ".wav", "audio/wav" }, {".awb", "audio/amr-wb"}, {".spx", "audio/x-speex"}
                        };

            if (extensionToContentTypeMapping.ContainsKey(extension))
            {
                return extensionToContentTypeMapping[extension];
            }
            else
            {
                throw new ArgumentException("invalid attachment extension");
            }
        }


    }
}
