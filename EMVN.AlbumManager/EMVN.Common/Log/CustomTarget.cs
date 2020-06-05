using NLog;
using NLog.Common;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMVN.Common.Log
{
    [Target("custom")]
    public class CustomTarget: TargetWithLayout
    {
        protected override void Write(LogEventInfo logEvent)
        {
            if (OnLog != null)
            {
                OnLog(logEvent.FormattedMessage, logEvent.TimeStamp);
            }
        }

        public static event OnLog OnLog;
    }

    public delegate void OnLog(string message, DateTime timestamp);
}
