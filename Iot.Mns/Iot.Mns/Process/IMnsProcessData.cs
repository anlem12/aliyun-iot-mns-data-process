using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Mns.Process
{
    public interface IMnsProcessData
    {
        Task<bool> Process(string payload);
    }
}
