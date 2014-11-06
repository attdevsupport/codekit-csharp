using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using ATT.Codekit.REST;

namespace ATT.Codekit.ADS
{
    public class Ads
    {
        APIService apiService;
        /// <summary>
        /// Ads constructor
        /// </summary>
        /// <param name="endPoint">endPoint</param>
        /// <param name="accessToken">accessToken</param>
        public Ads(string endPoint, string accessToken)
        {
            apiService = new APIService(endPoint, accessToken, "/rest/1/");
        }

        public AdsObj.RootObject GetAds(string category, string udid, string userAgent)
        {
            try
            {
                APIRequest apiRequest = new APIRequest("ads?Category=" + category);

                apiRequest.accept = "application/json";
                apiRequest.addHeaders("Udid", udid);
                apiRequest.userAgent = userAgent;

                if (apiService.get(apiRequest))
                {
                    return new AdsObj().deserializeAdsObj(apiService.apiResponse.getResponseData());
                }

            }
            catch (Exception ex)
            {
                string error = ex.Message;
            }
            return null;
        }
    }
}
