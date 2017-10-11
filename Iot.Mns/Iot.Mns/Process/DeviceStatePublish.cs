using Iot.Mns.Core;
using Iot.Mns.Model;
using Iot.Mns.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Mns.Process
{
    public class DeviceStatePublish : IMnsProcessData
    {
        private ApiUtils apiUtils;

        public DeviceStatePublish()
        {
            apiUtils = new ApiUtils(Config.iotapidomain);
        }

        public async Task<bool> Process(string payload)
        {
            DeviceStatusModel dsEntity = JsonConvert.DeserializeObject<DeviceStatusModel>(payload);
            if (dsEntity == null || dsEntity.deviceName == null || dsEntity.deviceName == "")
            {
                return false;
            }

            string result = await apiUtils.GetAsync("/api/iot/mnscall/getdevicebinduser?devicename=" + dsEntity.deviceName);
            var jresult = JObject.Parse(result);
            if (jresult["statecode"] == null || jresult["statecode"].ToString() != "ok")
            {
                return false;
            }
            if (jresult["content"] == null || jresult["content"].ToString() == "")
            {
                return false;
            }
            List<DeviceBaseInfoShowModel> list = JsonConvert.DeserializeObject<List<DeviceBaseInfoShowModel>>(jresult["content"].ToString());
            if (list == null || list.Count == 0)
            {
                return false;
            }
            foreach (var ent in list)
            {
                if (CheckUserOnline.IsOnline(ent.productkey, new List<string>() { ent.devicename }))
                {
                    var data = new { devicename = dsEntity.deviceName, status = dsEntity.status };
                    PublishMsg.PubMsg(ent.productkey, ent.devicename,JsonConvert.SerializeObject(data));
                }
            }

            return true;
        }
    }
}
