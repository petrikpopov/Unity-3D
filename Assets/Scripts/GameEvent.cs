using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEvents
{
    public interface IMessage { string message { get; } }

    public class MessageEvent : IMessage
    {
        public object data { get; set; }
        public string message { get; set; }
    }

    public class KeyPointEvent : IMessage
    {
        public string keyName { get; set; }
        public bool isInTime { get; set; }
        public string message { get; set; }
    }

    public class GateEvent : IMessage
    {
        public string gateName { get; set; }
        public string message { get; set; }
    }
}
