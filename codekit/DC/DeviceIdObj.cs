using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace ATT.Codekit.DC
{
    public class DeviceIdObj
    {
        /// <summary>
        /// deserialize DeviceIdJSON json string to DeviceIdJSON object
        /// </summary>
        /// <param name="json">json</param>
        public DeviceIdObj.RootObject deserializeDeviceIdObj(string json)
        {
            var serializer = new JavaScriptSerializer();
            var deserializedJsonObj = serializer.Deserialize<DeviceIdObj.RootObject>(json);
            return deserializedJsonObj;
        }

        public class DeviceId
        {
            public string TypeAllocationCode { get; set; }
        }

        public class DeviceInfo
        {
            public DeviceId DeviceId { get; set; }
        }

        public class Capablities
        {
            public string Name { get; set; }
            public string Vendor { get; set; }
            public string Model { get; set; }
            public string FirmwareVersion { get; set; }
            public string UaProf { get; set; }
            public string MmsCapable { get; set; }
            public string AssistedGps { get; set; }
            public string LocationTechnology { get; set; }
            public string DeviceBrowser { get; set; }
            public string WapPushCapable { get; set; }
        }

        public class RootObject
        {
            public DeviceInfo DeviceInfo { get; set; }
            public Capablities Capablities { get; set; }
        }
    }
}
