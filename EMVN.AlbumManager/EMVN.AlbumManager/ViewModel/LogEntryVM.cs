using EMVN.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMVN.AlbumManager.ViewModel
{
    public class LogEntryVM: BaseVM
    {
        public LogEntryVM(string message, DateTime timestamp)
        {
            this.Message = message;
            this.Timestamp = timestamp;
        }

        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
