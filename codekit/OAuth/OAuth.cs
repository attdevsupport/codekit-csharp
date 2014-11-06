using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using ATT.Codekit.REST;

namespace ATT.Codekit.Authorization
{
    public class OAuth
    {
        public string getAuthCodeError { get; set; }
        public string getReadConfigFileError { get; set; }
        public string GetAccessTokenError { get; set; }
        public string CheckAccessTokenError { get; set; }
        public string endPoint { get; set; }
        public string scope { get; set; }
        public string apiKey { get; set; }
        public string authorizeRedirectUri { get; set; }
        public bool ignoressl { get; set; }
        public string authCode { get; set; }
        public string secretKey { get; set; }
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
        public string accessTokenExpiryTime { get; set; }
        public string refreshTokenExpiryTime { get; set; }
        public int refreshTokenExpiresIn { get; set; }
        public string accessTokenJson { get; set; }

        public APIService apiService { get; set; }

        public enum AccessTokenType
        {
            AuthorizationCode,
            RefreshToken,
            ClientCredentials
        }

        public enum TokenTypeHint
        {
            AccessToken,
            RefreshToken
        }

        /// <summary>
        /// OAuth constructor
        /// </summary>
        /// <param name="endPoint">endPoint</param>
        /// <param name="apiKey">apiKey</param>
        /// <param name="secretKey">secretKey</param>
        /// <param name="scope">scope</param>
        public OAuth(string endPoint, string apiKey, string secretKey, string scope)
        {
            this.endPoint = endPoint;
            this.scope = scope;
            this.apiKey = apiKey;
            this.secretKey = secretKey;
            this.refreshTokenExpiresIn = 24;
            this.apiService = new APIService(endPoint, "/oauth/v4/");
        }

        /// <summary>
        /// OAuth constructor
        /// </summary>
        /// <param name="endPoint">endPoint</param>
        /// <param name="apiKey">apiKey</param>
        /// <param name="secretKey">secretKey</param>
        /// <param name="authorizeRedirectUri">authorizeRedirectUri</param>
        /// <param name="scope">scope</param>
        public OAuth(string endPoint, string apiKey, string secretKey, string authorizeRedirectUri, string scope)
        {
            this.endPoint = endPoint;
            this.scope = scope;
            this.apiKey = apiKey;
            this.secretKey = secretKey;
            this.authorizeRedirectUri = authorizeRedirectUri;
            this.refreshTokenExpiresIn = 24;
            this.apiService = new APIService(endPoint, "/oauth/v4/");
        }

        /// <summary>
        /// OAuth constructor
        /// </summary>
        /// <param name="endPoint">endPoint</param>
        /// <param name="scope">scope</param>
        /// <param name="apiKey">apiKey</param>
        /// <param name="secretKey">secretKey</param>
        /// <param name="authorizeRedirectUri">authorizeRedirectUri</param>
        /// <param name="refreshTokenExpiresIn">refreshTokenExpiresIn</param>
        /// <param name="bypassSSL">bypassSSL</param>
        public OAuth(string endPoint,
                    string scope,
                    string apiKey,
                    string secretKey,
                    string authorizeRedirectUri,
                    int refreshTokenExpiresIn,
                    string bypassSSL)
        {
            this.endPoint = endPoint;
            this.scope = scope;
            this.apiKey = apiKey;
            this.secretKey = secretKey;
            this.authorizeRedirectUri = authorizeRedirectUri;
            this.refreshTokenExpiresIn = refreshTokenExpiresIn;
            this.ignoressl = BypassCertificateError(bypassSSL);
            this.apiService = new APIService(endPoint, "/oauth/v4/");
        }

        /// <summary>
        /// This function gets the Auth token 
        /// </summary>
        public bool GetAuthCode()
        {
            try
            {
                var builder = new UriBuilder(endPoint + "/oauth/v4/authorize");
                var query = HttpUtility.ParseQueryString(builder.Query);

                query["scope"] = scope;
                query["client_id"] = apiKey;
                query["redirect_uri"] = authorizeRedirectUri;
                builder.Query = query.ToString();
                string url = builder.ToString();

                HttpContext.Current.Response.Redirect(url);

            }
            catch (Exception ex)
            {
                this.getAuthCodeError = ex.Message;
                return false;
            }
            return true;
        }

        /// <summary>
        /// This function checks if token is valid
        /// </summary>
        public string IsTokenValid()
        {
            if (this.accessToken == null)
            {
                return "INVALID_ACCESS_TOKEN";
            }
            try
            {
                DateTime currentTime = DateTime.UtcNow.ToLocalTime();

                if (currentTime >= DateTime.Parse(accessTokenExpiryTime) &&
                    currentTime >= DateTime.Parse(refreshTokenExpiryTime))
                {
                    return "INVALID_ACCESS_TOKEN";
                }

                if (currentTime >= DateTime.Parse(accessTokenExpiryTime) &&
                currentTime < DateTime.Parse(refreshTokenExpiryTime))
                {
                    return "REFRESH_TOKEN";
                }

                if (currentTime < DateTime.Parse(accessTokenExpiryTime))
                {
                    return "VALID_ACCESS_TOKEN";
                }

                return "INVALID_ACCESS_TOKEN";
            }
            catch
            {
                return "INVALID_ACCESS_TOKEN";
            }
        }

        /// <summary>
        /// This function gets access token with post request
        /// </summary>
        public bool GetAccessToken(AccessTokenType type)
        {
            AccessTokenType t = type;

            var query = HttpUtility.ParseQueryString(String.Empty);

            string oauthParameters = String.Empty;

            switch (t)
            {
                case AccessTokenType.AuthorizationCode:

                    query["grant_type"] = "authorization_code";
                    query["client_id"] = apiKey;
                    query["client_secret"] = secretKey;
                    query["code"] = authCode;

                    break;

                case AccessTokenType.RefreshToken:

                    query["grant_type"] = "refresh_token";
                    query["client_id"] = apiKey;
                    query["client_secret"] = secretKey;
                    query["refresh_token"] = refreshToken;

                    break;

                case AccessTokenType.ClientCredentials:

                    query["grant_type"] = "client_credentials";
                    query["client_id"] = apiKey;
                    query["client_secret"] = secretKey;
                    query["scope"] = scope;

                    break;
            }
            oauthParameters = query.ToString();

            if (!String.IsNullOrEmpty(oauthParameters))
            {
                var apiRequest = new APIRequest("token");

                apiRequest.requireAccessToken = false;
                apiRequest.contentType = "application/x-www-form-urlencoded";
                apiRequest.accept = "application/json";
                apiRequest.setBinaryData(oauthParameters);

                if (apiService.post(apiRequest))
                {
                    accessTokenJson = apiService.apiResponse.getResponseData();
                    this.ResetTokenVariables();
                    return true;
                }
            }

            throw new Exception(apiService.errorResponse);
        }

        /// <summary>
        /// This function gets access token with post request
        /// </summary>
        public bool RevokeToken(TokenTypeHint tokenType, String token)
        {
            TokenTypeHint t = tokenType;

            var query = HttpUtility.ParseQueryString(String.Empty);

            string oauthParameters = String.Empty;

            switch (t)
            {
                case TokenTypeHint.AccessToken:

                    query["client_id"] = apiKey;
                    query["client_secret"] = secretKey;
                    query["token"] = token;
                    query["token_type_hint"] = "access_token";

                    break;

                case TokenTypeHint.RefreshToken:

                    query["client_id"] = apiKey;
                    query["client_secret"] = secretKey;
                    query["token"] = token;
                    query["token_type_hint"] = "refresh_token";

                    break;
            }
            oauthParameters = query.ToString();

            if (!String.IsNullOrEmpty(oauthParameters))
            {
                var revokeService = new APIService(endPoint, "/oauth/v4/");
                var apiRequest = new APIRequest("revoke");

                apiRequest.requireAccessToken = false;
                apiRequest.contentType = "application/x-www-form-urlencoded";
                apiRequest.setBinaryData(oauthParameters);


                if (revokeService.post(apiRequest))
                {
                    return true;
                }
            }

            throw new Exception(apiService.errorResponse);
            
        }

        /// <summary>
        /// This function Bypasses SSL if it's set to true
        /// </summary>
        private bool BypassCertificateError(string bypassSSL)
        {
            if ((!string.IsNullOrEmpty(bypassSSL))
                && (string.Equals(bypassSSL, "true", StringComparison.OrdinalIgnoreCase)))
            {
                ServicePointManager.ServerCertificateValidationCallback +=
                delegate(object sender1, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                {
                    return true;
                };
            }
            return false;
        }

        /// <summary>
        /// This function resets token variables
        /// </summary>
        private void ResetTokenVariables()
        {
            this.accessToken = null;
            this.refreshToken = null;
            this.refreshTokenExpiryTime = null;
            this.accessTokenExpiryTime = null;
        }
    }
}
