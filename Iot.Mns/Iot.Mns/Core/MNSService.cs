using Aliyun.MNS;
using Aliyun.MNS.Model;
using Iot.Mns.Model;
using Iot.Mns.Process;
using Iot.Mns.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Iot.Mns.Core
{
    public delegate void RunningStateEventHandler(string status);
    public class MNSService
    {
        public event RunningStateEventHandler RunningState;

        #region Private Properties
        private string _accessKeyId = Config.accessKeyId;
        private string _secretAccessKey = Config.accessKeySecret;
        private string _endpoint = Config.endpoint;

        // const string _topicName = "aliyun-iot-23351938";


        private string _receiptHandle;
        private AutoResetEvent _autoSetEvent = new AutoResetEvent(false);

      

        #endregion

        public bool isRunning = true;
        public DateTime runCurrentDateTime = DateTime.Now;

        private static object lockObj = new object();

        public MNSService()
        {
          
        }

        public void StartMns(string _topicName)
        {
            try
            {

                IMNS client = new Aliyun.MNS.MNSClient(_accessKeyId, _secretAccessKey, _endpoint);

                var nativeQueue = client.GetNativeQueue(_topicName);
                var receiveMessageRequest = new ReceiveMessageRequest();
                receiveMessageRequest.WaitSeconds = 30;

                while (isRunning)
                {
                    try
                    {
                        nativeQueue.BeginReceiveMessage(receiveMessageRequest, ReceiveMessageCallback, nativeQueue);
                        _autoSetEvent.WaitOne();
                    }
                    catch (MNSException me)
                    {
                        throw new ArgumentException("1.接受MNS数据:" + me.Message + me.StackTrace);
                    }
                }

            }
            catch (Exception err)
            {
                throw new ArgumentException("接受MNS数据:" + err.Message + err.StackTrace);
            }
        }

        Message message = null;
        void ReceiveMessageCallback(IAsyncResult ar)
        {
            try
            {

                var nativeQueue = (Queue)ar.AsyncState;

                var response = nativeQueue.EndReceiveMessage(ar);
                message = response.Message;

#if DEBUG
                //Console.WriteLine("Async Receive message successfully, status code: {0}", response.HttpStatusCode);
                //Console.WriteLine("----------------------------------------------------");

                //Console.WriteLine("MessageId: {0}", message.Id);
                //Console.WriteLine("ReceiptHandle: {0}", message.ReceiptHandle);
                //Console.WriteLine("MessageBody: {0}", message.Body);
                //Console.WriteLine("MessageBodyMD5: {0}", message.BodyMD5);
                //Console.WriteLine("EnqueueTime: {0}", message.EnqueueTime);
                //Console.WriteLine("NextVisibleTime: {0}", message.NextVisibleTime);
                //Console.WriteLine("FirstDequeueTime: {0}", message.FirstDequeueTime);
                //Console.WriteLine("DequeueCount: {0}", message.DequeueCount);
                //Console.WriteLine("Priority: {0}", message.Priority);
                //Console.WriteLine("----------------------------------------------------\n");
#endif
                lock (lockObj)
                {
                    ParseMessage(message.Body);
                }

                if (RunningState != null)
                {
                    runCurrentDateTime = DateTime.Now;
                    RunningState("消息接受成功：" + runCurrentDateTime.ToString() + "，MessageBody：" + GetBase64ToString(message.Body));
                }

                _receiptHandle = message.ReceiptHandle;

                var deleteMessageRequest = new DeleteMessageRequest(_receiptHandle);
                nativeQueue.BeginDeleteMessage(deleteMessageRequest, DeleteMessageCallback, nativeQueue);

                _autoSetEvent.Set();

            }

            catch (Exception ex)
            {
                if (RunningState != null)
                {
                    runCurrentDateTime = DateTime.Now;
                    RunningState("ReceiveMessageCallback异常：" + runCurrentDateTime.ToString() + "，Exception：" + ex.Message + (ex.InnerException).Message);
                }
                Console.WriteLine("Async Receive message failed, exception info: " + ex.Message + ex.GetType().Name);
                _autoSetEvent.Set();
            }
        }

        void DeleteMessageCallback(IAsyncResult ar)
        {
            try
            {
                if (ar.AsyncState == null) return;
                var nativeQueue = (Queue)ar.AsyncState;
                if (nativeQueue != null)
                {
                    var deleteMessageResponse = nativeQueue.EndDeleteMessage(ar);
                    Console.WriteLine("Async Delete message successfully, status code: {0}", deleteMessageResponse.HttpStatusCode);
                }
                _autoSetEvent.Set();
            }
            catch (MNSException ex)
            {
                Console.WriteLine("Async Delete message failed, exception info: " + ex.Message + (ex.InnerException).Message);
                if (RunningState != null)
                {
                    runCurrentDateTime = DateTime.Now;
                    RunningState("DeleteMessageCallback异常：" + runCurrentDateTime.ToString() + "，Exception：" + ex.Message);
                }
                _autoSetEvent.Set();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Async Delete message failed, exception info: " + ex.Message + (ex.InnerException).Message);
                if (RunningState != null)
                {
                    runCurrentDateTime = DateTime.Now;
                    RunningState("DeleteMessageCallback异常：" + runCurrentDateTime.ToString() + "，Exception：" + ex.Message);
                }
                _autoSetEvent.Set();
            }
        }

        async void ParseMessage(string msgBody)
        {
            try
            {

                string scon = GetBase64ToString(msgBody);
                Console.WriteLine("body:{0}", scon);
                MessageModel msgModel = JsonConvert.DeserializeObject<MessageModel>(scon);

                string payload = GetBase64ToString(msgModel.payload);
                Console.WriteLine("payload:{0}", payload);

                if (msgModel != null && msgModel.messagetype.ToLower() == "status")
                {
                    IMnsProcessData imnsp = (IMnsProcessData)Assembly.Load("Iot.Mns").CreateInstance(ConfigurationManager.AppSettings[msgModel.messagetype.ToLower() + "_typeName"]);
                
                    await imnsp.Process(payload);
                }

                //if (msgModel != null && msgModel.messagetype.ToLower() == "upload")
                //{
                //    DeviceSendMessageModel dsmEntity = JsonConvert.DeserializeObject<DeviceSendMessageModel>(payload);

                    //    if (dsmEntity == null || dsmEntity.todevice == null || dsmEntity.todevice == "")
                    //    {
                    //        return;
                    //    }

                    //    if (dsmEntity.todevice.ToLower() == "server")
                    //    {
                    //        var joServer = JObject.Parse(dsmEntity.content);
                    //        if (joServer["action"] != null && joServer["action"].ToString() == "userbinddevice")
                    //        {
                    //            long userid = 0;
                    //            if (long.TryParse(joServer["user"].ToString(), out userid))
                    //            {
                    //                await UserBindDeviceDAL.UserBindDevice(dsmEntity.deviceid, userid, "127.0.0.1", "WIFI");
                    //            }

                    //        }
                    //        LeServerPubMsgToDevice.Pub(Convert.ToInt64(dsmEntity.productkey), "/" + dsmEntity.productkey + "/" + dsmEntity.devicename + "/get", BuildLight());
                    //    }
                    //    else if (dsmEntity.todevice.ToLower() == "app")
                    //    {
                    //        //通过设备ID，查出与设备绑定的用户ID列表
                    //        List<string> lstUser = await UserBindDeviceDAL.GetUID(dsmEntity.deviceid);
                    //        var resp = new { deviceid = dsmEntity.deviceid, time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), content = dsmEntity.content };
                    //        foreach (string sUser in lstUser)
                    //        {
                    //            LeServerPubMsgToDevice.Pub(IOTConfig.APP_Client_IOT_ProductKey, string.Format("/1000153773/le{0}/get", sUser), JsonConvert.SerializeObject(resp));
                    //        }
                    //    }
                    //}
                    //else if (msgModel != null && msgModel.messagetype.ToLower() == "status")
                    //{
                    //    DeviceStatusModel dsEntity = JsonConvert.DeserializeObject<DeviceStatusModel>(payload);
                    //    if (dsEntity == null || dsEntity.deviceId == null || dsEntity.deviceId == "")
                    //    {
                    //        return;
                    //    }
                    //    //通过设备ID，查出与设备绑定的用户ID列表
                    //    List<string> lstUser = await UserBindDeviceDAL.GetUID(dsEntity.deviceId);
                    //    foreach (string sUser in lstUser)
                    //    {
                    //        LeServerPubMsgToDevice.Pub(IOTConfig.APP_Client_IOT_ProductKey, string.Format("/1000153773/le{0}/get", sUser), payload);
                    //    }

                    //    await DeviceStatusDAL.UpdateDeviceOnlineStatus(dsEntity.deviceId, dsEntity.status);
                    //}
            }
            catch (Exception err)
            {
                if (RunningState != null)
                {
                    runCurrentDateTime = DateTime.Now;
                    RunningState("ParseMessage异常：" + runCurrentDateTime.ToString() + "，Exception：" + err.Message);
                }
                throw new ArgumentException("MNS_Servi,消息解析,ParseMessage：" + err.Message + err.StackTrace);
            }
        }

        private string BuildLight()
        {
            int i1 = new Random().Next(1, 255);

            int i2 = new Random().Next(1, i1);

            int i3 = new Random().Next(1, i2);
            var light = new { state = "light", red = i1.ToString(), green = i2.ToString(), blue = i3.ToString() };

            return JsonConvert.SerializeObject(light);
        }


        private string GetBase64ToString(string sContent)
        {
            try
            {
                byte[] bpath = Convert.FromBase64String(sContent);
                return System.Text.ASCIIEncoding.UTF8.GetString(bpath);
            }
            catch (Exception err)
            {
                throw err;
            }
        }



    }
}
