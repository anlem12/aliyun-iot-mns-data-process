using Aliyun.Acs.Core;
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
    public class PublishMsg
    {
        /// <summary>
        /// 发送消息到设备
        /// </summary>
        /// <param name="device_productKey">产品Key</param>
        /// <param name="device_topic">设备topic</param>
        /// <param name="msgContent">消息内容</param>
        public static bool PubMsg(string device_productKey, string device_name, string msgContent)
        {
            IClientProfile clientProfile = DefaultProfile.GetProfile(Config.regionId, Config.accessKeyId, Config.accessKeySecret);
            DefaultAcsClient client = new DefaultAcsClient(clientProfile);
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(msgContent);
                string strContent = Convert.ToBase64String(bytes);

                PubRequest pub = new PubRequest();
                pub.ProductKey = device_productKey;
                pub.MessageContent = strContent;
                pub.TopicFullName = string.Format("/{0}/{1}/get",device_productKey,device_name);
                pub.Qos = 0;
                PubResponse resp = client.GetAcsResponse(pub);
                return true;
            }
            catch (Exception err)
            {
                Console.Write(err.Message);
                return false;
            }
        }
    }
}
