using NLog;

namespace Iot.Mns.Utils
{
    public class LogHelper
    {
        private static Logger loger = LogManager.GetLogger("IOT_MNS");

        public static void Write(string body)
        {
            LogEventInfo logEvent = new LogEventInfo(LogLevel.Info, loger.Name, body);
            loger.Log(logEvent);
        }

        public static void Write(string category, string body)
        {
            LogEventInfo logEvent = new LogEventInfo(LogLevel.Info, loger.Name, "类别：" + category + "，信息：" + body);
            loger.Log(logEvent);
        }

        public static void Write(string ip, string url, string body)
        {
            LogEventInfo logEvent = new LogEventInfo(LogLevel.Info, loger.Name, "IP：" + ip + "，URL：" + url + "，信息：" + body);
            loger.Log(logEvent);
        }

    }
}
