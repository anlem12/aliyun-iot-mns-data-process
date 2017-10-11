using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Mns.Model
{
    public class DeviceStatusModel
    {
        public string lastTime { get; set; }

        public string time { get; set; }

        public string productKey { get; set; }

        public string deviceId { get; set; }

        public string deviceName { get; set; }

        public string status { get; set; }
    }
}
