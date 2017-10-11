using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Iot.Model.V20170420;
using Iot.Mns.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Mns.Core
{
    /// <summary>
    /// 查询设备是否在线
    /// </summary>
    public class CheckUserOnline
    {
        /// <summary>
        /// 查询设备是否在线
        /// </summary>
        /// <param name="product_key">产品Key</param>
        /// <param name="lstDevicesName">要查询的设备名称列表</param>
        /// <returns></returns>
        public static bool IsOnline(string product_key, List<string> lstDevicesName)
        {
            IClientProfile clientProfile = DefaultProfile.GetProfile(Config.regionId, Config.accessKeyId, Config.accessKeySecret);
            DefaultAcsClient client = new DefaultAcsClient(clientProfile);
            
            BatchGetDeviceStateRequest request = new BatchGetDeviceStateRequest();
            request.ProductKey = product_key;

            request.DeviceNames = lstDevicesName;
            BatchGetDeviceStateResponse response = null;
            try
            {
                response = client.GetAcsResponse(request);
            }
            catch (ClientException err)
            {
                throw err;
            }
            if (response != null)
            {
                List<BatchGetDeviceStateResponse.DeviceStatus> lstDeviceStatus = response.DeviceStatusList;
                if (lstDeviceStatus.Count > 0 && lstDeviceStatus[0].Status == "ONLINE")
                {
                    return true;
                }
                Console.WriteLine("Response requestId:" + response.RequestId + " isSuccess:" + response.Success + " Error:" + response.ErrorMessage);
            }
            return false;

        }
    }
}
