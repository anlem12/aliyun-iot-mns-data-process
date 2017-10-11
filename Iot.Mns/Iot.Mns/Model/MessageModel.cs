using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Mns.Model
{
    public class MessageModel
    {
        public string messagetype { get; set; }

        public string payload { get; set; }

        public string messageid { get; set; }

        public string topic { get; set; }

        public long timestamp { get; set; }
    }
}
