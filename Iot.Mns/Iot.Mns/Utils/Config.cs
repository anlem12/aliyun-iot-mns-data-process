using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Mns.Utils
{
    public class Config
    {
        /// <summary>
        /// api域名
        /// </summary>
        public static string iotapidomain
        {
            get
            {
                return ConfigurationManager.AppSettings["iotapidomain"];
            }
        }

        public static string regionId
        {
            get
            {
                return ConfigurationManager.AppSettings["regionId"];
            }
        }

        public static string accessKeyId
        {
            get
            {
                return ConfigurationManager.AppSettings["accessKeyId"];
            }
        }

        public static string accessKeySecret
        {
            get
            {
                return ConfigurationManager.AppSettings["secretAccessKey"];
            }
        }

        public static string endpoint
        {
            get
            {
                return ConfigurationManager.AppSettings["endpoint"];
            }
        }

        /// <summary>
        /// MNS主题
        /// </summary>
        public static string topicName
        {
            get
            {
                return ConfigurationManager.AppSettings["topicName"];
            }
        }
    }
}
