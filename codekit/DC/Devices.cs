using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using ATT.Codekit.REST;

namespace ATT.Codekit.DC
{
    public class Devices
    {
        APIService apiService;

        /// <summary>
        /// SMS constructor
        /// </summary>
        public Devices(string endPoint, string accessToken)
        {
            apiService = new APIService(endPoint, accessToken, "/rest/2/Devices/");
        }

        public DeviceIdObj.RootObject GetDeviceCapabilities()
        {

            APIRequest apiRequest = new APIRequest("Info");

            apiRequest.accept = "application/json";

            if (apiService.get(apiRequest))
            {
                DeviceIdObj responseObj = new DeviceIdObj();
                return responseObj.deserializeDeviceIdObj(apiService.apiResponse.getResponseData());
            }
            return null;

        }

    }
}
