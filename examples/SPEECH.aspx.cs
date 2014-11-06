using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Script.Serialization;
using System.IO;
/// <summary>
///  This Quickstart Guide for the Speech API requires the C# code kit, 
///  which can be found at: https://github.com/attdevsupport/codekit-csharp
///  Make sure that dlls are compiled and referenced in the project.
/// </summary>
using ATT.Codekit.SPEECH;
using ATT.Codekit.Authorization;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // Enter the value from the 'App Key' field obtained at developer.att.com in your 
        // app account.
        string clientId = "";

        // Enter the value from the 'App Secret' field obtained at developer.att.com 
        // in your app account.
        string secretKey = "";

        // Set the fully-qualified domain name to: https://api.att.com
        string fqdn = "https://api.att.com";

        // Set the scope to SPEECH,STTC,TTS
        string scope = "SPEECH,STTC,TTS";

        // Create the service for requesting an OAuth access token.
        var oauth = new OAuth(fqdn, clientId, secretKey, scope);

        // Get the OAuth access token using the OAuth Client Credentials.
        if (oauth.GetAccessToken(OAuth.AccessTokenType.ClientCredentials))
        {
            // Get access token 
            OAuthToken at = new OAuthToken();
            string accessToken = at.getAccessToken(oauth.accessTokenJson);

            // Create the service for making the method request.
            var speech = new Speech(fqdn, accessToken);

            //*************************************************************************
            // Operation: speech To Text

            // Set params:
            string sttXspeechContext = "BusinessSearch";
            string sttXArgs = "ClientApp=NoteTaker,ClientVersion=1.0.1,DeviceType=Android";
            string sttSpeechFilePath = Server.MapPath("~/") + "audio/BostonCeltics.wav";
            bool sttChunked = true;

            try
            {
                // Make an Make a method call to the Speech To Text API.
                // Method takes:
                // Param 1: speechContext
                // Param 2: XArgs
                // Param 3: SpeechFilePath
                // Param 4: Chunked
                RecognitionObj.RootObject speachToTextResponseObj 
                    = speech.speechToText(sttXspeechContext, 
                                            sttXArgs, 
                                            sttSpeechFilePath, 
                                            sttChunked);
            }
            catch (Exception respex)
            {
                string error = respex.StackTrace;
            }

            //*************************************************************************
            // Operation: Speech To Text Custom

            // Set params:
            string mimeData = System.IO.File.ReadAllText(Server.MapPath("~/") + "template/x-dictionary.txt");
            string sttcXspeechContext = "GrammarList";
            string sttcXArgs = "GrammarPenaltyPrefix=1.1,GrammarPenaltyGeneric=2.0,GrammarPenaltyAltgram=4.1";
            string sttcSpeechFilePath = Server.MapPath("~/") + "audio/pizza-en-US.wav";
            string xgrammerContent = string.Empty;
            string xdictionaryContent = string.Empty;

            StreamReader streamReader = new StreamReader(Server.MapPath("~/") + "template/x-dictionary.txt");
            xdictionaryContent = streamReader.ReadToEnd();
            mimeData = "x-dictionary:" + Environment.NewLine + xdictionaryContent;
            
            StreamReader streamReader1 = new StreamReader(Server.MapPath("~/") + "template/x-grammer.txt");
            xgrammerContent = streamReader1.ReadToEnd();
            mimeData = mimeData + Environment.NewLine + "x-grammar:" + Environment.NewLine + xgrammerContent;
            
            streamReader.Close();
            streamReader1.Close();

            //make speech to text custom request
            try
            {
                // Make an Make a method call to Speech To Text Custom.
                // Method takes:
                // Param 1: mimeData
                // Param 2: speechContext
                // Param 3: XArgs
                // Param 4: SpeechFilePath
                // Param 5: xdictionaryContent
                // Param 5: xgrammerContent
                RecognitionObj.RootObject speachToTextResponseCObj
                    = speech.speechToTextCustom(mimeData,
                                                sttcXspeechContext,
                                                sttcXArgs,
                                                sttcSpeechFilePath,
                                                xdictionaryContent,
                                                xgrammerContent);
            }
            catch (Exception respex)
            {
                string error = respex.StackTrace;
            }

            //*************************************************************************
            // Operation: Text To Speech 

            try
            {
                // Make an Make a method call to Text to Speech.
                // Method takes:
                // Param 1: test string
                byte[] responseAudioData = speech.textToSpeech("test");
            }
            catch (Exception respex)
            {
                string error = respex.StackTrace;
            }
        }
    }
}
